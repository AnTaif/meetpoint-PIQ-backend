namespace PIQService.Models.Domain.Assessments;

public class AssessmentMark
{
    public Guid Id { get; private set; }

    public User Assessor { get; private set; }

    public User Assessed { get; private set; }

    public Assessment Assessment { get; private set; }

    public List<Choice> Choices { get; private set; }

    public AssessmentMark(Guid id, User assessor, User assessed, Assessment assessment, List<Choice> choices)
    {
        Id = id;
        Assessor = assessor;
        Assessed = assessed;
        Assessment = assessment;
        Choices = choices;
    }
}