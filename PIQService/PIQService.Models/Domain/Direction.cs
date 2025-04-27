namespace PIQService.Models.Domain;

public class Direction
{
    public Guid Id { get; private set; }

    public Event Event { get; private set; }

    public string Name { get; private set; }

    public Direction(Guid id, Event @event, string name)
    {
        Id = id;
        Event = @event;
        Name = name;
    }

    public Direction(Event @event, string name)
    {
        Id = Guid.NewGuid();
        Event = @event;
        Name = name;
    }
}