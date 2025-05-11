namespace PIQService.Models.Domain.Assessments;

public class AssessmentMarkWithoutDeps
{
    public Guid Id { get; private set; }

    public Guid AssessorId { get; private set; }

    public Guid AssessedId { get; private set; }

    public Guid AssessmentId { get; private set; }

    public List<Choice> Choices { get; private set; }

    public AssessmentMarkWithoutDeps(Guid id, Guid assessorId, Guid assessedId, Guid assessmentId, List<Choice> choices)
    {
        Id = id;
        AssessorId = assessorId;
        AssessedId = assessedId;
        AssessmentId = assessmentId;
        Choices = choices;
    }
}