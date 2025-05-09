using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters.Assessments;

public static class QuestionConverter
{
    public static QuestionDbo ToDboModel(this Question question, Guid formId, Guid criteriaId) =>
        new()
        {
            Id = question.Id,
            QuestionText = question.Text,
            Weight = question.Weight,
            FormId = formId,
            CriteriaId = criteriaId,
            Order = question.Order,
        };

    public static Question ToDomainModel(this QuestionDbo questionDbo) =>
        new(
            questionDbo.Id,
            questionDbo.QuestionText,
            questionDbo.Weight,
            questionDbo.Order,
            questionDbo.Criteria.ToDomainModel(),
            questionDbo.Choices.Select(c => c.ToDomainModel()).ToList()
        );

    public static QuestionShortDto ToShortDtoModel(this Question question) =>
        new()
        {
            Id = question.Id,
            Text = question.Text,
            CriteriaId = question.Criteria.Id,
            Choices = question.Choices.Select(c => c.ToShortDtoModel()).ToList(),
        };
}