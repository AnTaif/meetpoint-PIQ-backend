namespace PIQService.Models.Domain.Assessments;

public class Template
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Form CircleForm { get; private set; }

    public Form BehaviorForm { get; private set; }

    public Template(Guid id, string name, Form circleForm, Form behaviorForm)
    {
        Id = id;
        Name = name;
        CircleForm = circleForm;
        BehaviorForm = behaviorForm;
    }
}