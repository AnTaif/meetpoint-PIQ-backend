using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Assessments;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

public class AssessmentRepository(AppDbContext dbContext) : IAssessmentRepository
{
    public async Task<AssessmentWithoutDeps?> FindWithoutDepsAsync(Guid id)
    {
        var dbo = await dbContext.Assessments.FindAsync(id);
        return dbo?.ToDomainWithoutDepsModel();
    }

    public async Task<IEnumerable<AssessmentWithoutDeps>> SelectByTeamIdAsync(Guid teamId)
    {
        var dbos = await dbContext.Assessments
            .Where(a => a.Teams.Any(t => t.Id == teamId))
            .OrderByDescending(a => a.StartDate)
            .ThenByDescending(a => a.EndDate)
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

    public void Update(AssessmentWithoutDeps assessmentWithoutDeps)
    {
        var existingEntity = dbContext.Assessments.Find(assessmentWithoutDeps.Id);
        if (existingEntity != null)
        {
            dbContext.Entry(existingEntity).CurrentValues.SetValues(assessmentWithoutDeps.ToDboModel());
        }
    }

    public void Delete(AssessmentWithoutDeps assessmentWithoutDeps)
    {
        dbContext.Assessments.Remove(assessmentWithoutDeps.ToDboModel());
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}