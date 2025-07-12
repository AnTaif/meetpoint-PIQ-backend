using PIQService.Models.Domain.Assessments;

namespace PIQService.Application.Implementation.Templates.Forms;

public interface IFormRepository
{
    Task<Form?> FindAsync(Guid formId);
}