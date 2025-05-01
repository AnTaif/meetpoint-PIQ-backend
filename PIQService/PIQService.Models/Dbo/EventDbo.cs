using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;
using PIQService.Models.Dbo.Assessments;

namespace PIQService.Models.Dbo;

[Table("events")]
public class EventDbo : EntityBase
{
    [Column("name")]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("assessment_template_id")]
    public Guid TemplateId { get; set; }

    [ForeignKey(nameof(TemplateId))]
    public TemplateDbo Template { get; set; } = null!;
}