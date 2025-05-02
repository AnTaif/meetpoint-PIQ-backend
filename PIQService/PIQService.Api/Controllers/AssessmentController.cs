using Core.Results;
using Microsoft.AspNetCore.Mvc;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Models.Dto;

namespace PIQService.Api.Controllers;

[ApiController]
[Route("teams/{teamId}/assessments")]
public class AssessmentController : ControllerBase
{
    private readonly IAssessmentService assessmentService;

    public AssessmentController(IAssessmentService assessmentService)
    {
        this.assessmentService = assessmentService;
    }

    [HttpPost]
    public async Task<ActionResult<AssessmentDto>> CreateForTeam(Guid teamId, CreateTeamAssessmentRequest request)
    {
        var result = await assessmentService.CreateTeamAssessmentAsync(teamId, request);

        return result.ToActionResult(this);
    }
}