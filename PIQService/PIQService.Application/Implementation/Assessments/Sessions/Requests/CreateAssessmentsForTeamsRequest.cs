using System.ComponentModel.DataAnnotations;

namespace PIQService.Application.Implementation.Assessments.Sessions.Requests;

public class CreateAssessmentsForTeamsRequest : CreateAssessmentRequestBase
{
    [MinLength(1)]
    public IReadOnlyCollection<Guid> TeamIds { get; init; } = null!;
}