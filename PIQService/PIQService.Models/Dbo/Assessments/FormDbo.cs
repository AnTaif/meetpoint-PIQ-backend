using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Dbo.Assessments;

[Table("assessment_forms")]
public class FormDbo : EntityBase
{
    [Column("type", TypeName = "tinyint")]
    public AssessmentType Type { get; set; }

    public ICollection<CriteriaDbo> CriteriaList { get; set; } = null!;

    [ForeignKey(nameof(QuestionDbo.FormId))]
    public ICollection<QuestionDbo> Questions { get; set; } = null!;
}