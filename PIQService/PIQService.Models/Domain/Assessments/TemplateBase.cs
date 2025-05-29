namespace PIQService.Models.Domain.Assessments;

public class TemplateBase
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Guid CircleFormId { get; private set; }

    public Guid BehaviorFormId { get; private set; }

    public TemplateBase(Guid id, string name, Guid circleFormId, Guid behaviorFormId)
    {
        Id = id;
        Name = name;
        CircleFormId = circleFormId;
        BehaviorFormId = behaviorFormId;
    }
}