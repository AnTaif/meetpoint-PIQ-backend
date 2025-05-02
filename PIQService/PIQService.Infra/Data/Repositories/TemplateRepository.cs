using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

public class TemplateRepository(AppDbContext dbContext) : ITemplateRepository
{
    public async Task<Template?> FindAsync(Guid templateId)
    {
        var dbo = await dbContext.Templates
            .Include(t => t.CircleForm)
            .ThenInclude(f => f.CriteriaList)
            .Include(t => t.CircleForm)
            .ThenInclude(f => f.Questions)
            .ThenInclude(q => q.Criteria)
            .Include(t => t.CircleForm)
            .ThenInclude(f => f.Questions)
            .ThenInclude(q => q.Choices)
            .SingleOrDefaultAsync(t => t.Id == templateId);
        return dbo?.ToDomainModel();
    }
}