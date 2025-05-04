using Core.Results;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public class AssessmentFormsService : IAssessmentFormsService
{
    private readonly ITemplateRepository templateRepository;
    private readonly IAssessmentRepository assessmentRepository;

    public AssessmentFormsService(
        ITemplateRepository templateRepository,
        IAssessmentRepository assessmentRepository
    )
    {
        this.templateRepository = templateRepository;
        this.assessmentRepository = assessmentRepository;
    }

    public async Task<Result<IEnumerable<FormShortDto>>> GetAssessmentFormsAsync(Guid assessmentId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
        {
            return HttpError.NotFound("Assessment not found");
        }

        var template = await templateRepository.FindAsync(assessment.TemplateId);

        if (template == null)
        {
            Console.WriteLine($"ERROR: Assessment with id={assessment.Id} does not have template!");
            return Array.Empty<FormShortDto>();
        }

        var usedForms = new List<FormShortDto>();

        if (assessment.UseCircleAssessment)
        {
            var circleForm = template.CircleForm.ToShortDtoModel();
            usedForms.Add(circleForm);
        }

        return usedForms;
    }
}