using System.ComponentModel.DataAnnotations;

namespace PIQService.Application.Implementation.Assessments.Requests;

public class CreateTeamsAssessmentRequest : CreateAssessmentRequestBase
{
    [MinLength(1)]
    public IReadOnlyCollection<Guid> TeamIds { get; init; } = null!;
}