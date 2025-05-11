using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters.Assessments;

public static class AssessmentMarkConverter
{
    public static AssessmentMarkDbo ToDboModel(this AssessmentMarkWithoutDeps mark) =>
        new()
        {
            Id = mark.Id,
            AssessorId = mark.AssessorId,
            AssessedId = mark.AssessedId,
            AssessmentId = mark.AssessmentId,
            Choices = mark.Choices.Select(c => new ChoiceDbo() { Id = c.Id }).ToList(),
        };

    public static AssessmentMark ToDomainModel(this AssessmentMarkDbo markDbo) =>
        new(
            markDbo.Id,
            markDbo.Assessor.ToDomainModel(),
            markDbo.Assessed.ToDomainModel(),
            markDbo.Assessment.ToDomainModel(),
            markDbo.Choices.Select(c => c.ToDomainModel()).ToList()
        );

    public static AssessmentMarkWithoutDeps ToDomainModelWithoutDeps(this AssessmentMarkDbo markDbo) =>
        new(
            markDbo.Id,
            markDbo.AssessorId,
            markDbo.AssessedId,
            markDbo.AssessmentId,
            markDbo.Choices.Select(c => c.ToDomainModel()).ToList()
        );

    public static AssessmentMarkDto ToDtoModel(this AssessmentMarkWithoutDeps assessmentMark) =>
        new()
        {
            Id = assessmentMark.Id,
            AssessorId = assessmentMark.AssessorId,
            AssessedId = assessmentMark.AssessedId,
            AssessmentId = assessmentMark.AssessmentId,
            Choices = assessmentMark.Choices.Select(c => c.Id).ToList(),
        };
}