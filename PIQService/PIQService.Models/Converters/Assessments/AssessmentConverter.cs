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
            UseCircleAssessment = assessment.UseCircleAssessment,
            UseBehaviorAssessment = assessment.UseBehaviorAssessment,
            TeamId = assessment.TeamId,
        };

    public static AssessmentDbo ToDboModel(this AssessmentWithoutDeps assessment) =>
        new()
        {
            Id = assessment.Id,
            Name = assessment.Name,
            TemplateId = assessment.TemplateId,
            StartDate = assessment.StartDate,
            EndDate = assessment.EndDate,
            UseCircleAssessment = assessment.UseCircleAssessment,
            UseBehaviorAssessment = assessment.UseBehaviorAssessment,
            TeamId = assessment.TeamId,
        };

    public static Assessment ToDomainModel(this AssessmentDbo assessmentDbo) =>
        new(
            assessmentDbo.Id,
            assessmentDbo.Name,
            assessmentDbo.Team.ToDomainModel(), // TODO: зависимости не нужны
            assessmentDbo.Template.ToDomainModel(),
            assessmentDbo.StartDate,
            assessmentDbo.EndDate,
            assessmentDbo.UseCircleAssessment,
            assessmentDbo.UseBehaviorAssessment
        );

    public static AssessmentWithoutDeps ToDomainWithoutDepsModel(this AssessmentDbo assessmentDbo) =>
        new(
            assessmentDbo.Id,
            assessmentDbo.Name,
            assessmentDbo.StartDate,
            assessmentDbo.EndDate,
            assessmentDbo.UseCircleAssessment,
            assessmentDbo.UseBehaviorAssessment,
            assessmentDbo.TemplateId,
            assessmentDbo.TeamId
        );

    public static AssessmentDto ToDtoModel(this AssessmentWithoutDeps assessment) =>
        new()
        {
            Id = assessment.Id,
            Name = assessment.Name,
            StartDate = assessment.StartDate,
            EndDate = assessment.EndDate,
            TeamId = assessment.TeamId,
            UseCircleAssessment = assessment.UseCircleAssessment,
            UseBehaviorAssessment = assessment.UseBehaviorAssessment,
        };
}