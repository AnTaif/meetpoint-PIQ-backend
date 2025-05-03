using System.ComponentModel.DataAnnotations;

namespace PIQService.Application.Implementation.Assessments.Requests;

public class EditAssessmentRequest
{
    [MaxLength(30)]
    public string? Name { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Только для предстоящего оценивания
    /// </summary>
    public bool? UseCircleAssessment { get; init; }

    /// <summary>
    /// Только для предстоящего оценивания
    /// </summary>
    public bool? UseBehaviorAssessment { get; init; }
}