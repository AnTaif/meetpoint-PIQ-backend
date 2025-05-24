using Core.Auth;
using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Scores;

public interface IScoreService
{
    Task<Result<UserMeanScoreDto>> GetUserMeanScoresAsync(Guid userId, ContextUser contextUser);
    
    /// <param name="onlyWhereTutor">Параметр для админов-тьюторов, если false - можно получить всю иерархию</param>
    Task<Result<List<UserMeanScoreDto>>> GetUsersMeanScoresByFormIdAsync(Guid formId, ContextUser contextUser, bool onlyWhereTutor = true);
}