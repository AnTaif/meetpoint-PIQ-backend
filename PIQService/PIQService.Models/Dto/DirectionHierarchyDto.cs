namespace PIQService.Models.Dto;

public class DirectionHierarchyDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public IEnumerable<ProjectHierarchyDto> Projects { get; init; } = null!;
}