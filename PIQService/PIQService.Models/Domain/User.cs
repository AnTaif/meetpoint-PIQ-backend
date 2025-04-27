namespace PIQService.Models.Domain;

public class User
{
    public Guid Id { get; init; }

    public Team? Team { get; private set; }

    public User(Guid id, Team? team)
    {
        Id = id;
        Team = team;
    }
}