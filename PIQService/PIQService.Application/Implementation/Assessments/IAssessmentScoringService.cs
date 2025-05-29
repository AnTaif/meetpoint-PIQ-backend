using Core.Auth;
using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentScoringService
{
    Task<Result<AssessmentMarkDto>> ScoreAsync(Guid assessmentId, Guid assessedUserId, ContextUser assessor, IReadOnlyCollection<Guid> choiceIds);   
    
    Task<Result<List<AssessUserDto>>> GetUsersToScoreAsync(Guid assessmentId, ContextUser contextUser);
    
    Task<Result<List<AssessChoiceDto>>> GetChoicesAsync(Guid assessmentId, Guid assessedId, ContextUser assessor);
}