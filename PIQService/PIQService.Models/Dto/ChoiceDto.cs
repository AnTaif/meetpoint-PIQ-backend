namespace PIQService.Models.Dto;

public class ChoiceDto
{
    public Guid Id { get; init; }

    public string Text { get; init; } = null!;

    public short Value { get; init; }
}