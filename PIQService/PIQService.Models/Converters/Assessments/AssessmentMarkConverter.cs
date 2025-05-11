using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Converters.Assessments;

public static class AssessmentMarkConverter
{
    public static AssessmentMarkDbo ToDboModel(this AssessmentMark mark) =>
        new()
        {
            Id = mark.Id,
            AssessorId = mark.Assessor.Id,
            AssessedId = mark.Assessed.Id,
            AssessmentId = mark.Assessment.Id,
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
}