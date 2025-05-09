using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo.Assessments;

[Table("assessment_questions")]
public class QuestionDbo : EntityBase
{
    [Column("question_text")]
    [MaxLength(255)]
    public string QuestionText { get; set; } = null!;

    [Column("weight")]
    public float Weight { get; set; }

    [Column("form_id")]
    public Guid FormId { get; set; }

    [Column("criteria_id")]
    public Guid CriteriaId { get; set; }

    [Column("order", TypeName = "tinyint")]
    public short Order { get; set; }

    [ForeignKey(nameof(CriteriaId))]
    public CriteriaDbo Criteria { get; set; } = null!;

    [ForeignKey(nameof(ChoiceDbo.QuestionId))]
    public ICollection<ChoiceDbo> Choices { get; set; } = null!;
}