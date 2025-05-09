using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Dto;

public class FormShortDto
{
    public Guid Id { get; init; }

    public AssessmentType Type { get; init; }

    public List<CriteriaDto> CriteriaList { get; init; } = null!;

    public List<QuestionShortDto> Questions { get; init; } = null!;
}