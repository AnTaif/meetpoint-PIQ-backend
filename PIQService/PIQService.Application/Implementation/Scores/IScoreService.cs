using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Scores;

public interface IScoreService
{
    Task<Result<List<UserMeanScoreDto>>> GetUsersMeanScoresByFormIdAsync(Guid formId, Guid contextUserId);
}