using System.Text.Json.Serialization;

namespace PIQService.Models.Domain;

public class Direction
{
    public Guid Id { get; private set; }

    public EventBase EventBase { get; private set; }

    public string Name { get; private set; }

    [JsonConstructor]
    public Direction(Guid id, EventBase eventBase, string name)
    {
        Id = id;
        EventBase = eventBase;
        Name = name;
    }

    public Direction(EventBase eventBase, string name)
    {
        Id = Guid.NewGuid();
        EventBase = eventBase;
        Name = name;
    }
}