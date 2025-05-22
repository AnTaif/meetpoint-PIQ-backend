using Core.Results;
using PIQService.Application.Implementation.Events;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Templates;

public class TemplateService(
    IEventService eventService,
    ITemplateRepository templateRepository
)
    : ITemplateService
{
    public async Task<Result<IReadOnlyCollection<FormWithCriteriaDto>>> GetFormsWithCriteriaAsync(Guid? eventId)
    {
        if (eventId == null)
        {
            var eventResult = await eventService.FindCurrentEventAsync();

            if (eventResult.IsFailure)
                return eventResult.Error;

            eventId = eventResult.Value!.Id;
        }

        var template = await templateRepository.FindAsync(eventId.Value);
        if (template == null)
            return StatusError.NotFound("Template not found");

        return GetFormWithCriteriaDtos(template);
    }

    private static List<FormWithCriteriaDto> GetFormWithCriteriaDtos(Template template)
    {
        return
        [
            GetFormWithCriteriaDto(template.CircleForm),
            GetFormWithCriteriaDto(template.BehaviorForm),
        ];
    }

    private static FormWithCriteriaDto GetFormWithCriteriaDto(Form form)
    {
        var criteria = form.CriteriaList.Select(c => c.ToDtoModel()).ToList();
        return new FormWithCriteriaDto
        {
            Id = form.Id,
            Type = AssessmentType.Circle,
            CriteriaList = criteria,
        };
    }
}