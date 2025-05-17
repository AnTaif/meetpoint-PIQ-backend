using System.ComponentModel.DataAnnotations;

namespace AccountService.Contracts;

public class LoginRequest
{
    [EmailAddress]
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}