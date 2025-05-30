using Core.Auth;
using Core.Results;
using Microsoft.Extensions.Caching.Hybrid;
using PIQService.Application.Implementation.Assessments.Marks;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Forms;
using PIQService.Application.Implementation.Teams;
using PIQService.Application.Implementation.Templates;
using PIQService.Application.Implementation.Users;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Scores;

[RegisterScoped]
public class ScoreService(
    HybridCache cache,
    ITeamRepository teamRepository,
    IUserRepository userRepository,
    IEventService eventService,
    IEventRepository eventRepository,
    ITemplateRepository templateRepository,
    IAssessmentMarkRepository markRepository,
    IFormRepository formRepository
)
    : IScoreService
{
    public async Task<Result<UserMeanScoreDto>> GetUserMeanScoresAsync(Guid userId, ContextUser contextUser, Guid? byAssessment)
    {
        var user = await userRepository.FindAsync(userId);

        if (user == null)
            return StatusError.NotFound("User not found");

        var activeEvents = await eventRepository.SelectActiveAsync(DateTime.UtcNow);
        var currentEvent = activeEvents.FirstOrDefault();

        if (currentEvent == null)
            return StatusError.NotFound("Event not found");

        var template = await templateRepository.FindAsync(currentEvent.TemplateId);

        if (template == null)
            return StatusError.NotFound("Template not found");

        var forms = new List<Form>();
        
        var circleForm = await cache.GetOrCreateAsync(
            $"forms_{template.CircleFormId}",
            async _ => await formRepository.FindAsync(template.CircleFormId)
        );
        
        if (circleForm != null)
            forms.Add(circleForm);

        var behaviorForm = await cache.GetOrCreateAsync(
            $"forms_{template.BehaviorFormId}",
            async _ => await formRepository.FindAsync(template.BehaviorFormId)
        );
        if (behaviorForm != null)
            forms.Add(behaviorForm);

        if (user.TeamId == null)
            return StatusError.BadRequest("Этот пользователь не состоит ни в одной команде, у него не может быть результатов оцениваний");

        var team = await teamRepository.FindWithoutDepsAsync(user.TeamId.Value);

        if (team == null)
            return StatusError.NotFound("Team not found");

        return await GetUserMeanScoreDtoAsync(
            (new UserDto { Id = user.Id, FullName = user.GetFullName(), }, new TeamDto { Id = team.Id, Name = team.Name, }),
            forms, byAssessment
        );
    }

    public async Task<Result<List<UserMeanScoreDto>>> GetTeamMeanScoresAsync(Guid teamId, ContextUser contextUser, Guid? byAssessment)
    {
        var team = await teamRepository.FindAsync(teamId);

        if (team == null)
            return StatusError.NotFound("Team not found");

        if (!(contextUser.Roles.Contains(RolesConstants.Admin) ||
              (contextUser.Roles.Contains(RolesConstants.Tutor) && team.TutorId == contextUser.Id) ||
              team.Users.Any(u => u.Id == contextUser.Id)))
        {
            return StatusError.Forbidden("Вы не можете просматривать результаты оцениваний данной команды");
        }

        var activeEvents = await eventRepository.SelectActiveAsync(DateTime.UtcNow);
        var currentEvent = activeEvents.FirstOrDefault();

        if (currentEvent == null)
            return StatusError.NotFound("Event not found");

        var template = await templateRepository.FindAsync(currentEvent.TemplateId);

        if (template == null)
            return StatusError.NotFound("Template not found");

        var forms = new List<Form>();
        var circleForm = await cache.GetOrCreateAsync(
            $"forms_{template.CircleFormId}",
            async _ => await formRepository.FindAsync(template.CircleFormId)
        );
        if (circleForm != null)
            forms.Add(circleForm);

        var behaviorForm = await cache.GetOrCreateAsync(
            $"forms_{template.BehaviorFormId}",
            async _ => await formRepository.FindAsync(template.BehaviorFormId)
        );
        if (behaviorForm != null)
            forms.Add(behaviorForm);

        var userTeamPairs = team.Users.Select(u =>
            (
                new UserDto { Id = u.Id, FullName = u.GetFullName() }, new TeamDto { Id = team.Id, Name = team.Name }
            )
        ).ToList();

        var dtos = new List<UserMeanScoreDto>();
        foreach (var userTeamPair in userTeamPairs)
        {
            dtos.Add(await GetUserMeanScoreDtoAsync(userTeamPair, forms, byAssessment));
        }

        return dtos;
    }

    public async Task<Result<List<UserMeanScoreDto>>> GetUsersMeanScoresByFormIdAsync(Guid formId, ContextUser contextUser,
        bool onlyWhereTutor = true)
    {
        var form = await formRepository.FindAsync(formId);
        if (form == null)
            return StatusError.NotFound("Form not found");

        var hierarchyResult = await eventService.GetEventHierarchyForUserAsync(contextUser, onlyWhereTutor: onlyWhereTutor);
        if (hierarchyResult.IsFailure)
            return hierarchyResult.Error;

        var hierarchy = hierarchyResult.Value;

        var teams = hierarchy.Event.Directions
            .SelectMany(d => d.Projects)
            .SelectMany(p => p.Teams)
            .ToList();

        var userTeamPairs = teams
            .SelectMany(team => team.Members.Select(member => (User: member, Team: team))
            )
            .ToList();

        var dtos = new List<UserMeanScoreDto>();
        foreach (var userTeamPair in userTeamPairs)
        {
            dtos.Add(await GetUserMeanScoreDtoAsync(userTeamPair, [form]));
        }

        return dtos;
    }

    private async Task<UserMeanScoreDto> GetUserMeanScoreDtoAsync((UserDto User, TeamDto Team) userTeamPair,
        IEnumerable<Form> forms, Guid? byAssessment = null)
    {
        var questionToCriteriaIds = GetQuestionToCriteriaIds(forms);

        var marks = await markRepository.SelectByAssessedUserIdAsync(userTeamPair.User.Id, byAssessment);

        var criteriaToValues = new Dictionary<Guid, List<int>>();
        foreach (var mark in marks)
        {
            var choices = mark.Choices;

            foreach (var choice in choices)
            {
                if (!questionToCriteriaIds.TryGetValue(choice.QuestionId, out var criteriaId))
                {
                    continue;
                }

                if (criteriaToValues.TryGetValue(criteriaId, out var value))
                {
                    value.Add(choice.Value);
                }
                else
                {
                    criteriaToValues.Add(criteriaId, [choice.Value]);
                }
            }
        }

        var criteriaToMeanValue = criteriaToValues
            .ToDictionary(pair => pair.Key, pair => GetMeanValue(pair.Value));

        return new UserMeanScoreDto
        {
            UserId = userTeamPair.User.Id,
            FullName = userTeamPair.User.FullName,
            TeamId = userTeamPair.Team.Id,
            TeamName = userTeamPair.Team.Name,
            ScoreByCriteriaIds = criteriaToMeanValue,
        };
    }

    private Dictionary<Guid, Guid> GetQuestionToCriteriaIds(IEnumerable<Form> forms)
    {
        return forms
            .SelectMany(f => f.Questions)
            .ToDictionary(q => q.Id, q => q.Criteria.Id);
    }

    private static double GetMeanValue(IReadOnlyCollection<int> values) => (double)values.Sum() / values.Count;
}