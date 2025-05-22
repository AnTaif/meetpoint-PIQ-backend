using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Dto;

public class FormWithCriteriaDto
{
    public Guid Id { get; init; }

    public AssessmentType Type { get; init; }

    public List<CriteriaDto> CriteriaList { get; init; } = null!;
}