using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs;

public class AssessChoiceDtoExample : IExamplesProvider<AssessChoiceDto>
{
    public AssessChoiceDto GetExamples() =>
        new()
        {
            QuestionId = Guid.Parse("94c12604-01b6-4bd3-b363-f4aabf318f79"),
            ChoiceId = Guid.Parse("7cff7585-b409-4c3c-a4f1-b2583979e037"),
        };
}

public class EnumerableAssessChoiceDtoExample : IExamplesProvider<IEnumerable<AssessChoiceDto>>
{
    public IEnumerable<AssessChoiceDto> GetExamples() =>
    [
        new()
        {
            QuestionId = Guid.Parse("94c12604-01b6-4bd3-b363-f4aabf318f79"),
            ChoiceId = Guid.Parse("7cff7585-b409-4c3c-a4f1-b2583979e037"),
        },
        new()
        {
            QuestionId = Guid.Parse("0a9478ee-8040-497c-803d-fb9b9bda20e5"),
            ChoiceId = Guid.Parse("eb2ce6a7-ab68-46c7-9dc1-991cb60b129e"),
        },
    ];
}