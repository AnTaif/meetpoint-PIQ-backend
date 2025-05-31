using Core.Auth;
using Core.Results;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters;
using PIQService.Models.Domain;
using PIQService.Models.Dto;
using PIQService.Models.Dto.Responses;

namespace PIQService.Application.Implementation.Hierarchies;

[RegisterScoped]
public class HierarchyService(
    HybridCache cache,
    IEventService eventService,
    ITeamRepository teamRepository,
    ILogger<HierarchyService> logger
)
    : IHierarchyService
{
    public async Task<Result<GetHierarchyResponse>> GetHierarchyForEventByUserAsync(
        Guid? eventId, ContextUser contextUser, bool onlyWhereTutor = true)
    {
        var currentEvent = await eventService.FindEventWithoutDepsAsync(eventId);
        if (currentEvent == null)
        {
            return StatusError.NotFound("Current event not found");
        }

        var teams = await ResolveTeamsForEventByUserAsync(currentEvent.Id, contextUser, onlyWhereTutor);

        var assessmentRequiredTeamIds = await cache.GetOrCreateAsync<HashSet<Guid>>(
            $"requires_evaluation_by_user_{contextUser.Id}",
            async _ => (await teamRepository.SelectNotAssessedTeamsAsync(contextUser.Id)).Select(t => t.Id).ToHashSet()
        );

        return new GetHierarchyResponse
        {
            Event = new EventHierarchyDto
            {
                Id = currentEvent.Id,
                Name = currentEvent.Name,
                StartDate = currentEvent.StartDate,
                EndDate = currentEvent.EndDate,
                Directions = ConvertTeamsToDirectionDtos(teams, assessmentRequiredTeamIds),
            }
        };
    }

    public async Task<Result<GetHierarchyResponse>> GetHierarchyForTeamAsync(Guid teamId, Guid? eventId, ContextUser contextUser)
    {
        var currentEvent = await eventService.FindEventWithoutDepsAsync(eventId);
        if (currentEvent == null)
        {
            return StatusError.NotFound("Current event not found");
        }

        var team = await teamRepository.FindAsync(teamId);
        if (team == null)
        {
            return StatusError.NotFound("Team not found");
        }

        var assessmentRequiredTeamIds = await cache.GetOrCreateAsync(
            $"requires_evaluation_by_user_{contextUser.Id}",
            async _ => (await teamRepository.SelectNotAssessedTeamsAsync(contextUser.Id)).Select(t => t.Id).ToHashSet()
        );

        return new GetHierarchyResponse
        {
            Event = new EventHierarchyDto
            {
                Id = currentEvent.Id,
                Name = currentEvent.Name,
                StartDate = currentEvent.StartDate,
                EndDate = currentEvent.EndDate,
                Directions = ConvertTeamsToDirectionDtos([team], assessmentRequiredTeamIds),
            },
        };
    }

    private async Task<List<Team>> ResolveTeamsForEventByUserAsync(Guid eventId, ContextUser contextUser, bool byTutor)
    {
        if (contextUser.Roles.Contains(RolesConstants.Admin) && !byTutor)
        {
            return await teamRepository.SelectByEventIdAsync(eventId);
        }

        if (contextUser.Roles.Contains(RolesConstants.Tutor))
        {
            return await teamRepository.SelectByTutorIdAsync(contextUser.Id, eventId);
        }

        if (contextUser.Roles.Contains(RolesConstants.Student))
        {
            return await teamRepository.SelectByStudentIdAsync(contextUser.Id, eventId);
        }

        logger.LogError(
            "Не смогли разобраться, какую иерархию мероприятия (id={eventId}) вернуть юзеру: contextUserId = {userId}, contextUserRoles = {roles}",
            eventId,
            contextUser.Id,
            string.Join(',', contextUser.Roles)
        );
        return [];
    }

    // TODO: дальше бога нет
    private static List<DirectionHierarchyDto> ConvertTeamsToDirectionDtos(List<Team> teams, HashSet<Guid> assessmentRequiredTeamIds)
    {
        var directions = teams.Select(t => t.Project.Direction).DistinctBy(d => d.Id).ToList();
        var projects = teams.Select(t => t.Project).DistinctBy(t => t.Id).ToList();

        var directionIdToProjects = new Dictionary<Guid, List<Project>>();
        foreach (var direction in directions)
        {
            var projectsByDirection = projects.Where(p => p.Direction.Id == direction.Id).ToList();
            directionIdToProjects.Add(direction.Id, projectsByDirection);
        }

        var projectIdToTeams = new Dictionary<Guid, List<Team>>();
        foreach (var project in projects)
        {
            var teamsByProject = teams.Where(t => t.Project.Id == project.Id).ToList();
            projectIdToTeams.Add(project.Id, teamsByProject);
        }

        return directions.Select(d => new DirectionHierarchyDto()
        {
            Id = d.Id,
            Name = d.Name,
            Projects = directionIdToProjects[d.Id].Select(p => new ProjectHierarchyDto
            {
                Id = p.Id,
                Name = p.Name,
                Teams = projectIdToTeams[p.Id].Select(t => new TeamHierarchyDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    AssessmentRequired = assessmentRequiredTeamIds.Contains(t.Id),
                    Tutor = t.Tutor.ToDtoModel(),
                    Members = t.Users.Select(u => u.ToDtoModel()),
                }),
            }),
        }).ToList();
    }
}