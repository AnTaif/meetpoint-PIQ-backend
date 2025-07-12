using PIQService.Application.Implementation.Assessments.Sessions.Requests;
using PIQService.Application.Implementation.Events;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Domain.Assessments;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments.Sessions;

public interface IAssessmentCreator
{
    Task<AssessmentDto> CreateSingleForTeamAsync(CreateAssessmentForTeamRequest request, Guid teamId);
    Task<List<AssessmentDto>> CreateManyForTeamsAsync(CreateAssessmentsForTeamsRequest request);
}

[RegisterScoped]
public class AssessmentCreator(
    IEventService eventService,
    IAssessmentRepository assessmentRepository
)
    : IAssessmentCreator
{
    public async Task<AssessmentDto> CreateSingleForTeamAsync(CreateAssessmentForTeamRequest request, Guid teamId)
    {
        var dtos = await CreateAssessmentsForTeamsAsync(request, [teamId]);
        return dtos.Single();
    }

    public async Task<List<AssessmentDto>> CreateManyForTeamsAsync(CreateAssessmentsForTeamsRequest request)
    {
        var dtos = await CreateAssessmentsForTeamsAsync(request, request.TeamIds);
        return dtos;
    }

    private async Task<List<AssessmentDto>> CreateAssessmentsForTeamsAsync(
        CreateAssessmentRequestBase request, IReadOnlyCollection<Guid> teamIds)
    {
        var currentEvent = await eventService.FindEventWithoutDepsAsync(null)
                           ?? throw new Exception("Current event not found");

        var dtos = new List<AssessmentDto>();
        foreach (var teamId in teamIds)
        {
            var newAssessment = new AssessmentWithoutDeps(
                Guid.NewGuid(),
                request.Name,
                request.StartDate,
                request.EndDate,
                request.UseCircleAssessment,
                request.UseBehaviorAssessment,
                currentEvent.TemplateId,
                teamId
            );

            assessmentRepository.Create(newAssessment);
            dtos.Add(newAssessment.ToDtoModel(-1, -1));
        }

        await assessmentRepository.SaveChangesAsync();
        return dtos;
    }
}