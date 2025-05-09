namespace PIQService.Models.Domain.Assessments;

public class AssessmentWithoutDeps
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public bool UseCircleAssessment { get; private set; }

    public bool UseBehaviorAssessment { get; private set; }

    public Guid TeamId { get; init; }

    public Guid TemplateId { get; init; }

    public AssessmentWithoutDeps(Guid id, string name, DateTime startDate, DateTime endDate, bool useCircleAssessment,
        bool useBehaviorAssessment, Guid templateId, Guid teamId)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        UseCircleAssessment = useCircleAssessment;
        UseBehaviorAssessment = useBehaviorAssessment;
        TemplateId = templateId;
        TeamId = teamId;
    }

    public void Edit(string? name, DateTime? startDate, DateTime? endDate, bool? useCircleAssessment, bool? useBehaviorAssessment)
    {
        var isFutureAssessment = StartDate > DateTime.UtcNow;

        if (name != null) Name = name;
        if (startDate != null) StartDate = startDate.Value;
        if (endDate != null) EndDate = endDate.Value;

        if (isFutureAssessment)
        {
            if (useCircleAssessment != null) UseCircleAssessment = useCircleAssessment.Value;
            if (useBehaviorAssessment != null) UseBehaviorAssessment = useBehaviorAssessment.Value;
        }
    }
}