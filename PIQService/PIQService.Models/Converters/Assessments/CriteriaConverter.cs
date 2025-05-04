using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters.Assessments;

public static class CriteriaConverter
{
    public static CriteriaDbo ToDboModel(this Criteria criteria) =>
        new()
        {
            Id = criteria.Id,
            Name = criteria.Name,
            Description = criteria.Description,
        };

    public static Criteria ToDomainModel(this CriteriaDbo criteriaDbo) =>
        new(criteriaDbo.Id, criteriaDbo.Name, criteriaDbo.Description);

    public static CriteriaDto ToDtoModel(this Criteria criteria) =>
        new()
        {
            Id = criteria.Id,
            Name = criteria.Name,
            Description = criteria.Description,
        };
}