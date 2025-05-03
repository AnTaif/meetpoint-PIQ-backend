using PIQService.Application.Implementation.Assessments.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.RequestExamples;

public class EditAssessmentRequestExample : IExamplesProvider<EditAssessmentRequest>
{
    public EditAssessmentRequest GetExamples() =>
        new()
        {
            Name = null,
            StartDate = null,
            EndDate = null,
            UseCircleAssessment = null,
            UseBehaviorAssessment = null,
        };
}