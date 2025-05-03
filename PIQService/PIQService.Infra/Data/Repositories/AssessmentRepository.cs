using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Assessments;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

public class AssessmentRepository(AppDbContext dbContext) : IAssessmentRepository
{
    public async Task<IEnumerable<AssessmentWithoutDeps>> SelectByTeamIdAsync(Guid teamId)
    {
        var dbos = await dbContext.Assessments
            .Where(a => a.Teams.Any(t => t.Id == teamId))
            .OrderByDescending(a => a.StartDate)
            .ToListAsync();

        return dbos.Select(x => x.ToDomainWithoutDepsModel()).ToList();
    }

    public async Task CreateAsync(Assessment assessment, params Team[] teams)
    {
        var assessmentDbo = assessment.ToDboModel();

        foreach (var team in teams)
        {
            var teamDbo = dbContext.Teams.Local.FirstOrDefault(t => t.Id == team.Id) ?? await dbContext.Teams.FindAsync(team.Id);

            if (teamDbo == null)
            {
                throw new InvalidOperationException("Team not found in database.");
            }

            assessmentDbo.Teams.Add(teamDbo);
        }

        dbContext.Assessments.Add(assessmentDbo);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}