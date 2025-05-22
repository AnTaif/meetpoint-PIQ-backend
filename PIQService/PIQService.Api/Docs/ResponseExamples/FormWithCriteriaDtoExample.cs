using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.ResponseExamples;

public class FormWithCriteriaDtoExample : IExamplesProvider<FormWithCriteriaDto>
{
    public FormWithCriteriaDto GetExamples()
    {
        return new FormWithCriteriaDto
        {
            Id = Guid.Parse("f5b899ba-e606-4379-830e-b459bf532f1d"),
            Type = AssessmentType.Circle,
            CriteriaList =
            [
                new CriteriaDto
                {
                    Id = Guid.Parse("0af69f70-b443-4bec-a3a6-ea168601ab29"), Name = "Обучаемость", Description = "Описание обучаемости",
                },
                new CriteriaDto
                {
                    Id = Guid.Parse("b4f7d5a4-cd64-4aec-ab52-d97a76b619f2"), Name = "Вовлеченность", Description = "Описание вовлеченности",
                },
            ],
        };
    }
}

public class EnumerableFormWithCriteriaDtoExample : IExamplesProvider<IEnumerable<FormWithCriteriaDto>>
{
    public IEnumerable<FormWithCriteriaDto> GetExamples()
    {
        return new List<FormWithCriteriaDto>
        {
            new()
            {
                Id = Guid.Parse("f5b899ba-e606-4379-830e-b459bf532f1d"),
                Type = AssessmentType.Circle,
                CriteriaList =
                [
                    new CriteriaDto
                    {
                        Id = Guid.Parse("0af69f70-b443-4bec-a3a6-ea168601ab29"), Name = "Обучаемость", Description = "Описание обучаемости",
                    },
                    new CriteriaDto
                    {
                        Id = Guid.Parse("b4f7d5a4-cd64-4aec-ab52-d97a76b619f2"), Name = "Вовлеченность",
                        Description = "Описание вовлеченности",
                    },
                ],
            }
        };
    }
}