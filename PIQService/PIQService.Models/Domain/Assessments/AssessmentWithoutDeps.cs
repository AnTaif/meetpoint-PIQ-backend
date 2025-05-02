namespace PIQService.Models.Domain.Assessments;

public class AssessmentWithoutDeps
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public AssessmentWithoutDeps(Guid id, string name, DateTime startDate, DateTime endDate)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }
}