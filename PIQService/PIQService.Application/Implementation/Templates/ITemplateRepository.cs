using PIQService.Models.Domain.Assessments;

namespace PIQService.Application.Implementation.Templates;

public interface ITemplateRepository
{
    Task<Template?> FindAsync(Guid templateId);
}