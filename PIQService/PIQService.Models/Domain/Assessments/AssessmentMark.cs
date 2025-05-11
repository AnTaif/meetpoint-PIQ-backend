namespace PIQService.Models.Domain.Assessments;

public class AssessmentMark : AssessmentMarkWithoutDeps
{
    public User Assessor { get; private set; }

    public User Assessed { get; private set; }

    public Assessment Assessment { get; private set; }

    public AssessmentMark(Guid id, User assessor, User assessed, Assessment assessment, List<Choice> choices)
        : base(id, assessor.Id, assessed.Id, assessment.Id, choices)
    {
        Assessor = assessor;
        Assessed = assessed;
        Assessment = assessment;
    }
}