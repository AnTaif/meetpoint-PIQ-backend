namespace PIQService.Models.Dto;

public class CriteriaDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;
}