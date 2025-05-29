using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class TemplateRepository(AppDbContext dbContext) : ITemplateRepository
{
    public async Task<TemplateBase?> FindAsync(Guid templateId)
    {
        var dbo = await dbContext.Templates
            .SingleOrDefaultAsync(t => t.Id == templateId);

        return dbo?.ToDomainBaseModel();
    }
}