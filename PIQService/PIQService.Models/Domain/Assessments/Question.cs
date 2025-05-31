using System.Text.Json.Serialization;

namespace PIQService.Models.Domain.Assessments;

public class Question
{
    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public string Text { get; private set; }

    [JsonInclude]
    public float Weight { get; private set; }

    [JsonInclude]
    public short Order { get; private set; }

    [JsonInclude]
    public Criteria Criteria { get; private set; }

    [JsonInclude]
    public List<Choice> Choices { get; private set; }

    [JsonConstructor]
    public Question(Guid id, string text, float weight, short order, Criteria criteria, List<Choice> choices)
    {
        Id = id;
        Text = text;
        Weight = weight;
        Order = order;
        Criteria = criteria;
        Choices = choices;
    }
}