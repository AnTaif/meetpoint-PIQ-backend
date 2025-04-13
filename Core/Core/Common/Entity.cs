namespace Core.Common;

public class Entity(Guid id)
{
    public Guid Id { get; init; } = id;
}