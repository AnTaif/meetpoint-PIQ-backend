namespace PIQService.Models.Domain.Assessments;

public class Template : TemplateBase
{
    public Form CircleForm { get; private set; }

    public Form BehaviorForm { get; private set; }

    public Template(Guid id, string name, Form circleForm, Form behaviorForm) : base(id, name, circleForm.Id, behaviorForm.Id)
    {
        CircleForm = circleForm;
        BehaviorForm = behaviorForm;
    }
}