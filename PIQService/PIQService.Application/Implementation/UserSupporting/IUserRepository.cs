using PIQService.Models.Domain;

namespace PIQService.Application.Implementation.UserSupporting;

public interface IUserRepository
{
    Task<UserWithoutDeps?> FindAsync(Guid id);
}