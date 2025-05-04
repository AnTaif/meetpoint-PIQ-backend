namespace PIQService.Models.Domain.Assessments;

public class Assessment : AssessmentWithoutDeps
{
    public List<Team> Teams { get; private set; }

    public Template Template { get; private set; }

    public Assessment(Guid id, string name, List<Team> teams, Template template, DateTime startDate, DateTime endDate,
        bool useCircleAssessment, bool useBehaviorAssessment)
        : base(id, name, startDate, endDate, useCircleAssessment, useBehaviorAssessment, template.Id)
    {
        Teams = teams;
        Template = template;
    }
}