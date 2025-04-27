namespace PIQService.Models.Domain;

public class Team
{
    public Guid Id { get; init; }

    public Project Project { get; private set; }

    public User Tutor { get; private set; }

    public string Name { get; private set; }

    public User[] Users { get; private set; }

    public Team(Guid id, Project project, User tutor, string name, User[] users)
    {
        Id = id;
        Project = project;
        Tutor = tutor;
        Name = name;
        Users = users;
    }
}