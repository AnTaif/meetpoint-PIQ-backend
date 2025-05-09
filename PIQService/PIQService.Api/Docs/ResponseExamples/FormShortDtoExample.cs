using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.ResponseExamples;

public class FormShortDtoExample : IExamplesProvider<FormShortDto>
{
    public FormShortDto GetExamples()
    {
        return new FormShortDto
        {
            Id = Guid.Parse("f5b899ba-e606-4379-830e-b459bf532f1d"),
            Type = AssessmentType.Circle,
            CriteriaList =
            [
                new CriteriaDto { Id = Guid.Parse("0af69f70-b443-4bec-a3a6-ea168601ab29"), Name = "Обучаемость", Description = "Описание обучаемости", },
                new CriteriaDto { Id = Guid.Parse("b4f7d5a4-cd64-4aec-ab52-d97a76b619f2"), Name = "Вовлеченность", Description = "Описание вовлеченности", },
            ],
            Questions =
            [
                new QuestionShortDto()
                {
                    Id = Guid.Parse("0c1aba94-b9c0-4c65-9e1a-fc6d8e743c8d"),
                    Text = "Проявляет инициативу в обсуждениях?",
                    CriteriaId = Guid.Parse("b4f7d5a4-cd64-4aec-ab52-d97a76b619f2"),
                    Choices =
                    [
                        new ChoiceShortDto { Id = Guid.Parse("0989e7bd-1f52-4822-8e4e-48e2c8204d26"), Text = "-1", Description = "Описание выбора" },
                        new ChoiceShortDto { Id = Guid.Parse("849c8814-92cf-4921-be34-cb4278069d47"), Text = "0", Description = "Описание выбора" },
                        new ChoiceShortDto { Id = Guid.Parse("8712de36-0421-4e37-b576-eaedb0e210bd"), Text = "1", Description = "Описание выбора" },
                        new ChoiceShortDto { Id = Guid.Parse("7152ad87-861e-4884-85b9-906ce0211a05"), Text = "2", Description = "Описание выбора" },
                        new ChoiceShortDto { Id = Guid.Parse("5a013f9d-4dc7-4a8e-bac0-dcbd3dadd7dd"), Text = "3", Description = "Описание выбора" },
                    ],
                },
            ],
        };
    }
}

public class EnumerableFormShortDtoExample : IExamplesProvider<IEnumerable<FormShortDto>>
{
    public IEnumerable<FormShortDto> GetExamples()
    {
        return [new FormShortDtoExample().GetExamples()];
    }
}