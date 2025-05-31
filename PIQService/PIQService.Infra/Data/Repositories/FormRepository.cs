using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Forms;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class FormRepository(AppDbContext dbContext) : IFormRepository
{
    public async Task<Form?> FindAsync(Guid formId)
    {
        var dbo = await dbContext.Forms
            .Include(f => f.Questions)
            .ThenInclude(q => q.Choices)
            .Include(f => f.Questions)
            .ThenInclude(q => q.Criteria)
            .Include(f => f.CriteriaList)
            .SingleOrDefaultAsync(f => f.Id == formId);

        return dbo?.ToDomainModel();
    }
    
    
}