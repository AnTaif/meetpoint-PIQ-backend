using Core.Auth;
using Core.Results;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentCreationService
{
    Task<Result<AssessmentDto>> CreateAssessmentForTeamAsync(Guid teamId, CreateTeamAssessmentRequest request, ContextUser contextUser);

    Task<Result<IEnumerable<AssessmentDto>>> CreateAssessmentsForTeamsAsync(CreateTeamsAssessmentRequest request, ContextUser contextUser);
}