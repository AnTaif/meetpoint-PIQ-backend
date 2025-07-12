using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.EventSupporting;

public interface IEventRepository
{
    Task<Event?> FindAsync(Guid id);
    
    Task<EventBase?> FindBaseAsync(Guid id);

    Task<Guid?> FindTemplateIdByEventIdAsync(Guid eventId);
    
    Task<Guid?> FindTemplateIdByActiveEventAsync(DateTime onDate);

    Task<IEnumerable<Event>> SelectActiveAsync(DateTime onDate);

    Task<IEnumerable<EventBase>> SelectActiveBaseAsync(DateTime onDate);
}