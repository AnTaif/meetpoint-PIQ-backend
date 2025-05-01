namespace PIQService.Models.Domain.Assessments;

public class Question
{
    public Guid Id { get; private set; }

    public string Text { get; private set; }

    public float Weight { get; private set; }

    public Criteria Criteria { get; private set; }

    public List<Choice> Choices { get; private set; }

    public Question(Guid id, string text, float weight, Criteria criteria, List<Choice> choices)
    {
        Id = id;
        Text = text;
        Weight = weight;
        Criteria = criteria;
        Choices = choices;
    }
}