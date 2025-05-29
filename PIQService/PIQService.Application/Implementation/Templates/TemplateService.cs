using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Forms;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Templates;

[RegisterScoped]
public class TemplateService(
    IFormRepository formRepository,
    IEventService eventService,
    IEventRepository eventRepository,
    ITemplateRepository templateRepository,
    ILogger<TemplateService> logger
)
    : ITemplateService
{
    public async Task<Result<List<FormWithCriteriaDto>>> GetFormsWithCriteriaAsync(Guid? eventId)
    {
        var resolvedEventId = await ResolveEventIdAsync(eventId);
        if (resolvedEventId.IsFailure)
            return resolvedEventId.Error;

        var templateResult = await GetTemplateByEventIdAsync(resolvedEventId.Value);
        if (templateResult.IsFailure)
            return templateResult.Error;

        return await GetFormsByTemplateAsync(templateResult.Value);
    }

    private async Task<Result<Guid>> ResolveEventIdAsync(Guid? eventId)
    {
        if (eventId.HasValue)
            return eventId.Value;

        var currentEventResult = await eventService.FindCurrentEventAsync();
        if (currentEventResult.IsFailure)
            return currentEventResult.Error;

        return currentEventResult.Value?.Id ??
               throw new InvalidOperationException("Current event not found");
    }

    private async Task<Result<TemplateBase>> GetTemplateByEventIdAsync(Guid eventId)
    {
        var @event = await eventRepository.FindAsync(eventId);
        if (@event == null)
            return StatusError.NotFound("Event not found");

        var template = await templateRepository.FindAsync(@event.TemplateId);
        if (template == null)
            return StatusError.NotFound("Template not found");

        return template;
    }

    private async Task<Result<List<FormWithCriteriaDto>>> GetFormsByTemplateAsync(TemplateBase template)
    {
        var forms = GetFormsAsync(template);

        var dtos = new List<FormWithCriteriaDto>();
        await foreach (var form in forms)
        {
            if (form == null)
            {
                logger.LogError("Какая-то форма шаблона с id={templateId} не нашлась в бд", template.Id);
                continue;
            }

            dtos.Add(MapToFormWithCriteriaDto(form));
        }

        return dtos;
    }

    private async IAsyncEnumerable<Form?> GetFormsAsync(TemplateBase template)
    {
        var formIds = new List<Guid> { template.CircleFormId, template.BehaviorFormId };

        foreach (var formId in formIds)
        {
            var form = await formRepository.FindAsync(formId);
            if (form == null)
                yield return null;

            yield return form;
        }
    }

    private static FormWithCriteriaDto MapToFormWithCriteriaDto(Form form)
    {
        return new FormWithCriteriaDto
        {
            Id = form.Id,
            Type = form.Type,
            CriteriaList = form.CriteriaList.Select(c => c.ToDtoModel()).ToList()
        };
    }
}