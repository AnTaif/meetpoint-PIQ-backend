using Core.Results;
using PIQService.Models.Converters;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Teams;

public class TeamService : ITeamService
{
    private readonly ITeamRepository teamRepository;

    public TeamService(ITeamRepository teamRepository)
    {
        this.teamRepository = teamRepository;
    }

    public async Task<Result<IEnumerable<TeamDto>>> GetTeamsByTutorIdAsync(Guid tutorId)
    {
        var teams = await teamRepository.FindAllByTutorIdAsync(tutorId);

        return teams.Select(t => t.ToDtoModel()).ToList();
    }
}