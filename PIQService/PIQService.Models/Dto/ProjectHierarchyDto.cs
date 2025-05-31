namespace PIQService.Models.Dto;

public class ProjectHierarchyDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public IEnumerable<TeamHierarchyDto> Teams { get; init; } = null!;
}