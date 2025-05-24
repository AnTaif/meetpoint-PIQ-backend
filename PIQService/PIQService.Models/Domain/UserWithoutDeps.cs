namespace PIQService.Models.Domain;

public class UserWithoutDeps
{
    public Guid Id { get; init; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string? MiddleName { get; private set; }

    public Guid? TeamId { get; private set; }
    
    public UserWithoutDeps(Guid id, string firstName, string lastName, string? middleName, Guid? teamId)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        TeamId = teamId;
    }
    
    public string GetFullName()
    {
        var fullname = $"{LastName} {FirstName}";

        if (MiddleName != null)
        {
            fullname += $" {MiddleName}";
        }

        return fullname;
    }
}