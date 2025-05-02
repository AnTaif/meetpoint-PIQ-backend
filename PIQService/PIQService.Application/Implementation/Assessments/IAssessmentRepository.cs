using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentRepository
{
    Task<IEnumerable<AssessmentWithoutDeps>> SelectByTeamIdAsync(Guid teamId);

    Task CreateAsync(Assessment assessment, Team team);

    Task SaveChangesAsync();
}