using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

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
            Description = choice.Description,
        };

    public static Choice ToDomainModel(this ChoiceDbo choiceDbo) =>
        new(choiceDbo.Id, choiceDbo.Text, choiceDbo.Value, choiceDbo.Description, choiceDbo.QuestionId);

    public static ChoiceShortDto ToShortDtoModel(this Choice choice) =>
        new()
        {
            Id = choice.Id,
            Text = choice.Text,
            Description = choice.Description,
        };
}