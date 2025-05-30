using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Forms;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class FormRepository(AppDbContext dbContext, ILogger<FormRepository> logger) : IFormRepository
{
    public async Task<Form?> FindAsync(Guid formId)
    {
        logger.LogInformation("Searching for form...");
        
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