namespace AccountService.Contracts.Models;

public class LoginRequest
{
    public string Email { get; } = null!;
    public string Password { get; } = null!;
}