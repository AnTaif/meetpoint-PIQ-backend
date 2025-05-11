using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo.Assessments;

[Table("assessments")]
public class AssessmentDbo : EntityBase
{
    [Column("name")]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [Column("template_id")]
    public Guid TemplateId { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("use_circle_assessment")]
    public bool UseCircleAssessment { get; set; }

    [Column("use_behavior_assessment")]
    public bool UseBehaviorAssessment { get; set; }

    [Column("team_id")]
    public Guid TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public TeamDbo Team { get; set; } = null!;

    [ForeignKey(nameof(TemplateId))]
    public TemplateDbo Template { get; set; } = null!;
}