namespace PIQService.Models.Domain.Assessments;

public class Choice
{
    public Guid Id { get; private set; }

    public string Text { get; private set; }

    public short Value { get; private set; }

    public string Description { get; private set; }

    public Choice(Guid id, string text, short value, string description)
    {
        Id = id;
        Text = text;
        Value = value;
        Description = description;
    }

    public Choice(Guid id)
    {
        Id = id;
        Text = "";
        Value = -1;
        Description = "";
    }
}