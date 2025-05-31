using System.Text.Json.Serialization;

namespace PIQService.Models.Domain.Assessments;

public class Choice
{
    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public string Text { get; private set; }

    [JsonInclude]
    public short Value { get; private set; }

    [JsonInclude]
    public string Description { get; private set; }

    [JsonInclude]
    public Guid QuestionId { get; private set; }

    [JsonConstructor]
    public Choice(Guid id, string text, short value, string description, Guid questionId)
    {
        Id = id;
        Text = text;
        Value = value;
        Description = description;
        QuestionId = questionId;
    }

    public Choice(Guid id)
    {
        Id = id;
        Text = "";
        Value = -1;
        Description = "";
    }
}