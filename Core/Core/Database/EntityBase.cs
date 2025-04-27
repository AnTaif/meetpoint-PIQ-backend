using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database;

public abstract class EntityBase
{
    [Column("id", Order = 0)]
    public Guid Id { get; set; }
}