namespace PIQService.Models.Dto;

public class TeamDto
{
    public Guid Id { get; init; }

    public ProjectDto Project { get; init; } = null!;

    public UserDto Tutor { get; init; } = null!;

    public string Name { get; init; } = null!;

    public IEnumerable<UserDto> Members { get; init; } = null!;
}