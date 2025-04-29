using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo;

[Table("users")]
public class UserDbo : EntityBase
{
    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("middle_name")]
    public string? MiddleName { get; set; } = null;

    [Column("team_id")]
    public Guid? TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public TeamDbo? Team { get; set; }
}