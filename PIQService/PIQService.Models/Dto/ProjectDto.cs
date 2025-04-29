namespace PIQService.Models.Dto;

public class ProjectDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public IEnumerable<TeamDto> Teams { get; init; } = null!;
}