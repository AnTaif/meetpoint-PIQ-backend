using System.ComponentModel.DataAnnotations;

namespace PIQService.Application.Implementation.Assessments.Requests;

public class CreateTeamAssessmentRequest
{
    [MaxLength(30)]
    public string Name { get; init; } = null!;

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public bool UseCircleAssessment { get; init; }

    public bool UseBehaviorAssessment { get; init; }
}