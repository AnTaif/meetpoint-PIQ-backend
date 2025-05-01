using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Converters.Assessments;

public static class AssessmentConverter
{
    public static AssessmentDbo ToDboModel(this Assessment assessment) =>
        new()
        {
            Id = assessment.Id,
            Name = assessment.Name,
            TemplateId = assessment.Template.Id,
            StartDate = assessment.StartDate,
            EndDate = assessment.EndDate,
            Teams = assessment.Teams.Select(t => t.ToDboModel()).ToList(),
        };

    public static Assessment ToDomainModel(this AssessmentDbo assessmentDbo) =>
        new(
            assessmentDbo.Id,
            assessmentDbo.Name,
            assessmentDbo.Teams.Select(t => t.ToDomainModel()).ToList(), // TODO: зависимости не нужны
            assessmentDbo.Template.ToDomainModel(),
            assessmentDbo.StartDate,
            assessmentDbo.EndDate
        );
}