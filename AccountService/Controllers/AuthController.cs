using AccountService.Contracts.Models;
using AccountService.Services;
using Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(
        IAuthService authService
    )
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest);
        return result.ToActionResult(this);
    }
}