using PIQService.Models.Dbo;
using PIQService.Models.Domain;

namespace PIQService.Models.Converters;

public static class DirectionConverter
{
    public static DirectionDbo ToDboModel(this Direction domainModel)
    {
        return new DirectionDbo
        {
            Id = domainModel.Id,
            EventId = domainModel.Event.Id,
            Name = domainModel.Name
        };
    }

    public static Direction ToDomainModel(this DirectionDbo dbo)
    {
        return new Direction(
            dbo.Id,
            dbo.Event.ToDomainModel(),
            dbo.Name
        );
    }
}