using Core.Auth;
using Core.Extensions;
using Core.Results;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Teams;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

[RegisterScoped]
public class AssessmentCreationService(
    ITemplateRepository templateRepository,
    ITeamRepository teamRepository,
    IAssessmentRepository assessmentRepository
)
    : IAssessmentCreationService
{
    public async Task<Result<AssessmentDto>> CreateTeamAssessmentAsync(Guid teamId, CreateTeamAssessmentRequest request,
        ContextUser contextUser)
    {
        var result = await CreateAssessmentForTeamsAsync([teamId], request.Name, request.StartDate, request.EndDate,
            request.UseCircleAssessment, request.UseBehaviorAssessment, contextUser);

        return result.IsFailure
            ? result.Error
            : result.Value.Single();
    }

    public async Task<Result<IEnumerable<AssessmentDto>>> CreateTeamsAssessmentAsync(CreateTeamsAssessmentRequest request,
        ContextUser contextUser)
    {
        return await CreateAssessmentForTeamsAsync(request.TeamIds, request.Name, request.StartDate, request.EndDate,
            request.UseCircleAssessment, request.UseBehaviorAssessment, contextUser);
    }

    private async Task<Result<IEnumerable<AssessmentDto>>> CreateAssessmentForTeamsAsync(
        IReadOnlyCollection<Guid> teamIds, string name, DateTime startDate, DateTime endDate, bool useCircle, bool useBehavior,
        ContextUser contextUser)
    {
        if (teamIds.Count == 0)
        {
            return StatusError.BadRequest("TeamIds is empty");
        }

        if (startDate > endDate)
        {
            return StatusError.BadRequest("StartDate must be earlier than EndDate");
        }

        if (!useCircle && !useBehavior)
        {
            return StatusError.BadRequest("At least one form must be selected.");
        }

        var teams = new List<Team>();
        foreach (var teamId in teamIds)
        {
            var team = await teamRepository.FindAsync(teamId);
            if (team == null)
            {
                return StatusError.NotFound($"Team with id={teamId} not found");
            }

            if (!CanCreateAssessment(team, contextUser))
            {
                return StatusError.Forbidden("Вы не можете создать оценивание для данной команды");
            }

            teams.Add(team);
        }

        var templateId = teams.First().Project.Direction.Event.TemplateId;
        var template = await templateRepository.FindAsync(templateId);
        if (template == null)
        {
            return StatusError.NotFound("Template not found");
        }

        var assessments = new List<AssessmentWithoutDeps>();
        teams.Foreach(t =>
        {
            var newAssessment = new AssessmentWithoutDeps(Guid.NewGuid(), name, startDate, endDate, useCircle, useBehavior, template.Id, t.Id);

            assessmentRepository.Create(newAssessment);
            assessments.Add(newAssessment);
        });
        await assessmentRepository.SaveChangesAsync();

        return assessments.Select(a => a.ToDtoModel(-1, -1)).ToList();
    }
    
    private static bool CanCreateAssessment(Team team, ContextUser user)
    {
        return team.Tutor.Id == user.Id || user.Roles.Contains(RolesConstants.Admin);
    }
}