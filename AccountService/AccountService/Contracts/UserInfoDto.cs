namespace AccountService.Contracts;

public class UserInfoDto
{
    public string FullName { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string AvatarUrl { get; init; } = null!;
}