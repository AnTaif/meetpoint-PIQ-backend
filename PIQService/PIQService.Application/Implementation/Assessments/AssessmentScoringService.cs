using Core.Auth;
using Core.Results;
using Microsoft.Extensions.Logging;
using PIQService.Application.Implementation.Assessments.Marks;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Converters;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

[RegisterScoped]
public class AssessmentScoringService(
    IAssessmentMarkRepository markRepository,
    IAssessmentRepository assessmentRepository,
    ITeamRepository teamRepository,
    IAssessmentFormsService assessmentFormsService,
    ILogger<AssessmentScoringService> logger
)
    : IAssessmentScoringService
{
    public async Task<Result<AssessmentMarkDto>> ScoreAsync(
        Guid assessmentId, Guid assessedUserId, ContextUser assessor, IReadOnlyCollection<Guid> choiceIds)
    {
        var usedFormsResult = await assessmentFormsService.GetAssessmentUsedFormsAsync(assessmentId);

        if (usedFormsResult.IsFailure)
            return usedFormsResult.Error;

        var questions = usedFormsResult.Value.SelectMany(f => f.Questions).ToList();

        var choiceToQuestion = questions
            .SelectMany(q => q.Choices.Select(c => new { ChoiceId = c.Id, QuestionId = q.Id }))
            .ToDictionary(x => x.ChoiceId, x => x.QuestionId);

        var usedQuestionIds = new HashSet<Guid>();

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

        var mark = await markRepository.FindWithoutDepsAsync(assessmentId, assessor.Id, assessedUserId);
        var newChoices = choiceIds.Select(id => new Choice(id)).ToList();

        if (mark == null)
        {
            mark = new AssessmentMarkWithoutDeps(Guid.NewGuid(), assessor.Id, assessedUserId, assessmentId, newChoices);
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

    public async Task<Result<List<AssessUserDto>>> GetUsersToScoreAsync(Guid assessmentId, ContextUser contextUser)
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

        var assessedUsers = await markRepository.SelectAssessedUsersAsync(contextUser.Id, assessmentId);
        var assessedUsersById = assessedUsers.ToLookup(u => u.Id);

        var usersToAssess = team.Users
            .OrderBy(u => u.LastName)
            //.Concat([team.Tutor]) //TODO: подумать над тем, должны ли студенты оценивать куратора по тем же вопросам
            .Where(u => u.Id != contextUser.Id)
            .ToList();

        return usersToAssess.Select(u => new AssessUserDto
        {
            User = u.ToDtoModel(),
            Assessed = assessedUsersById.Contains(u.Id),
        }).ToList();
    }

    public async Task<Result<List<AssessChoiceDto>>> GetChoicesAsync(Guid assessmentId, Guid assessedId, ContextUser assessor)
    {
        var assessment = await assessmentRepository.FindWithoutDepsAsync(assessmentId);

        if (assessment == null)
            return StatusError.NotFound("Assessment not found");

        var mark = await markRepository.FindWithoutDepsAsync(assessmentId, assessor.Id, assessedId);

        if (mark == null)
            return new List<AssessChoiceDto>();

        return mark.Choices
            .Select(c => new AssessChoiceDto() { QuestionId = c.QuestionId, ChoiceId = c.Id })
            .ToList();
    }
}