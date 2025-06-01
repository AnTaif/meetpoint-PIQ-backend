using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Teams;

public interface ITeamRepository
{
    Task<Team?> FindAsync(Guid teamId);

    Task<TeamWithoutDeps?> FindWithoutDepsAsync(Guid teamId);

    Task<List<Team>> SelectAsync(IEnumerable<Guid> teamIds);

    Task<List<TeamWithoutDeps>> SelectWithoutDepsAsync(IEnumerable<Guid> teamIds);
    
    Task<List<Team>> SelectByEventIdAsync(Guid eventId, IEnumerable<Guid>? byTeams = null);

    Task<List<Team>> SelectByStudentIdAsync(Guid studentId, Guid eventId, IEnumerable<Guid>? byTeams = null);
    
    Task<List<Team>> SelectByTutorIdAsync(Guid tutorId, Guid eventId, IEnumerable<Guid>? byTeams = null);

    Task<List<Team>> SelectNotAssessedTeamsAsync(Guid tutorId);
}