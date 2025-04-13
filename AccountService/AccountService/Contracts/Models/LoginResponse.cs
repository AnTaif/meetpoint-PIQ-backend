namespace AccountService.Contracts.Models;

public class LoginResponse
{
    public string Email { get; init; } = null!;
    public string Token { get; init; } = null!;
}