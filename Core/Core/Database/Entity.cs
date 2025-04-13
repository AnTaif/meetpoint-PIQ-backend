namespace Core.Database;

public class Entity(Guid id)
{
    public Guid Id { get; init; } = id;
}