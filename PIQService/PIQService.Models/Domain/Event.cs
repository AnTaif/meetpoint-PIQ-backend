namespace PIQService.Models.Domain;

public class Event
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public Guid TemplateId { get; private set; }

    public Event(Guid id, string name, DateTime startDate, DateTime endDate, Guid templateId)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        TemplateId = templateId;
    }
}