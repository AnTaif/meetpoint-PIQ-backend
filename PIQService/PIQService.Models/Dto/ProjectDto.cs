namespace PIQService.Models.Dto;

public class ProjectDto
{
    public Guid Id { get; init; }

    public DirectionDto Direction { get; init; } = null!;

    public string Name { get; init; } = null!;
}