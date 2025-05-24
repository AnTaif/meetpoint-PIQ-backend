namespace PIQService.Models.Domain;

public class TeamWithoutDeps
{
    public Guid Id { get; init; }

    public Guid ProjectId { get; private set; }

    public Guid TutorId { get; private set; }

    public string Name { get; private set; }
    
    public TeamWithoutDeps(Guid id, Guid projectId, Guid tutorId, string name)
    {
        Id = id;
        ProjectId = projectId;
        TutorId = tutorId;
        Name = name;
    }
}