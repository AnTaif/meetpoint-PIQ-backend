using System.ComponentModel.DataAnnotations.Schema;
using Core.Database;

namespace PIQService.Models.Dbo;

[Table("events")]
public class EventDbo : EntityBase
{
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }
}