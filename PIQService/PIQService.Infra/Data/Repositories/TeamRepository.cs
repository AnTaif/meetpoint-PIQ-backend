using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters;
using PIQService.Models.Domain;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class TeamRepository(AppDbContext dbContext) : ITeamRepository
{
    public async Task<Team?> FindAsync(Guid teamId)
    {
        var team = await dbContext.Teams
            .Include(t => t.Project)
            .ThenInclude(p => p.Direction)
            .ThenInclude(d => d.Event)
            .Include(t => t.Members)
            .Include(t => t.Tutor)
            .SingleOrDefaultAsync(t => t.Id == teamId);
        return team?.ToDomainModel();
    }

    public async Task<TeamWithoutDeps?> FindWithoutDepsAsync(Guid teamId)
    {
        var team = await dbContext.Teams.FindAsync(teamId);

        return team?.ToDomainModelWithoutDeps();
    }

    public async Task<List<Team>> SelectAsync(IEnumerable<Guid> teamIds)
    {
        var dbos = await dbContext.Teams
            .Include(t => t.Project)
            .ThenInclude(p => p.Direction)
            .ThenInclude(d => d.Event)
            .Include(t => t.Members)
            .Include(t => t.Tutor)
            .Where(t => teamIds.Contains(t.Id))
            .ToListAsync();

        return dbos.Select(t => t.ToDomainModel()).ToList();
    }

    public async Task<List<TeamWithoutDeps>> SelectWithoutDepsAsync(IEnumerable<Guid> teamIds)
    {
        var dbos = await dbContext.Teams
            .Where(t => teamIds.Contains(t.Id))
            .ToListAsync();

        return dbos.Select(t => t.ToDomainModelWithoutDeps()).ToList();
    }

    public async Task<List<Team>> SelectByEventIdAsync(Guid eventId, IEnumerable<Guid>? byTeams = null)
    {
        var query = dbContext.Teams.AsQueryable();
        
        if (byTeams != null)
        {
            query = query.Where(t => byTeams.Contains(t.Id));
        }
        
        var teams = await query
            .Include(t => t.Tutor)
            .Include(t => t.Members)
            .Include(t => t.Project)
            .ThenInclude(p => p.Direction)
            .ThenInclude(d => d.Event)
            .Where(t => t.Project.Direction.Event.Id == eventId)
            .OrderBy(t => t.Project.Direction.Event.Name)
            .ThenBy(t => t.Project.Direction.Name)
            .ThenBy(t => t.Project.Name)
            .ThenBy(t => t.Name)
            .ToListAsync();

        return teams.Select(t => t.ToDomainModel()).ToList();
    }

    public async Task<List<Team>> SelectByStudentIdAsync(Guid studentId, Guid eventId, IEnumerable<Guid>? byTeams = null)
    {
        var query = dbContext.Teams.AsQueryable();
        
        if (byTeams != null)
        {
            query = query.Where(t => byTeams.Contains(t.Id));
        }
        
        var teams = await query
            .Include(t => t.Tutor)
            .Include(t => t.Members)
            .Include(t => t.Project)
            .ThenInclude(p => p.Direction)
            .ThenInclude(d => d.Event)
            .Where(t => t.Project.Direction.Event.Id == eventId)
            .Where(t => t.Members.Any(m => m.Id == studentId))
            .OrderBy(t => t.Project.Direction.Event.Name)
            .ThenBy(t => t.Project.Direction.Name)
            .ThenBy(t => t.Project.Name)
            .ThenBy(t => t.Name)
            .ToListAsync();

        return teams.Select(t => t.ToDomainModel()).ToList();
    }

    public async Task<List<Team>> SelectByTutorIdAsync(Guid tutorId, Guid eventId, IEnumerable<Guid>? byTeams = null)
    {
        var query = dbContext.Teams.AsQueryable();
        
        if (byTeams != null)
        {
            query = query.Where(t => byTeams.Contains(t.Id));
        }

        var teams = await query
            .Include(t => t.Tutor)
            .Include(t => t.Members)
            .Include(t => t.Project)
            .ThenInclude(p => p.Direction)
            .ThenInclude(d => d.Event)
            .Where(t => t.Project.Direction.Event.Id == eventId)
            .Where(t => t.TutorId == tutorId)
            .OrderBy(t => t.Project.Direction.Event.Name)
            .ThenBy(t => t.Project.Direction.Name)
            .ThenBy(t => t.Project.Name)
            .ThenBy(t => t.Name)
            .ToListAsync();

        return teams.Select(t => t.ToDomainModel()).ToList();
    }

    public async Task<List<Team>> SelectNotAssessedTeamsAsync(Guid tutorId)
    {
        var now = DateTime.UtcNow;

        var teams = await dbContext.Teams
            .Where(team => team.TutorId == tutorId)
            .Where(team => dbContext.Assessments.Any(assessment =>
                assessment.StartDate <= now &&
                assessment.EndDate >= now &&
                assessment.Team.Id == team.Id &&
                team.Members.Any(member =>
                    !dbContext.AssessmentMarks.Any(mark =>
                        mark.AssessmentId == assessment.Id &&
                        mark.AssessorId == tutorId &&
                        mark.AssessedId == member.Id
                    )
                )
            ))
            .ToListAsync();

        return teams.Select(t => t.ToDomainModel()).ToList();
    }
}