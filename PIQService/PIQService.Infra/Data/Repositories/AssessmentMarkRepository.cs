using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Assessments.Marks;
using PIQService.Models.Converters;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class AssessmentMarkRepository(AppDbContext dbContext) : IAssessmentMarkRepository
{
    public async Task<AssessmentMarkWithoutDeps?> FindWithoutDepsAsync(Guid assessmentId, Guid assessorId, Guid assessedId)
    {
        var mark = await dbContext.AssessmentMarks
            .Include(m => m.Choices)
            .SingleOrDefaultAsync(a =>
                a.AssessmentId == assessmentId
                && a.AssessorId == assessorId
                && a.AssessedId == assessedId);

        return mark?.ToDomainModelWithoutDeps();
    }

    public void Create(AssessmentMarkWithoutDeps mark, IEnumerable<Guid> choiceIds)
    {
        var dbo = mark.ToDboModel();

        var choices = choiceIds.Select(id => dbContext.Choices.Find(id)!).ToList();
        dbo.Choices = choices;

        dbContext.AssessmentMarks.Add(dbo);
    }

    public void UpdateChoices(AssessmentMarkWithoutDeps mark, IEnumerable<Guid> choiceIds)
    {
        var markDbo = dbContext.AssessmentMarks.Find(mark.Id)!;

        var choices = choiceIds.Select(id => dbContext.Choices.Find(id)!).ToList();
        markDbo.Choices = choices;

        dbContext.AssessmentMarks.Update(markDbo);
    }

    public async Task<IEnumerable<User>> SelectAssessedUsersAsync(Guid assessorId, Guid assessmentId)
    {
        var assessedUsers = await dbContext.AssessmentMarks
            .Include(m => m.Assessed)
            .Include(m => m.Choices)
            .Where(m => m.AssessmentId == assessmentId)
            .Where(m => m.AssessorId == assessorId)
            .Where(m => m.Choices.Any())
            .Select(m => m.Assessed)
            .ToListAsync();

        return assessedUsers.Select(u => u.ToDomainModel()).ToList();
    }

    public async Task<IReadOnlyCollection<AssessmentMarkWithoutDeps>> SelectByAssessedUserIdAsync(Guid assessedUserId, Guid? byAssessment)
    {
        var query = dbContext.AssessmentMarks
            .Include(m => m.Choices)
            .Where(m => m.AssessedId == assessedUserId);

        if (byAssessment != null)
        {
            query = query.Where(m => m.AssessmentId == byAssessment);
        }

        var assessedUsers = await query.ToListAsync();

        return assessedUsers.Select(u => u.ToDomainModelWithoutDeps()).ToList();
    }
}