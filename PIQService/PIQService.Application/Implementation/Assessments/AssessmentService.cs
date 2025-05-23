using Core.Auth;
using Core.Extensions;
using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Assessments.Marks;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Teams;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public class AssessmentService(
    IAssessmentFormsService assessmentFormsService,
    IAssessmentMarkRepository markRepository,
    ITeamRepository teamRepository,
    ITemplateRepository templateRepository,
    IAssessmentRepository assessmentRepository,
    ILogger<AssessmentService> logger
)
    : IAssessmentService
{
    public async Task<Result<IEnumerable<AssessmentDto>>> GetTeamAssessments(Guid teamId)
    {
        var team = await teamRepository.FindAsync(teamId);
        if (team == null)
        {
            return StatusError.NotFound("Team not found");
        }

        var assessments = await assessmentRepository.SelectByTeamIdAsync(teamId);

        return assessments.Select(a => a.ToDtoModel()).ToList();
    }

    public async Task<Result<IEnumerable<AssessUserDto>>> SelectUsersToAssessAsync(Guid currentUserId, Guid assessmentId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
            return StatusError.NotFound("Assessment not found");

        var team = await teamRepository.FindAsync(assessment.TeamId);

        if (team == null)
        {
            logger.LogError(
                "Assessment with id={assessmentId} does not have team with id={teamId}",
                assessmentId,
                assessment.TeamId);

            return StatusError.NotFound("Team not found");
        }

        var assessedUsers = await markRepository.SelectAssessedUsersAsync(currentUserId, assessmentId);
        var assessedUsersById = assessedUsers.ToLookup(u => u.Id);

        var usersToAssess = team.Users
            .OrderBy(u => u.LastName)
            .Concat([team.Tutor])
            .Where(u => u.Id != currentUserId)
            .ToList();

        return usersToAssess.Select(u => new AssessUserDto
        {
            User = u.ToDtoModel(),
            Assessed = assessedUsersById.Contains(u.Id),
        }).ToList();
    }

    public async Task<Result<IEnumerable<AssessChoiceDto>>> SelectAssessChoicesAsync(Guid assessmentId, Guid assessorId, Guid assessedId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
            return StatusError.NotFound("Assessment not found");

        var mark = await markRepository.FindWithoutDepsAsync(assessmentId, assessorId, assessedId);

        if (mark == null)
            return Array.Empty<AssessChoiceDto>();

        return mark.Choices
            .Select(c => new AssessChoiceDto() { QuestionId = c.QuestionId, ChoiceId = c.Id })
            .ToList();
    }

    public async Task<Result<AssessmentDto>> CreateTeamAssessmentAsync(Guid teamId, CreateTeamAssessmentRequest request, ContextUser contextUser)
    {
        var result = await CreateAssessmentForTeamsAsync([teamId], request.Name, request.StartDate, request.EndDate,
            request.UseCircleAssessment, request.UseBehaviorAssessment, contextUser);

        return result.IsFailure
            ? result.Error
            : result.Value.Single();
    }

    public async Task<Result<IEnumerable<AssessmentDto>>> CreateTeamsAssessmentAsync(CreateTeamsAssessmentRequest request, ContextUser contextUser)
    {
        return await CreateAssessmentForTeamsAsync(request.TeamIds, request.Name, request.StartDate, request.EndDate,
            request.UseCircleAssessment, request.UseBehaviorAssessment, contextUser);
    }

    private async Task<Result<IEnumerable<AssessmentDto>>> CreateAssessmentForTeamsAsync(
        IReadOnlyCollection<Guid> teamIds, string name, DateTime startDate, DateTime endDate, bool useCircle, bool useBehavior, ContextUser contextUser)
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

        var assessments = new List<Assessment>();
        teams.Foreach(t =>
        {
            var newAssessment = new Assessment(Guid.NewGuid(), name, t, template, startDate, endDate, useCircle, useBehavior);

            assessmentRepository.Create(newAssessment);
            assessments.Add(newAssessment);
        });
        await assessmentRepository.SaveChangesAsync();

        return assessments.Select(a => a.ToDtoModel()).ToList();
    }

    public async Task<Result<AssessmentMarkDto>> AssessUserAsync(
        Guid assessmentId, Guid assessorUserId, Guid assessedUserId, IEnumerable<Guid> selectedChoiceIds)
    {
        var usedFormsResult = await assessmentFormsService.GetAssessmentUsedFormsAsync(assessmentId);

        if (usedFormsResult.IsFailure)
            return usedFormsResult.Error;

        var questions = usedFormsResult.Value.SelectMany(f => f.Questions).ToList();

        var choiceToQuestion = questions
            .SelectMany(q => q.Choices.Select(c => new { ChoiceId = c.Id, QuestionId = q.Id }))
            .ToDictionary(x => x.ChoiceId, x => x.QuestionId);

        var usedQuestionIds = new HashSet<Guid>();
        var choiceIds = selectedChoiceIds.ToList();

        foreach (var selectedChoiceId in choiceIds)
        {
            if (!choiceToQuestion.TryGetValue(selectedChoiceId, out var questionId))
            {
                return StatusError.NotFound($"Choice with id={selectedChoiceId} not found");
            }

            if (!usedQuestionIds.Add(questionId))
            {
                return StatusError.Conflict($"Для вопроса с id={questionId} выбрано несколько вариантов");
            }
        }

        var mark = await markRepository.FindWithoutDepsAsync(assessmentId, assessorUserId, assessedUserId);
        var newChoices = choiceIds.Select(id => new Choice(id)).ToList();

        if (mark == null)
        {
            mark = new AssessmentMarkWithoutDeps(Guid.NewGuid(), assessorUserId, assessedUserId, assessmentId, newChoices);
            markRepository.Create(mark, choiceIds);
        }
        else
        {
            mark.UpdateChoices(newChoices);
            markRepository.UpdateChoices(mark, choiceIds);
        }

        await assessmentRepository.SaveChangesAsync();
        return mark.ToDtoModel();
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

        return assessment.ToDtoModel();
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

        return Result.Success();
    }

    private async Task<bool> CanManageAssessmentAsync(AssessmentWithoutDeps assessment, ContextUser user)
    {
        var team = await teamRepository.FindAsync(assessment.TeamId)
                   ?? throw new Exception("Error when finding assessment's team");

        return team.Tutor.Id == user.Id || user.Roles.Contains(RolesConstants.Admin);
    }

    private bool CanCreateAssessment(Team team, ContextUser user)
    {
        return team.Tutor.Id == user.Id || user.Roles.Contains(RolesConstants.Admin);
    }
}