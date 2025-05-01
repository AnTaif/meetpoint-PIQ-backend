using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Converters.Assessments;

public static class FormConverter
{
    public static FormDbo ToDboModel(this Form form) =>
        new()
        {
            Id = form.Id,
            Type = form.Type,
            CriteriaList = form.CriteriaList.Select(c => c.ToDboModel()).ToList(),
        };

    public static Form ToDomainModel(this FormDbo formDbo) =>
        new(
            formDbo.Id,
            formDbo.Type,
            formDbo.CriteriaList.Select(c => c.ToDomainModel()).ToList(),
            formDbo.Questions.Select(q => q.ToDomainModel()).ToList()
        );
}