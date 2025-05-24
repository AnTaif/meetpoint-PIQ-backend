using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Users;
using PIQService.Models.Converters;
using PIQService.Models.Domain;

namespace PIQService.Infra.Data.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<UserWithoutDeps?> FindAsync(Guid id)
    {
        var dbo = await dbContext.Users
            .Include(u => u.Team)
            .SingleOrDefaultAsync(u => u.Id == id);

        return dbo?.ToDomainModelWithoutDeps();
    }
}