namespace PIQService.Models.Dto;

public class AssessmentDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public Guid TeamId { get; init; }

    public bool UseCircleAssessment { get; init; }

    public bool UseBehaviorAssessment { get; init; }
    
    /// <summary>
    /// Количество пользователей, которое может оценить текущий пользователь
    /// </summary>
    public int AssessUsersCount { get; init; }
    
    /// <summary>
    /// Количество неоцененных пользователей
    /// </summary>
    /// <returns></returns>
    public int NotAssessedCount { get; init; }
}