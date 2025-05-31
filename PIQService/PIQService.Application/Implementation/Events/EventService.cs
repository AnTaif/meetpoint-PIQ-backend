using Microsoft.Extensions.Logging;
using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Events;

[RegisterScoped]
public class EventService(
    IEventRepository eventRepository,
    ILogger<EventService> logger
)
    : IEventService
{
    public async Task<Event?> FindEventAsync(Guid? eventId)
    {
        return eventId == null ? await FindCurrentEventAsync() : await eventRepository.FindAsync(eventId.Value);
    }

    public async Task<EventBase?> FindEventWithoutDepsAsync(Guid? eventId)
    {
        return eventId == null ? await FindCurrentEventBaseAsync() : await eventRepository.FindBaseAsync(eventId.Value);
    }

    private async Task<Event?> FindCurrentEventAsync()
    {
        var activeEvents = await eventRepository.SelectActiveAsync(DateTime.UtcNow);
        return activeEvents.FirstOrDefault();
    }

    private async Task<EventBase?> FindCurrentEventBaseAsync()
    {
        var activeEvents = await eventRepository.SelectActiveBaseAsync(DateTime.UtcNow);
        return activeEvents.FirstOrDefault();
    }
}