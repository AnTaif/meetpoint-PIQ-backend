namespace PIQService.Models.Domain;

public class Project
{
    public Guid Id { get; private set; }

    public Direction Direction { get; private set; }

    public string Name { get; private set; }

    public Project(Guid id, Direction direction, string name)
    {
        Id = id;
        Direction = direction;
        Name = name;
    }
}