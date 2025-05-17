namespace PIQService.Infra.Data.Seeding.JsonConfigs;

public class TemplateConfig
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public FormConfig CircleForm { get; set; } = null!;
    
    public FormConfig BehaviorForm { get; set; } = null!;
}