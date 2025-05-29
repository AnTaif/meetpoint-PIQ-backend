using Core.Auth;
using Core.Results;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentService
{
    Task<Result<AssessmentDto>> GetAssessmentAsync(Guid assessmentId, ContextUser contextUser);
    
    Task<Result<List<AssessmentDto>>> GetTeamAssessmentsAsync(Guid teamId, ContextUser contextUser);

    Task<Result<AssessmentDto>> EditAssessmentAsync(Guid id, EditAssessmentRequest request, ContextUser contextUser);

    Task<Result> DeleteAsync(Guid id, ContextUser contextUser);
}