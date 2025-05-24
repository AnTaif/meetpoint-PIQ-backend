using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Teams;

public interface ITeamRepository
{
    Task<Team?> FindAsync(Guid teamId);

    Task<TeamWithoutDeps?> FindWithoutDepsAsync(Guid teamId);

    Task<List<Team>> SelectAsync(Guid eventId);
    
    Task<List<Team>> SelectByTutorIdAsync(Guid tutorId, Guid eventId);

    Task<List<Team>> SelectNotAssessedTeamsAsync(Guid tutorId);
}