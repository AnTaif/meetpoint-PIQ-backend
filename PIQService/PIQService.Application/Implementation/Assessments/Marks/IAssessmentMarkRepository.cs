using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Assessments.Marks;

public interface IAssessmentMarkRepository
{
    Task<IEnumerable<User>> SelectAssessedUsersAsync(Guid assessorId, Guid assessmentId);
}