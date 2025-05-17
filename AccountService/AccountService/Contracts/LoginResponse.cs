namespace AccountService.Contracts;

public class LoginResponse
{
    public string Email { get; init; } = null!;
    public string Token { get; init; } = null!;
}