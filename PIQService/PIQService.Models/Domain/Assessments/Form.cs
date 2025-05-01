namespace PIQService.Models.Domain.Assessments;

public class Form
{
    public Guid Id { get; private set; }

    public AssessmentType Type { get; private set; }

    public List<Criteria> CriteriaList { get; private set; }

    public List<Question> Questions { get; private set; }

    public Form(Guid id, AssessmentType type, List<Criteria> criteriaList, List<Question> questions)
    {
        Id = id;
        Type = type;
        CriteriaList = criteriaList;
        Questions = questions;
    }
}