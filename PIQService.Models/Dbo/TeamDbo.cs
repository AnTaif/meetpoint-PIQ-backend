using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo;

[Table("teams")]
public class TeamDbo : EntityBase
{
    [Column("project_id")]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public ProjectDbo Project { get; set; } = null!;

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

    [ForeignKey(nameof(TutorId))]
    public UserDbo Tutor { get; set; } = null!;

    [Column("name")]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [Column("teams_users")]
    public ICollection<UserDbo> Users { get; set; } = null!;
}