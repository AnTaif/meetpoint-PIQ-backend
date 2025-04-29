using Core.Results;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters;
using PIQService.Models.Domain;
using PIQService.Models.Dto;
using PIQService.Models.Dto.Responses;

namespace PIQService.Application.Implementation.Events;

public class EventService : IEventService
{
    private readonly IEventRepository eventRepository;
    private readonly ITeamRepository teamRepository;

    public EventService(
        IEventRepository eventRepository,
        ITeamRepository teamRepository
    )
    {
        this.eventRepository = eventRepository;
        this.teamRepository = teamRepository;
    }

    public async Task<Result<GetEventWithIncludesResponse>> GetEventWithIncludesByUserIdAsync(Guid userId, Guid? eventId = null)
    {
        Event? @event;
        if (!eventId.HasValue)
        {
            var activeEvents = await eventRepository.SelectActiveAsync(DateTime.UtcNow);

            @event = activeEvents.FirstOrDefault(); // TODO: сравнивать с командой юзера, чтобы гарантированно выбрать именно его меро
        }
        else
        {
            @event = await eventRepository.FindAsync(eventId.Value);
        }

        if (@event == null)
        {
            return HttpError.NotFound("Event not found");
        }

        var teams = await teamRepository.SelectByTutorIdAsync(userId, @event.Id);

        return new GetEventWithIncludesResponse
        {
            Event = new EventDto
            {
                Id = @event.Id,
                Name = @event.Name,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                Directions = ConvertTeamsToDirectionDtos(teams),
            },
            TeamIdsForEvaluation = requiresEvaluationTeams,
        };
    }

    // TODO: получать из оцениваний
    private readonly IEnumerable<Guid> requiresEvaluationTeams =
    [
        Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5"),
    ];

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