using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo.Assessments;

[Table("assessment_marks")]
public class AssessmentMarkDbo : EntityBase
{
    [Column("assessor_id")]
    public Guid AssessorId { get; set; }

    [Column("assessed_id")]
    public Guid AssessedId { get; set; }

    [Column("session_id")]
    public Guid AssessmentId { get; set; }

    [ForeignKey(nameof(AssessorId))]
    public UserDbo Assessor { get; set; } = null!;

    [ForeignKey(nameof(AssessedId))]
    public UserDbo Assessed { get; set; } = null!;

    [ForeignKey(nameof(AssessmentId))]
    public AssessmentDbo Assessment { get; set; } = null!;

    public ICollection<ChoiceDbo> Choices { get; set; } = null!;
}