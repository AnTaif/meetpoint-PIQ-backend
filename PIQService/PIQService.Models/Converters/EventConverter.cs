using PIQService.Models.Converters.Assessments;
using PIQService.Models.Dbo;
using PIQService.Models.Domain;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters;

public static class EventConverter
{
    public static EventDbo ToDboModel(this EventBase domainModel)
    {
        return new EventDbo
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            StartDate = domainModel.StartDate,
            EndDate = domainModel.EndDate,
        };
    }

    public static Event ToDomainModel(this EventDbo domainModel) =>
        new(
            domainModel.Id,
            domainModel.Name,
            domainModel.StartDate,
            domainModel.EndDate,
            domainModel.Template.ToDomainBaseModel()
        );

    public static EventBase ToDomainBaseModel(this EventDbo domainModel) =>
        new(
            domainModel.Id,
            domainModel.Name,
            domainModel.StartDate,
            domainModel.EndDate,
            domainModel.TemplateId
        );

    public static EventDto ToDtoModel(this EventBase eventBase) =>
        new()
        {
            Id = eventBase.Id,
            Name = eventBase.Name,
            StartDate = eventBase.StartDate,
            EndDate = eventBase.EndDate,
        };
}