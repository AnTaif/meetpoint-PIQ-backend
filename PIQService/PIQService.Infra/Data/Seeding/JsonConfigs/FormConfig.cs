using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Seeding.JsonConfigs;

public class FormConfig
{
    public AssessmentType Type { get; set; }
    
    public List<CriteriaConfig> Criteria { get; set; } = null!;
    
    public List<QuestionConfig> Questions { get; set; } = null!;
}