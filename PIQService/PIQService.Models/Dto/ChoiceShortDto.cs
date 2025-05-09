namespace PIQService.Models.Dto;

public class ChoiceShortDto
{
    public Guid Id { get; init; }

    public string Text { get; init; } = null!;

    public string Description { get; init; } = null!;
}