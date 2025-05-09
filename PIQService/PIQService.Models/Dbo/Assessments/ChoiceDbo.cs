using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo.Assessments;

[Table("assessment_choices")]
public class ChoiceDbo : EntityBase
{
    [Column("question_id")]
    public Guid QuestionId { get; set; }

    [Column("text")]
    [MaxLength(255)]
    public string Text { get; set; } = null!;

    [Column("value")]
    public short Value { get; set; }

    [Column("description")]
    [MaxLength(255)]
    public string Description { get; set; } = null!;
}