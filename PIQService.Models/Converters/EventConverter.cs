using PIQService.Models.Dbo;
using PIQService.Models.Domain;

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
}