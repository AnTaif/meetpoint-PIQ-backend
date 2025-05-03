using FluentValidation;
using PIQService.Application.Implementation.Assessments.Requests;

namespace PIQService.Api.Validators;

public class EditAssessmentRequestValidator : AbstractValidator<EditAssessmentRequest>
{
    public EditAssessmentRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => x.StartDate == null || x.EndDate == null || x.StartDate < x.EndDate)
            .WithMessage("StartDate must be earlier than EndDate");
    }
}