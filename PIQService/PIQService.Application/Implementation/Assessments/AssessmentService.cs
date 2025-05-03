using Core.Results;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Teams;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public class AssessmentService : IAssessmentService
{
    private readonly ITeamRepository teamRepository;
    private readonly ITemplateRepository templateRepository;
    private readonly IAssessmentRepository assessmentRepository;

    public AssessmentService(
        ITeamRepository teamRepository,
        ITemplateRepository templateRepository,
        IAssessmentRepository assessmentRepository
    )
    {
        this.teamRepository = teamRepository;
        this.templateRepository = templateRepository;
        this.assessmentRepository = assessmentRepository;
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

    public async Task<Result<AssessmentDto>> CreateTeamAssessmentAsync(Guid teamId, CreateTeamAssessmentRequest request)
    {
        var newAssessmentResult = await CreateAssessmentForTeamsAsync([teamId], request.Name, request.StartDate, request.EndDate);
        return newAssessmentResult.IsFailure ? newAssessmentResult.Error : newAssessmentResult.Value.ToDtoModel();
    }

    public async Task<Result<AssessmentDto>> CreateTeamsAssessmentAsync(CreateTeamsAssessmentRequest request)
    {
        var newAssessmentResult = await CreateAssessmentForTeamsAsync(request.TeamIds, request.Name, request.StartDate, request.EndDate);
        return newAssessmentResult.IsFailure ? newAssessmentResult.Error : newAssessmentResult.Value.ToDtoModel();
    }

    private async Task<Result<Assessment>> CreateAssessmentForTeamsAsync(
        IReadOnlyCollection<Guid> teamIds, string name, DateTime startDate, DateTime endDate)
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

        var newAssessment = new Assessment(Guid.NewGuid(), name, [], template, startDate, endDate);

        await assessmentRepository.CreateAsync(newAssessment, teams.ToArray());
        await assessmentRepository.SaveChangesAsync();

        return newAssessment;
    }
}