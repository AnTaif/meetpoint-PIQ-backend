using Core.Auth;
using Core.Results;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

[RegisterScoped]
public class AssessmentCreationService(
    HybridCache cache,
    IEventService eventService,
    ITeamRepository teamRepository,
    IAssessmentRepository assessmentRepository,
    ILogger<AssessmentCreationService> logger
)
    : IAssessmentCreationService
{
    public async Task<Result<AssessmentDto>> CreateAssessmentForTeamAsync(
        Guid teamId, CreateTeamAssessmentRequest request, ContextUser contextUser)
    {
        var result = await CreateAssessmentsForTeamsAsync([teamId], request, contextUser);
        return result.IsFailure ? result.Error : result.Value.Single();
    }

    public async Task<Result<IEnumerable<AssessmentDto>>> CreateAssessmentsForTeamsAsync(
        CreateTeamsAssessmentRequest request, ContextUser contextUser)
    {
        return await CreateAssessmentsForTeamsAsync(request.TeamIds, request, contextUser);
    }

    private async Task<Result<IEnumerable<AssessmentDto>>> CreateAssessmentsForTeamsAsync(
        IReadOnlyCollection<Guid> teamIds, CreateAssessmentRequestBase request, ContextUser contextUser)
    {
        var teams = await teamRepository.SelectWithoutDepsAsync(teamIds);

        if (teams.Count != teamIds.Count)
        {
            logger.LogError(
                "Какая-то команда не найдена, ids={ids}",
                teamIds.Except(teams.Select(t => t.Id))
            );
        }

        var currentEvent = await eventService.FindEventWithoutDepsAsync(null);
        if (currentEvent == null)
        {
            return StatusError.NotFound("Current event not found");
        }

        var assessments = new List<AssessmentDto>();
        foreach (var assessmentResult in CreateAssessmentsForTeams(teams, request, currentEvent.TemplateId, contextUser))
        {
            if (assessmentResult.IsFailure)
            {
                return assessmentResult.Error;
            }

            assessmentRepository.Create(assessmentResult.Value);
            assessments.Add(assessmentResult.Value.ToDtoModel(-1, -1));
        }

        await cache.RemoveAsync($"requires_evaluation_by_user_{contextUser.Id}");
        await assessmentRepository.SaveChangesAsync();
        return assessments;
    }

    private static IEnumerable<Result<AssessmentWithoutDeps>> CreateAssessmentsForTeams(
        IEnumerable<TeamWithoutDeps> teams, CreateAssessmentRequestBase request, Guid templateId, ContextUser contextUser)
    {
        foreach (var team in teams)
        {
            if (!CanCreateAssessment(team, contextUser))
            {
                yield return StatusError.Forbidden($"Вы не можете создать оценивание для данной команды, teamId={team.Id}");
            }

            var newAssessment = CreateAssessment(request, templateId, team.Id);

            yield return newAssessment;
        }
    }

    private static bool CanCreateAssessment(TeamWithoutDeps team, ContextUser user)
    {
        return team.TutorId == user.Id || user.Roles.Contains(RolesConstants.Admin);
    }

    private static AssessmentWithoutDeps CreateAssessment(CreateAssessmentRequestBase request, Guid templateId, Guid teamId) =>
        new(
            Guid.NewGuid(),
            request.Name,
            request.StartDate,
            request.EndDate,
            request.UseCircleAssessment,
            request.UseBehaviorAssessment,
            templateId,
            teamId
        );
}