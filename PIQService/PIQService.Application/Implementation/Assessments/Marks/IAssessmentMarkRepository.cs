using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Application.Implementation.Assessments.Marks;

public interface IAssessmentMarkRepository
{
    Task<AssessmentMarkWithoutDeps?> FindWithoutDepsAsync(Guid assessmentId, Guid assessorId, Guid assessedId);

    Task<IEnumerable<User>> SelectAssessedUsersAsync(Guid assessorId, Guid assessmentId);
}