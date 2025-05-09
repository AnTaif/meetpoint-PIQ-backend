using PIQService.Models.Domain.Assessments;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentRepository
{
    Task<AssessmentWithoutDeps?> FindWithoutDepsAsync(Guid id);

    Task<IEnumerable<AssessmentWithoutDeps>> SelectByTeamIdAsync(Guid teamId);

    void Create(Assessment assessment);

    void Update(AssessmentWithoutDeps assessmentWithoutDeps);

    void Delete(AssessmentWithoutDeps assessmentWithoutDeps);

    Task SaveChangesAsync();
}