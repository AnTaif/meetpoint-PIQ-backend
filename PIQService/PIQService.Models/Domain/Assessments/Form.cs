using System.Text.Json.Serialization;

namespace PIQService.Models.Domain.Assessments;

public class Form
{
    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public AssessmentType Type { get; private set; }

    [JsonInclude]
    public List<Criteria> CriteriaList { get; private set; }

    [JsonInclude]
    public List<Question> Questions { get; private set; }

    [JsonConstructor]
    public Form(Guid id, AssessmentType type, List<Criteria> criteriaList, List<Question> questions)
    {
        Id = id;
        Type = type;
        CriteriaList = criteriaList;
        Questions = questions;
    }
}