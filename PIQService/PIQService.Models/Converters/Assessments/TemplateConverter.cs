using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Models.Converters.Assessments;

public static class TemplateConverter
{
    public static TemplateDbo ToDboModel(this Template template) =>
        new()
        {
            Id = template.Id,
            Name = template.Name,
            CircleFormId = template.CircleForm.Id,
            BehaviorFormId = template.BehaviorForm.Id,
        };

    public static Template ToDomainModel(this TemplateDbo templateDbo) =>
        new(templateDbo.Id, templateDbo.Name, templateDbo.CircleForm.ToDomainModel(), templateDbo.BehaviorForm.ToDomainModel());
}