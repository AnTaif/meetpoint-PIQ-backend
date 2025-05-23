namespace PIQService.Models.Dto;

public class UserMeanScoreDto
{
    public Guid UserId { get; init; }

    public string FullName { get; init; } = null!;

    public Guid TeamId { get; init; }
    
    public string TeamName { get; init; } = null!;

    public Dictionary<Guid, double> ScoreByCriteriaIds { get; init; } = null!;
}