using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Application.Implementation.Assessments.Marks;

public interface IAssessmentMarkRepository
{
    Task<AssessmentMarkWithoutDeps?> FindWithoutDepsAsync(Guid assessmentId, Guid assessorId, Guid assessedId);

    void Create(AssessmentMarkWithoutDeps mark, IEnumerable<Guid> choiceIds);

    void UpdateChoices(AssessmentMarkWithoutDeps mark, IEnumerable<Guid> choiceIds);

    Task<IEnumerable<User>> SelectAssessedUsersAsync(Guid assessorId, Guid assessmentId);

    Task<IReadOnlyCollection<AssessmentMarkWithoutDeps>> SelectByAssessedUserIdAsync(Guid assessedUserId, Guid? byAssessment);
}