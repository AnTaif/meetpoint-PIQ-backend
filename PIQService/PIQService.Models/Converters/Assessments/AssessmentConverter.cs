using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

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
            Teams = [],
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

    public static AssessmentWithoutDeps ToDomainWithoutDepsModel(this AssessmentDbo assessmentDbo) =>
        new(
            assessmentDbo.Id,
            assessmentDbo.Name,
            assessmentDbo.StartDate,
            assessmentDbo.EndDate
        );

    public static AssessmentDto ToDtoModel(this AssessmentWithoutDeps assessment) =>
        new()
        {
            Id = assessment.Id,
            Name = assessment.Name,
            StartDate = assessment.StartDate,
            EndDate = assessment.EndDate,
        };
}