using PIQService.Application.Implementation.Assessments.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.RequestExamples;

public class CreateTeamAssessmentRequestExample : IExamplesProvider<CreateTeamAssessmentRequest>
{
    public CreateTeamAssessmentRequest GetExamples() =>
        new()
        {
            Name = "Неделя 1",
            StartDate = new DateTime(2025, 3, 11),
            EndDate = new DateTime(2025, 3, 18),
            UseCircleAssessment = true,
            UseBehaviorAssessment = false,
        };
}