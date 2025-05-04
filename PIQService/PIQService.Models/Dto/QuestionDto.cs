namespace PIQService.Models.Dto;

public class QuestionDto
{
    public Guid Id { get; init; }

    public string Text { get; init; } = null!;

    public float Weight { get; init; }

    public short Order { get; init; }

    public Guid CriteriaId { get; init; }

    public List<ChoiceDto> Choices { get; init; } = null!;
}