using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Forms;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

[RegisterScoped]
public class AssessmentFormsService(
    IFormRepository formRepository,
    ITemplateRepository templateRepository,
    IAssessmentRepository assessmentRepository,
    ILogger<AssessmentFormsService> logger
)
    : IAssessmentFormsService
{
    public async Task<Result<IEnumerable<FormShortDto>>> GetAssessmentUsedFormsAsync(Guid assessmentId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
        {
            return StatusError.NotFound("Assessment not found");
        }

        var template = await templateRepository.FindAsync(assessment.TemplateId);

        if (template == null)
        {
            logger.LogError("Assessment with id={assessmentId} does not have template!", assessment.Id);
            return Array.Empty<FormShortDto>();
        }

        var usedForms = new List<FormShortDto>();

        if (assessment.UseCircleAssessment)
        {
            var circleForm = await formRepository.FindAsync(template.CircleFormId);
            if (circleForm == null)
            {
                logger.LogError("Не нашли форму с id={formId} для шаблона id={templateId}", template.CircleFormId, template.Id);
            }
            else
            {
                usedForms.Add(circleForm.ToShortDtoModel());
            }
        }

        if (assessment.UseBehaviorAssessment)
        {
            var behaviorForm = await formRepository.FindAsync(template.BehaviorFormId);
            if (behaviorForm == null)
            {
                logger.LogError("Не нашли форму с id={formId} для шаблона id={templateId}", template.CircleFormId, template.Id);
            }
            else
            {
                usedForms.Add(behaviorForm.ToShortDtoModel());
            }
        }

        return usedForms;
    }
}