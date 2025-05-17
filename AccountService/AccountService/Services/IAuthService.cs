using AccountService.Contracts;
using Core.Results;

namespace AccountService.Services;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
}