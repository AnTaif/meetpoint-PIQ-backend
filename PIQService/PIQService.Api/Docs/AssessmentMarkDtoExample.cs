using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs;

public class AssessmentMarkDtoExample : IExamplesProvider<AssessmentMarkDto>
{
    public AssessmentMarkDto GetExamples() =>
        new()
        {
            Id = Guid.Parse("9e768cf8-eda6-4de0-b84b-b141d211871e"),
            AssessorId = Guid.Parse("f25f0097-eec9-4352-9121-9a4197670b78"),
            AssessedId = Guid.Parse("cb675333-380c-448b-b239-8e87f9b98e23"),
            AssessmentId = Guid.Parse("2bf70c34-7109-4439-a287-062958c3831b"),
            Choices =
            [
                Guid.Parse("cf719228-c257-45ea-a1c1-1c24180f441d"),
                Guid.Parse("86924253-9be5-422e-b0e4-d3eb954a4b7c"),
            ],
        };
}