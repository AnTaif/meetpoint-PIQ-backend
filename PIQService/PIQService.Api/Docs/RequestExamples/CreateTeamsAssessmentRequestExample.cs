using PIQService.Application.Implementation.Assessments.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.RequestExamples;

public class CreateTeamsAssessmentRequestExample : IExamplesProvider<CreateTeamsAssessmentRequest>
{
    public CreateTeamsAssessmentRequest GetExamples() =>
        new()
        {
            Name = "Неделя 1",
            StartDate = new DateTime(2025, 3, 11),
            EndDate = new DateTime(2025, 3, 18),
            UseCircleAssessment = true,
            UseBehaviorAssessment = false,
            TeamIds = [Guid.Parse("28f08351-d9e1-4602-b329-be6c72692299"), Guid.Parse("65b29076-ee2f-4afd-ab68-44be0f5d71c9")],
        };
}