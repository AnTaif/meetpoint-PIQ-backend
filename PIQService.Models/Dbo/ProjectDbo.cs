using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo;

[Table("projects")]
public class ProjectDbo : EntityBase
{
    [Column("direction_id")]
    public Guid DirectionId { get; set; }

    [ForeignKey(nameof(DirectionId))]
    public DirectionDbo Direction { get; set; } = null!;

    [Column("name")]
    public string Name { get; set; } = null!;
}