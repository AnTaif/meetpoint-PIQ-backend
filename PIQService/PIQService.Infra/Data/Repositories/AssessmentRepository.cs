using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Assessments;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class AssessmentRepository(AppDbContext dbContext) : IAssessmentRepository
{
    public async Task<AssessmentWithoutDeps?> FindWithoutDepsAsync(Guid id)
    {
        var dbo = await dbContext.Assessments.FindAsync(id);
        return dbo?.ToDomainWithoutDepsModel();
    }

    public async Task<IReadOnlyList<AssessmentWithoutDeps>> SelectByTeamIdAsync(Guid teamId)
    {
        var dbos = await dbContext.Assessments
            .Where(a => a.Team.Id == teamId)
            .OrderByDescending(a => a.StartDate)
            .ThenByDescending(a => a.EndDate)
            .ToListAsync();

        return dbos.Select(x => x.ToDomainWithoutDepsModel()).ToList();
    }

    public void Create(AssessmentWithoutDeps assessment)
    {
        var assessmentDbo = assessment.ToDboModel();
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
        var dbo = dbContext.Assessments.Find(assessmentWithoutDeps.Id);
        dbContext.Assessments.Remove(dbo!);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}