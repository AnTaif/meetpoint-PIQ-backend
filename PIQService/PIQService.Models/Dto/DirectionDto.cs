namespace PIQService.Models.Dto;

public class DirectionDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public IEnumerable<ProjectDto> Projects { get; init; } = null!;
}