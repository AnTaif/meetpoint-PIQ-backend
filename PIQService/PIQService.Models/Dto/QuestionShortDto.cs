namespace PIQService.Models.Dto;

public class QuestionShortDto
{
    public Guid Id { get; init; }

    public string Text { get; init; } = null!;

    public short Order { get; init; }

    public Guid CriteriaId { get; init; }

    public List<ChoiceShortDto> Choices { get; init; } = null!;
}