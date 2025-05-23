using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs;
using PIQService.Api.Docs.RequestExamples;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[ApiController]
[Authorize]
[Route("teams")]
public class TeamsController(
    IAssessmentService assessmentService
)
    : ControllerBase
{
    /// <summary>
    /// Получение оцениваний команды
    /// </summary>
    [HttpGet("{teamId}/assessments")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EnumerableAssessmentDtoExample))]
    [ProducesResponseType<IEnumerable<AssessmentDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AssessmentDto>>> GetTeamAssessments(Guid teamId)
    {
        var result = await assessmentService.GetTeamAssessments(teamId);

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Создание оценивания для команды
    /// </summary>
    [HttpPost("{teamId}/assessments")]
    [Authorize(Roles = RolesConstants.AdminTutor)]
    [SwaggerRequestExample(typeof(CreateTeamAssessmentRequest), typeof(CreateTeamAssessmentRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(AssessmentDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssessmentDto>> CreateForTeam(Guid teamId, CreateTeamAssessmentRequest request)
    {
        var result = await assessmentService.CreateTeamAssessmentAsync(teamId, request, User.ReadContextUser());

        return result.ToActionResult(this, dto => CreatedAtAction("CreateForTeam", dto));
    }
}