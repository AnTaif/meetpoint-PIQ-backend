namespace PIQService.Models.Domain;

public class User
{
    public Guid Id { get; init; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string? MiddleName { get; private set; }

    public Team? Team { get; private set; }

    public User(Guid id, string firstName, string lastName, string? middleName, Team? team)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Team = team;
    }
}