namespace PIQService.Models.Dto;

public class EventHierarchyDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public IEnumerable<DirectionHierarchyDto> Directions { get; init; } = null!;
}