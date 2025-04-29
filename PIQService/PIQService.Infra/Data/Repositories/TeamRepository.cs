using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters;
using PIQService.Models.Domain;

namespace PIQService.Infra.Data.Repositories;

public class TeamRepository(AppDbContext dbContext) : ITeamRepository
{
    public async Task<IEnumerable<Team>> SelectByTutorIdAsync(Guid tutorId, Guid eventId)
    {
        var teams = await dbContext.Teams
            .Include(t => t.Tutor)
            .Include(t => t.Members)
            .Include(t => t.Project)
            .ThenInclude(p => p.Direction)
            .ThenInclude(d => d.Event)
            .Where(t => t.Project.Direction.Event.Id == eventId)
            .Where(t => t.TutorId == tutorId)
            .ToListAsync();

        return teams.Select(t => t.ToDomainModel()).ToList();
    }
}