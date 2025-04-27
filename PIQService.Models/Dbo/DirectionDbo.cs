using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo;

[Table("directions")]
public class DirectionDbo : EntityBase
{
    [Column("event_id")]
    public Guid EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public EventDbo Event { get; set; } = null!;

    [Column("name")]
    public string Name { get; set; } = null!;
}