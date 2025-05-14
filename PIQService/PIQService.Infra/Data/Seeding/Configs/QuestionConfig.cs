namespace PIQService.Infra.Data.Seeding.Configs;

public class QuestionConfig
{
    public string Text { get; set; } = null!;
    
    public float Weight { get; set; }
    
    public short Order { get; set; }
    
    public string CriteriaName { get; set; } = null!;
    
    public List<ChoiceConfig> Choices { get; set; } = null!;
}