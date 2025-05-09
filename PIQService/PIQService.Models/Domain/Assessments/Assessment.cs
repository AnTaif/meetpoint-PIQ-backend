namespace PIQService.Models.Domain.Assessments;

public class Assessment : AssessmentWithoutDeps
{
    public Team Team { get; private set; }

    public Template Template { get; private set; }

    public Assessment(Guid id, string name, Team team, Template template, DateTime startDate, DateTime endDate,
        bool useCircleAssessment, bool useBehaviorAssessment)
        : base(id, name, startDate, endDate, useCircleAssessment, useBehaviorAssessment, template.Id, team.Id)
    {
        Team = team;
        Template = template;
    }
}