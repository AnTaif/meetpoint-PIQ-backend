using AccountService.Contracts.Models;
using AccountService.Data;
using AccountService.Models;
using AccountService.Providers;
using Core.Results;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> userManager;
    private readonly ITokenProvider tokenProvider;

    public AuthService(
        UserManager<User> userManager,
        ITokenProvider tokenProvider
    )
    {
        this.userManager = userManager;
        this.tokenProvider = tokenProvider;
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return HttpError.NotFound($"User with email {request.Email} not found");
        }
        
        var isSuccess = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isSuccess)
        {
            return HttpError.BadRequest("Bad credentials.");
        }

        var token = tokenProvider.GenerateToken(user, []);

        return new LoginResponse
        {
            Email = user.Email!,
            Token = token
        };
    }
}