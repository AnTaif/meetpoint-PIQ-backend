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

public class AssessmentService : IAssessmentService
{
    private readonly IAssessmentFormsService assessmentFormsService;
    private readonly IAssessmentMarkRepository markRepository;
    private readonly ITeamRepository teamRepository;
    private readonly ITemplateRepository templateRepository;
    private readonly IAssessmentRepository assessmentRepository;
    private readonly ILogger<AssessmentService> logger;

    public AssessmentService(
        IAssessmentFormsService assessmentFormsService,
        IAssessmentMarkRepository markRepository,
        ITeamRepository teamRepository,
        ITemplateRepository templateRepository,
        IAssessmentRepository assessmentRepository,
        ILogger<AssessmentService> logger)
    {
        this.assessmentFormsService = assessmentFormsService;
        this.markRepository = markRepository;
        this.teamRepository = teamRepository;
        this.templateRepository = templateRepository;
        this.assessmentRepository = assessmentRepository;
        this.logger = logger;
    }

    public async Task<Result<IEnumerable<AssessmentDto>>> GetTeamAssessments(Guid teamId)
    {
        var team = await teamRepository.FindAsync(teamId);
        if (team == null)
        {
            return HttpError.NotFound("Team not found");
        }

        var assessments = await assessmentRepository.SelectByTeamIdAsync(teamId);

        return assessments.Select(a => a.ToDtoModel()).ToList();
    }

    public async Task<Result<IEnumerable<AssessUserDto>>> SelectUsersToAssessAsync(Guid currentUserId, Guid assessmentId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
            return HttpError.NotFound("Assessment not found");

        var team = await teamRepository.FindAsync(assessment.TeamId);

        if (team == null)
        {
            logger.LogError(
                "Assessment with id={assessmentId} does not have team with id={teamId}",
                assessmentId,
                assessment.TeamId);

            return HttpError.NotFound("Team not found");
        }

        var assessedUsers = await markRepository.SelectAssessedUsersAsync(currentUserId, assessmentId);
        var assessedUsersById = assessedUsers.ToLookup(u => u.Id);

        var usersToAssess = team.Users
            .OrderBy(u => u.LastName)
            .Concat(team.Tutor.ToSingleArray())
            .Where(u => u.Id != currentUserId)
            .ToList();

        return usersToAssess.Select(u => new AssessUserDto
        {
            User = u.ToDtoModel(),
            Assessed = assessedUsersById.Contains(u.Id),
        }).ToList();
    }

    public async Task<Result<IEnumerable<Guid>>> SelectChoiceIdsAsync(Guid assessmentId, Guid assessorId, Guid assessedId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
            return HttpError.NotFound("Assessment not found");

        var mark = await markRepository.FindWithoutDepsAsync(assessmentId, assessorId, assessedId);

        if (mark == null)
            return Array.Empty<Guid>();

        var choiceIds = mark.Choices.Select(c => c.Id).ToList();
        return choiceIds;
    }

    public async Task<Result<AssessmentDto>> CreateTeamAssessmentAsync(Guid teamId, CreateTeamAssessmentRequest request)
    {
        var newAssessmentResult = await CreateAssessmentForTeamsAsync([teamId], request.Name, request.StartDate, request.EndDate,
            request.UseCircleAssessment, request.UseBehaviorAssessment);
        return newAssessmentResult.IsFailure ? newAssessmentResult.Error : newAssessmentResult.Value.First().ToDtoModel();
    }

    public async Task<Result<IEnumerable<AssessmentDto>>> CreateTeamsAssessmentAsync(CreateTeamsAssessmentRequest request)
    {
        var newAssessmentResult = await CreateAssessmentForTeamsAsync(request.TeamIds, request.Name, request.StartDate, request.EndDate,
            request.UseCircleAssessment, request.UseBehaviorAssessment);
        return newAssessmentResult.IsFailure ? newAssessmentResult.Error : newAssessmentResult.Value.Select(a => a.ToDtoModel()).ToList();
    }

    private async Task<Result<IEnumerable<Assessment>>> CreateAssessmentForTeamsAsync(
        IReadOnlyCollection<Guid> teamIds, string name, DateTime startDate, DateTime endDate, bool useCircleAssessment,
        bool useBehaviorAssessment)
    {
        if (teamIds.Count == 0)
        {
            return HttpError.BadRequest("TeamIds is empty");
        }

        var teams = new List<Team>();
        foreach (var teamId in teamIds)
        {
            var team = await teamRepository.FindAsync(teamId);
            if (team == null)
            {
                return HttpError.NotFound($"Team with id={teamId} not found");
            }

            teams.Add(team);
        }

        var templateId = teams.First().Project.Direction.Event.TemplateId;
        var template = await templateRepository.FindAsync(templateId);
        if (template == null)
        {
            return HttpError.NotFound("Template not found");
        }

        var assessments = new List<Assessment>();
        teams.Foreach(t =>
        {
            var newAssessment = new Assessment(Guid.NewGuid(), name, t, template, startDate, endDate, useCircleAssessment,
                useBehaviorAssessment);

            assessmentRepository.Create(newAssessment);
            assessments.Add(newAssessment);
        });
        await assessmentRepository.SaveChangesAsync();

        return assessments;
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
                return HttpError.NotFound($"Choice with id={selectedChoiceId} not found");
            }

            if (!usedQuestionIds.Add(questionId))
            {
                return HttpError.Conflict($"Для вопроса с id={questionId} выбрано несколько вариантов");
            }
        }

        if (usedQuestionIds.Count != questions.Count)
        {
            return HttpError.BadRequest(
                "Не все вопросы имеют выбранный вариант ответа, ids=" +
                string.Join(',', questions.Select(q => q.Id).Except(usedQuestionIds)));
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

    public async Task<Result<AssessmentDto>> EditAssessmentAsync(Guid id, EditAssessmentRequest request, Guid userId)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(id);
        if (assessment == null)
        {
            return HttpError.NotFound("Assessment not found");
        }

        if (request.StartDate.HasValue ^ request.EndDate.HasValue)
        {
            var newStart = request.StartDate ?? assessment.StartDate;
            var newEnd = request.EndDate ?? assessment.EndDate;

            if (newStart > newEnd)
            {
                return HttpError.BadRequest("StartDate must be earlier than EndDate");
            }
        }

        assessment.Edit(request.Name, request.StartDate, request.EndDate, request.UseCircleAssessment, request.UseBehaviorAssessment);

        assessmentRepository.Update(assessment);
        await assessmentRepository.SaveChangesAsync();

        return assessment.ToDtoModel();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(id);

        if (assessment == null)
        {
            return HttpError.NotFound("Assessment not found");
        }

        if (assessment.EndDate <= DateTime.UtcNow)
        {
            return HttpError.Conflict("Cannot delete completed assessment");
        }

        assessmentRepository.Delete(assessment);
        await assessmentRepository.SaveChangesAsync();

        return Result.Success();
    }
}