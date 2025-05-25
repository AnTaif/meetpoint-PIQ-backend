using System.ComponentModel.DataAnnotations;
using PIQService.Application.Attributes;

namespace PIQService.Application.Implementation.Assessments.Requests;

public class CreateTeamAssessmentRequest
{
    [MaxLength(30)]
    public string Name { get; init; } = null!;

    public DateTime StartDate { get; init; }

    [NotPastDateTime]
    public DateTime EndDate { get; init; }

    public bool UseCircleAssessment { get; init; }

    public bool UseBehaviorAssessment { get; init; }
}