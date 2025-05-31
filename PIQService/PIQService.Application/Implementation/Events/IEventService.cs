using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Events;

public interface IEventService
{
    Task<Event?> FindEventAsync(Guid? eventId);
    
    Task<EventBase?> FindEventWithoutDepsAsync(Guid? eventId);
}