namespace PIQService.Models.Dto;

public class AssessmentMarkDto
{
    public Guid Id { get; init; }

    public Guid AssessorId { get; init; }

    public Guid AssessedId { get; init; }

    public Guid AssessmentId { get; init; }

    public IEnumerable<Guid> Choices { get; init; } = null!;
}