using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo;

[Table("users")]
public class UserDbo : EntityBase
{
    [Column("team_id")]
    public Guid? TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public TeamDbo? Team { get; set; } = null!;
}