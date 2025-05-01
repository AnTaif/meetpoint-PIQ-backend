namespace PIQService.Models.Domain.Assessments;

public class Criteria
{
    public Guid Id { get; private set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<Question> Questions { get; private set; }

    public Criteria(Guid id, string name, string description, List<Question> questions)
    {
        Id = id;
        Name = name;
        Description = description;
        Questions = questions;
    }
}