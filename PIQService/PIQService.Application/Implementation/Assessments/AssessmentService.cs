using Core.Auth;
using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

[RegisterScoped]
public class AssessmentService(
    IAssessmentScoringService assessmentScoringService,
    ITeamRepository teamRepository,
    IAssessmentRepository assessmentRepository,
    ILogger<AssessmentService> logger
)
    : IAssessmentService
{
    public async Task<Result<AssessmentDto>> GetAssessmentAsync(Guid assessmentId, ContextUser contextUser)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
            return StatusError.NotFound("Assessment not found");
        
        return assessment.ToDtoModel(-1, -1);
    }

    public async Task<Result<List<AssessmentDto>>> GetTeamAssessmentsAsync(Guid teamId, ContextUser contextUser)
    {
        var team = await teamRepository.FindAsync(teamId);
        if (team == null)
        {
            return StatusError.NotFound("Team not found");
        }

        var assessments = await assessmentRepository.SelectByTeamIdAsync(teamId);
        var dtos = new List<AssessmentDto>();
        foreach (var assessment in assessments)
        {
            var assessUsersResult = await assessmentScoringService.GetUsersToScoreAsync(assessment.Id, contextUser);
            if (assessUsersResult.IsFailure)
                return assessUsersResult.Error;

            var assessUsers = assessUsersResult.Value;

            dtos.Add(assessment.ToDtoModel(assessUsers.Count, assessUsers.Count(u => !u.Assessed)));
        }

        return dtos;
    }

    public async Task<Result<AssessmentDto>> EditAssessmentAsync(Guid id, EditAssessmentRequest request, ContextUser contextUser)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(id);
        if (assessment == null)
        {
            return StatusError.NotFound("Assessment not found");
        }

        if (!await CanManageAssessmentAsync(assessment, contextUser))
        {
            return StatusError.Forbidden("Вы не можете редактировать данное оценивание");
        }

        var newStart = request.StartDate ?? assessment.StartDate;
        var newEnd = request.EndDate ?? assessment.EndDate;

        if (newStart > newEnd)
        {
            return StatusError.BadRequest("StartDate must be earlier than EndDate");
        }

        assessment.Edit(request.Name, request.StartDate, request.EndDate, request.UseCircleAssessment, request.UseBehaviorAssessment);

        assessmentRepository.Update(assessment);
        await assessmentRepository.SaveChangesAsync();

        return assessment.ToDtoModel(-1, -1);
    }

    public async Task<Result> DeleteAsync(Guid id, ContextUser contextUser)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(id);

        if (assessment == null)
        {
            return StatusError.NotFound("Assessment not found");
        }

        if (!await CanManageAssessmentAsync(assessment, contextUser))
        {
            return StatusError.Forbidden("Вы не можете редактировать данное оценивание");
        }

        if (assessment.EndDate <= DateTime.UtcNow)
        {
            return StatusError.Conflict("Cannot delete completed assessment");
        }

        assessmentRepository.Delete(assessment);
        await assessmentRepository.SaveChangesAsync();

        return Result.Success;
    }

    private async Task<bool> CanManageAssessmentAsync(AssessmentWithoutDeps assessment, ContextUser user)
    {
        var team = await teamRepository.FindAsync(assessment.TeamId)
                   ?? throw new Exception("Error when finding assessment's team");

        return team.Tutor.Id == user.Id || user.Roles.Contains(RolesConstants.Admin);
    }
}