using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Events;

public interface IEventRepository
{
    Task<Event?> FindAsync(Guid id);

    Task<IEnumerable<Event>> SelectActiveAsync(DateTime onDate);
}