using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.Users;

public interface IUserRepository
{
    Task<UserWithoutDeps?> FindAsync(Guid id);
}