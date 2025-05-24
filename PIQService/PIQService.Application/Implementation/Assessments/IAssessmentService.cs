using Core.Auth;
using Core.Results;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentService
{
    Task<Result<IEnumerable<AssessmentDto>>> GetTeamAssessments(Guid teamId, ContextUser contextUser);

    Task<Result<List<AssessUserDto>>> SelectUsersToAssessAsync(Guid currentUserId, Guid assessmentId);

    Task<Result<IEnumerable<AssessChoiceDto>>> SelectAssessChoicesAsync(Guid assessmentId, Guid assessorId, Guid assessedId);

    Task<Result<AssessmentDto>> CreateTeamAssessmentAsync(Guid teamId, CreateTeamAssessmentRequest request, ContextUser contextUser);

    Task<Result<IEnumerable<AssessmentDto>>> CreateTeamsAssessmentAsync(CreateTeamsAssessmentRequest request, ContextUser contextUser);

    Task<Result<AssessmentMarkDto>> AssessUserAsync(Guid assessmentId, Guid assessorUserId, Guid assessedUserId, IEnumerable<Guid> selectedChoiceIds);

    Task<Result<AssessmentDto>> EditAssessmentAsync(Guid id, EditAssessmentRequest request, ContextUser contextUser);

    Task<Result> DeleteAsync(Guid id, ContextUser contextUser);
}