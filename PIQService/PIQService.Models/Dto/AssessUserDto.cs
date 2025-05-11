namespace PIQService.Models.Dto;

public class AssessUserDto
{
    public UserDto User { get; init; } = null!;

    public bool Assessed { get; init; }
}