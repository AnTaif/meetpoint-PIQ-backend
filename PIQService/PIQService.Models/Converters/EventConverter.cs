using PIQService.Models.Dbo;
using PIQService.Models.Domain;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters;

public static class EventConverter
{
    public static EventDbo ToDboModel(this Event domainModel)
    {
        return new EventDbo
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            StartDate = domainModel.StartDate,
            EndDate = domainModel.EndDate,
        };
    }

    public static Event ToDomainModel(this EventDbo domainModel)
    {
        return new Event(
            domainModel.Id,
            domainModel.Name,
            domainModel.StartDate,
            domainModel.EndDate
        );
    }

    public static EventDto ToDtoModel(this Event @event)
    {
        return new EventDto
        {
            Id = @event.Id,
            Name = @event.Name,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
        };
    }
}