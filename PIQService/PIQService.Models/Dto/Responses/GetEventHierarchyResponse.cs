namespace PIQService.Models.Dto.Responses;

public class GetEventHierarchyResponse
{
    public EventDto Event { get; init; } = null!;

    public IEnumerable<Guid> TeamIdsForEvaluation { get; init; } = null!;
}