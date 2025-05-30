using System.Text.Json.Serialization;

namespace PIQService.Models.Domain;

public class Team : TeamWithoutDeps
{

    public Project Project { get; private set; }

    public User Tutor { get; private set; }

    public User[] Users { get; private set; }

    [JsonConstructor]
    public Team(Guid id, Project project, User tutor, string name, User[] users): base(id, project.Id, tutor.Id, name)
    {
        Project = project;
        Tutor = tutor;
        Users = users;
    }
}