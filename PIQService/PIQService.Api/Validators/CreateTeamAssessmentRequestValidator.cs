using FluentValidation;
using PIQService.Application.Implementation.Assessments.Requests;

namespace PIQService.Api.Validators;

public class CreateTeamAssessmentRequestValidator : AbstractValidator<CreateTeamAssessmentRequest>
{
    public CreateTeamAssessmentRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("StartDate must be earlier than EndDate");

        RuleFor(x => x)
            .Must(x => x.UseCircleAssessment || x.UseBehaviorAssessment)
            .WithMessage("At least one form must be selected");
    }
}