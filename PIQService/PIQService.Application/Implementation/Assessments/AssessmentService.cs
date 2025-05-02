using Core.Results;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Teams;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
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
        var team = await teamRepository.FindAsync(teamId);
        if (team == null)
        {
            return HttpError.NotFound("Team not found");
        }

        var templateId = team.Project.Direction.Event.TemplateId;
        var template = await templateRepository.FindAsync(templateId);
        if (template == null)
        {
            return HttpError.NotFound("Template not found");
        }

        var newAssessment = new Assessment(Guid.NewGuid(), request.Name, [], template, request.StartDate, request.EndDate);

        await assessmentRepository.CreateAsync(newAssessment, team);
        await assessmentRepository.SaveChangesAsync();

        return newAssessment.ToDtoModel();
    }
}