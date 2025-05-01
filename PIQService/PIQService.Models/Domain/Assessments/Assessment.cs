namespace PIQService.Models.Domain.Assessments;

public class Assessment
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public List<Team> Teams { get; private set; }

    public Template Template { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public Assessment(Guid id, string name, List<Team> teams, Template template, DateTime startDate, DateTime endDate)
    {
        Id = id;
        Name = name;
        Teams = teams;
        Template = template;
        StartDate = startDate;
        EndDate = endDate;
    }
}