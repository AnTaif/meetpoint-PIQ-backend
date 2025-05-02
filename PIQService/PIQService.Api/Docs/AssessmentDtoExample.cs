using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs;

public class AssessmentDtoExample : IExamplesProvider<AssessmentDto>
{
    public AssessmentDto GetExamples() =>
        new()
        {
            Id = Guid.Parse("768c9125-630e-48db-9c12-ab5d9e51d476"),
            Name = "Неделя 1",
            StartDate = new DateTime(2025, 3, 11),
            EndDate = new DateTime(2025, 3, 18),
        };
}

public class EnumerableAssessmentDtoExample : IExamplesProvider<IEnumerable<AssessmentDto>>
{
    public IEnumerable<AssessmentDto> GetExamples() =>
    [
        new()
        {
            Id = Guid.Parse("ef6b2c59-5320-4438-9f5b-fb3c253a2aa7"),
            Name = "Неделя 2",
            StartDate = new DateTime(2025, 3, 19),
            EndDate = new DateTime(2025, 3, 26),
        },
        new()
        {
            Id = Guid.Parse("768c9125-630e-48db-9c12-ab5d9e51d476"),
            Name = "Неделя 1",
            StartDate = new DateTime(2025, 3, 11),
            EndDate = new DateTime(2025, 3, 18),
        },
    ];
}