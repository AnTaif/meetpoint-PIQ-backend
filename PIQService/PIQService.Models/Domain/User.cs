namespace PIQService.Models.Domain;

public class User : UserWithoutDeps
{
    public Team? Team { get; private set; }

    public User(Guid id, string firstName, string lastName, string? middleName, Team? team)
        : base(id, firstName, lastName, middleName, team?.Id)
    {
        Team = team;
    }
}