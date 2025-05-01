using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo.Assessments;

[Table("assessment_criteria")]
public class CriteriaDbo : EntityBase
{
    [Column("name")]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [MaxLength(255)]
    public string Description { get; set; } = null!;
}