namespace PIQService.Models.Dto;

public class EventDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }
}