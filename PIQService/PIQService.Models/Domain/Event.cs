using System.Text.Json.Serialization;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Domain;

public class Event : EventBase
{
    public TemplateBase Template { get; private set; }

    [JsonConstructor]
    public Event(Guid id, string name, DateTime startDate, DateTime endDate, TemplateBase template)
        : base(id, name, startDate, endDate, template.Id)
    {
        Template = template;
    }
}