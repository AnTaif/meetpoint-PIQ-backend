using Core.Auth;
using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters;
using PIQService.Models.Domain;
using PIQService.Models.Dto;
using PIQService.Models.Dto.Responses;

namespace PIQService.Application.Implementation.Events;

public class EventService(
    IEventRepository eventRepository,
    ITeamRepository teamRepository,
    ILogger<EventService> logger
)
    : IEventService
{
    public async Task<Result<Event?>> FindCurrentEventAsync()
    {
        var activeEvents = await eventRepository.SelectActiveAsync(DateTime.UtcNow);
        return activeEvents.FirstOrDefault();
    }

    public async Task<Result<GetEventHierarchyResponse>> GetEventHierarchyForUserAsync(ContextUser contextUser, Guid? eventId = null,
        bool onlyWhereTutor = true)
    {
        Event? @event;
        if (!eventId.HasValue)
        {
            var findCurrentEventResult = await FindCurrentEventAsync();

            if (findCurrentEventResult.IsFailure)
                return findCurrentEventResult.Error;

            @event = findCurrentEventResult.Value;
        }
        else
        {
            @event = await eventRepository.FindAsync(eventId.Value);
        }

        if (@event == null)
        {
            return StatusError.NotFound("Event not found");
        }

        List<Team> teams;
        if (contextUser.Roles.Contains(RolesConstants.Admin) && !onlyWhereTutor)
        {
            teams = await teamRepository.SelectAsync(@event.Id);
        }
        else if (contextUser.Roles.Contains(RolesConstants.Tutor) && onlyWhereTutor)
        {
            teams = await teamRepository.SelectByTutorIdAsync(contextUser.Id, @event.Id);
        }
        else if (contextUser.Roles.Contains(RolesConstants.Student))
        {
            teams = await teamRepository.SelectByStudentIdAsync(contextUser.Id, @event.Id);
        }
        else
        {
            logger.LogWarning(
                "Не смогли разобраться, какую иерархию мероприятия ({eventId}) вернуть юзеру: contextUserId = {userId}, contextUserRoles = {roles}",
                @event.Id,
                contextUser.Id,
                string.Join(',', contextUser.Roles)
            );
            teams = [];
        }

        var requiresEvaluationTeams = await teamRepository.SelectNotAssessedTeamsAsync(contextUser.Id);

        return new GetEventHierarchyResponse
        {
            Event = new EventDto
            {
                Id = @event.Id,
                Name = @event.Name,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                Directions = ConvertTeamsToDirectionDtos(teams),
            },
            TeamIdsForEvaluation = requiresEvaluationTeams.Select(t => t.Id),
        };
    }

    // TODO(!!!): сто процентов здесь неоптимальные перечисления
    private static IEnumerable<DirectionDto> ConvertTeamsToDirectionDtos(IEnumerable<Team> teams)
    {
        var teamList = teams.ToList();
        var directions = teamList.Select(t => t.Project.Direction).DistinctBy(d => d.Id).ToList();
        var projects = teamList.Select(t => t.Project).DistinctBy(t => t.Id).ToList();

        var directionIdToProjects = new Dictionary<Guid, List<Project>>();
        foreach (var direction in directions)
        {
            var projectsByDirection = projects.Where(p => p.Direction.Id == direction.Id).ToList();
            directionIdToProjects.Add(direction.Id, projectsByDirection);
        }

        var projectIdToTeams = new Dictionary<Guid, List<Team>>();
        foreach (var project in projects)
        {
            var teamsByProject = teamList.Where(t => t.Project.Id == project.Id).ToList();
            projectIdToTeams.Add(project.Id, teamsByProject);
        }

        return directions.Select(d => new DirectionDto
        {
            Id = d.Id,
            Name = d.Name,
            Projects = directionIdToProjects[d.Id].Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Teams = projectIdToTeams[p.Id].Select(t => new TeamDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Tutor = t.Tutor.ToDtoModel(),
                    Members = t.Users.Select(u => u.ToDtoModel()),
                }),
            }),
        }).ToList();
    }
}