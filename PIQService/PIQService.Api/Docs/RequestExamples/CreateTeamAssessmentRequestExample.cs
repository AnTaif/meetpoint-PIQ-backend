using PIQService.Application.Implementation.Assessments.Sessions.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.RequestExamples;

public class CreateTeamAssessmentRequestExample : IExamplesProvider<CreateAssessmentForTeamRequest>
{
    public CreateAssessmentForTeamRequest GetExamples() =>
        new()
        {
            Name = "Неделя 1",
            StartDate = new DateTime(2025, 3, 11),
            EndDate = new DateTime(2025, 3, 18),
            UseCircleAssessment = true,
            UseBehaviorAssessment = false,
        };
}