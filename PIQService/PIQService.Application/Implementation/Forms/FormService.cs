using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Events;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Forms;

[RegisterScoped]
public class FormService(
    IFormRepository formRepository,
    IEventService eventService,
    ILogger<FormService> logger
)
    : IFormService
{
    public async Task<Result<List<FormWithCriteriaDto>>> GetFormsWithCriteriaAsync(Guid? eventId)
    {
        var @event = await eventService.FindEventAsync(eventId);

        if (@event == null)
        {
            return StatusError.NotFound("Event not found");
        }

        return await GetFormsByEventAsync(@event);
    }
    
    private async Task<List<FormWithCriteriaDto>> GetFormsByEventAsync(Event @event)
    {
        var dtos = new List<FormWithCriteriaDto>();
        await foreach (var form in GetFormsAsync(@event))
        {
            if (form == null)
            {
                logger.LogError("Какая-то форма шаблона с id={templateId} не нашлась в бд", @event.Template.Id);
                continue;
            }

            dtos.Add(form.ToFormWithCriteriaDto());
        }

        return dtos;
    }

    private async IAsyncEnumerable<Form?> GetFormsAsync(Event @event)
    {
        var template = @event.Template;
        
        var formIds = new List<Guid> { template.CircleFormId, template.BehaviorFormId };

        foreach (var formId in formIds)
        {
            var form = await formRepository.FindAsync(formId);
            if (form == null)
                yield return null;

            yield return form;
        }
    }
}