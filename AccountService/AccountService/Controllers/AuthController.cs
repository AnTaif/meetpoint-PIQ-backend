using AccountService.Contracts;
using AccountService.Docs.RequestExamples;
using AccountService.Docs.ResponseExamples;
using AccountService.Services;
using Core.Results;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace AccountService.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService
)
    : ControllerBase
{
    [HttpPost("login")]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoginResponseExample))]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest);
        return result.ToActionResult(this);
    }
}