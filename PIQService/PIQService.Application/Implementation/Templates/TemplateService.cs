using Core.Results;
using PIQService.Application.Implementation.Events;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Templates;

public class TemplateService(
    IEventService eventService,
    IEventRepository eventRepository,
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

        var @event = await eventRepository.FindAsync(eventId.Value);
        if (@event == null)
            return StatusError.NotFound("Event not found");

        var template = await templateRepository.FindAsync(@event.TemplateId);
        if (template == null)
            return StatusError.NotFound("Template not found");

        return GetFormWithCriteriaDtosByTemplate(template);
    }

    private static List<FormWithCriteriaDto> GetFormWithCriteriaDtosByTemplate(Template template)
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
            Type = form.Type,
            CriteriaList = criteria,
        };
    }
}