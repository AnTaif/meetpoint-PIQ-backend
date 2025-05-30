using System.Text.Json.Serialization;

namespace PIQService.Models.Domain;

public class EventBase
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public Guid TemplateId { get; private set; }

    [JsonConstructor]
    public EventBase(Guid id, string name, DateTime startDate, DateTime endDate, Guid templateId)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        TemplateId = templateId;
    }
}