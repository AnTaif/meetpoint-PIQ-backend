using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public class AssessmentFormsService : IAssessmentFormsService
{
    private readonly ITemplateRepository templateRepository;
    private readonly IAssessmentRepository assessmentRepository;
    private readonly ILogger<AssessmentFormsService> logger;

    public AssessmentFormsService(
        ITemplateRepository templateRepository,
        IAssessmentRepository assessmentRepository,
        ILogger<AssessmentFormsService> logger
    )
    {
        this.templateRepository = templateRepository;
        this.assessmentRepository = assessmentRepository;
        this.logger = logger;
    }

    public async Task<Result<IEnumerable<FormShortDto>>> GetAssessmentUsedFormsAsync(Guid assessmentId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
        {
            return HttpError.NotFound("Assessment not found");
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
            var circleForm = template.CircleForm.ToShortDtoModel();
            usedForms.Add(circleForm);
        }

        if (assessment.UseBehaviorAssessment)
        {
            var behaviorForm = template.BehaviorForm.ToShortDtoModel();
            usedForms.Add(behaviorForm);
        }

        return usedForms;
    }
}