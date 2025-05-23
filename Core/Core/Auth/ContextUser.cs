namespace Core.Auth;

public class ContextUser
{
    public Guid Id { get; init; }

    public IReadOnlyCollection<string> Roles { get; init; } = null!;
}