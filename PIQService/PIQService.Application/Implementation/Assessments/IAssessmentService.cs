using Core.Results;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentService
{
    Task<Result<IEnumerable<AssessmentDto>>> GetTeamAssessments(Guid teamId);

    Task<Result<AssessmentDto>> CreateTeamAssessmentAsync(Guid teamId, CreateTeamAssessmentRequest request);

    Task<Result<AssessmentDto>> CreateTeamsAssessmentAsync(CreateTeamsAssessmentRequest request);

    Task<Result<AssessmentDto>> EditAssessmentAsync(Guid id, EditAssessmentRequest request, Guid userId);

    Task<Result> DeleteAsync(Guid id);
}