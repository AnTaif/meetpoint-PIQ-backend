using Core.Auth;
using Core.Results;
using PIQService.Application.Implementation.Assessments.Reports;
using PIQService.Application.Implementation.Assessments.Sessions.Requests;
using PIQService.Application.Implementation.EventSupporting.Teams;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments.Sessions;

public interface IAssessmentSessionService
{
    Task<Result<AssessmentDto>> GetAssessmentAsync(Guid assessmentId, ContextUser contextUser);
    Task<Result<List<AssessmentDto>>> GetAssessmentsForTeamAsync(Guid teamId, ContextUser contextUser);
    Task<Result<AssessmentDto>> CreateAssessmentForTeamAsync(Guid teamId, CreateAssessmentForTeamRequest request, ContextUser contextUser);
    Task<Result<IEnumerable<AssessmentDto>>> CreateAssessmentsForTeamsAsync(CreateAssessmentsForTeamsRequest request, ContextUser contextUser);
    Task<Result<AssessmentDto>> EditAssessmentAsync(Guid id, EditAssessmentRequest request, ContextUser contextUser);
    Task<Result> DeleteAssessmentAsync(Guid id, ContextUser contextUser);
}

[RegisterScoped]
public class AssessmentSessionService(
    IAssessmentRepository assessmentRepository,
    ITeamRepository teamRepository,
    IAssessmentReportService assessmentReportService,
    IAssessmentCreator assessmentCreator,
    ISecurityService securityService
)
    : IAssessmentSessionService
{
    public async Task<Result<AssessmentDto>> GetAssessmentAsync(Guid assessmentId, ContextUser contextUser)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
            return StatusError.NotFound("Assessment not found");

        return assessment.ToDtoModel(-1, -1);
    }

    public async Task<Result<List<AssessmentDto>>> GetAssessmentsForTeamAsync(Guid teamId, ContextUser contextUser)
    {
        var team = await teamRepository.FindWithoutDepsAsync(teamId);
        if (team == null)
        {
            return StatusError.NotFound("Team not found");
        }

        var assessments = await assessmentRepository.SelectByTeamIdAsync(teamId);
        var dtos = new List<AssessmentDto>();
        foreach (var assessment in assessments)
        {
            var assessUsersResult = await assessmentReportService.GetUsersToReportAsync(assessment.Id, contextUser);
            if (assessUsersResult.IsFailure)
                return assessUsersResult.Error;

            var assessUsers = assessUsersResult.Value;

            dtos.Add(assessment.ToDtoModel(assessUsers.Count, assessUsers.Count(u => !u.Assessed)));
        }

        return dtos;
    }

    public async Task<Result<AssessmentDto>> CreateAssessmentForTeamAsync(
        Guid teamId, CreateAssessmentForTeamRequest request, ContextUser contextUser)
    {
        if (!await securityService.IsAdminOrTeamsTutorAsync(contextUser, teamId))
        {
            return StatusError.Forbidden();
        }
        
        return await assessmentCreator.CreateSingleForTeamAsync(request, teamId);
    }

    public async Task<Result<IEnumerable<AssessmentDto>>> CreateAssessmentsForTeamsAsync(
        CreateAssessmentsForTeamsRequest request, ContextUser contextUser)
    {
        if (!await securityService.IsAdminOrTeamsTutorAsync(contextUser, request.TeamIds))
        {
            return StatusError.Forbidden();
        }
        
        return await assessmentCreator.CreateManyForTeamsAsync(request);
    }

    public async Task<Result<AssessmentDto>> EditAssessmentAsync(Guid id, EditAssessmentRequest request, ContextUser contextUser)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(id);
        if (assessment == null)
        {
            return StatusError.NotFound("Assessment not found");
        }

        if (!await securityService.IsAdminOrTeamsTutorAsync(contextUser, assessment.TeamId))
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

    public async Task<Result> DeleteAssessmentAsync(Guid id, ContextUser contextUser)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(id);

        if (assessment == null)
        {
            return StatusError.NotFound("Assessment not found");
        }

        if (!await securityService.IsAdminOrTeamsTutorAsync(contextUser, assessment.TeamId))
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
}