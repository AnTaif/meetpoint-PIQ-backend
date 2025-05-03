using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Teams;

public interface ITeamRepository
{
    Task<Team?> FindAsync(Guid teamId);

    Task<IEnumerable<Team>> SelectByTutorIdAsync(Guid tutorId, Guid eventId);

    Task<IEnumerable<Team>> SelectNotAssessedTeamsAsync(Guid tutorId);
}