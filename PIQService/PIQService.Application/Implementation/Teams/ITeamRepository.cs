using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Teams;

public interface ITeamRepository
{
    Task<IEnumerable<Team>> FindAllByTutorIdAsync(Guid tutorId);
}