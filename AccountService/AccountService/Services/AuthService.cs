using AccountService.Contracts;
using AccountService.Models;
using AccountService.Providers;
using Core.Results;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Services;

public class AuthService(
    UserManager<User> userManager,
    ITokenProvider tokenProvider
)
    : IAuthService
{
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return StatusError.NotFound($"User with email {request.Email} not found");
        }

        var isSuccess = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isSuccess)
        {
            return StatusError.BadRequest("Bad credentials.");
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = tokenProvider.GenerateToken(user, roles);

        return new LoginResponse
        {
            Email = user.Email!,
            Token = token,
        };
    }
}