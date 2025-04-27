namespace PIQService.Models.Dto;

public class DirectionDto
{
    public Guid Id { get; init; }

    public EventDto Event { get; init; } = null!;

    public string Name { get; init; } = null!;
}