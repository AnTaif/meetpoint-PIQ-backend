using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Teams;

public interface ITeamService
{
    Task<Result<IEnumerable<TeamDto>>> GetTeamsByTutorIdAsync(Guid tutorId);
}