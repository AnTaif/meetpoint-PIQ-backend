using Core.Results;
using PIQService.Application.Implementation.Assessments.Marks;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Forms;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Scores;

public class ScoreService(
    IEventService eventService,
    IAssessmentMarkRepository markRepository,
    IFormRepository formRepository
)
    : IScoreService
{
    public async Task<Result<List<UserMeanScoreDto>>> GetUsersMeanScoresByFormIdAsync(Guid formId, Guid contextUserId)
    {
        var form = await formRepository.FindAsync(formId);
        if (form == null)
            return StatusError.NotFound("Form not found");

        var hierarchyResult = await eventService.GetEventHierarchyByUserIdAsync(contextUserId);
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

        var questionToCriteriaIds = form.Questions.ToDictionary(q => q.Id, q => q.Criteria.Id);

        var dtos = new List<UserMeanScoreDto>();
        foreach (var userTeamPair in userTeamPairs)
        {
            dtos.Add(await GetUserMeanScoreDto(userTeamPair, questionToCriteriaIds));
        }

        return dtos;
    }

    private async Task<UserMeanScoreDto> GetUserMeanScoreDto((UserDto User, TeamDto Team) userTeamPair,
        Dictionary<Guid, Guid> questionToCriteriaIds)
    {
        var marks = await markRepository.SelectByAssessedUserIdAsync(userTeamPair.User.Id);

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

    private static double GetMeanValue(IReadOnlyCollection<int> values) => (double)values.Sum() / values.Count;
}