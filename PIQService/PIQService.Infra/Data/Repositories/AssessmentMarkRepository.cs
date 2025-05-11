using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Assessments.Marks;
using PIQService.Models.Converters;
using PIQService.Models.Domain;

namespace PIQService.Infra.Data.Repositories;

public class AssessmentMarkRepository(AppDbContext dbContext) : IAssessmentMarkRepository
{
    public async Task<IEnumerable<User>> SelectAssessedUsersAsync(Guid assessorId, Guid assessmentId)
    {
        var assessedUsers = await dbContext.AssessmentMarks
            .Where(m => m.SessionId == assessmentId)
            .Where(m => m.AssessorId == assessorId)
            .Include(m => m.Assessed)
            .Select(m => m.Assessed)
            .ToListAsync();

        return assessedUsers.Select(u => u.ToDomainModel()).ToList();
    }
}