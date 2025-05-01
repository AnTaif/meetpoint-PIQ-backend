using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

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
}