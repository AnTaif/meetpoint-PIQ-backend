using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo.Assessments;

[Table("assessment_templates")]
public class TemplateDbo : EntityBase
{
    [Column("name")]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [Column("circle_form_id")]
    public Guid CircleFormId { get; set; }

    [Column("behavior_form_id")]
    public Guid BehaviorFormId { get; set; }

    [ForeignKey(nameof(CircleFormId))]
    public FormDbo CircleForm { get; set; } = null!;

    [ForeignKey(nameof(BehaviorFormId))]
    public FormDbo BehaviorForm { get; set; } = null!;
}