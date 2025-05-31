using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Events;
using PIQService.Models.Converters;
using PIQService.Models.Domain;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class EventRepository(AppDbContext dbContext) : IEventRepository
{
    public async Task<Event?> FindAsync(Guid id)
    {
        var @event = await dbContext.Events
            .Include(e => e.Template)
            .SingleOrDefaultAsync(e => e.Id == id);

        return @event?.ToDomainModel();
    }

    public async Task<EventBase?> FindBaseAsync(Guid id)
    {
        var @event = await dbContext.Events.FindAsync(id);

        return @event?.ToDomainBaseModel();
    }

    public async Task<IEnumerable<Event>> SelectActiveAsync(DateTime onDate)
    {
        var events = await dbContext.Events
            .Include(e => e.Template)
            .Where(e => e.StartDate <= onDate && e.EndDate >= onDate)
            .ToListAsync();

        return events.Select(e => e.ToDomainModel());
    }

    public async Task<IEnumerable<EventBase>> SelectActiveBaseAsync(DateTime onDate)
    {
        var events = await dbContext.Events
            .Where(e => e.StartDate <= onDate && e.EndDate >= onDate)
            .ToListAsync();

        return events.Select(e => e.ToDomainBaseModel());
    }
}