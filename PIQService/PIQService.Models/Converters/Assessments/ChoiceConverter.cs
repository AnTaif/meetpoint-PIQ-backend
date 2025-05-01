using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Converters.Assessments;

public static class ChoiceConverter
{
    public static ChoiceDbo ToDboModel(this Choice choice, Guid questionId) =>
        new()
        {
            Id = choice.Id,
            QuestionId = questionId,
            Text = choice.Text,
            Value = choice.Value,
        };

    public static Choice ToDomainModel(this ChoiceDbo choiceDbo) =>
        new(choiceDbo.Id, choiceDbo.Text, choiceDbo.Value);
}