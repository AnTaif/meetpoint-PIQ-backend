namespace PIQService.Models.Dto;

public class TeamHierarchyDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public bool AssessmentRequired { get; init; }

    public UserDto Tutor { get; init; } = null!;

    public IEnumerable<UserDto> Members { get; init; } = null!;
}