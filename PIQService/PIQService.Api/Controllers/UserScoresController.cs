using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs.ResponseExamples;
using PIQService.Application.Implementation.Scores;
using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[ApiController]
[Authorize]
[Route("user-scores")]
public class UserScoresController(IScoreService scoreService) : ControllerBase
{
    [HttpGet("{userId}/criteria-mean")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserMeanScoreDtoExample))]
    [ProducesResponseType<UserMeanScoreDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserMeanScoreDto>> GetUserMeanScoresByForm(
        Guid userId, [FromQuery] Guid? byAssessment = null)
    {
        var result = await scoreService.GetUserMeanScoresAsync(userId, User.ReadContextUser(), byAssessment);
        return result.ToActionResult(this);
    }
    
    [HttpGet("forms/{formId}/criteria-mean")]
    [Authorize(Roles = RolesConstants.AdminTutor)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EnumerableUserMeanScoreDtoExample))]
    [ProducesResponseType<List<UserMeanScoreDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<UserMeanScoreDto>>> GetUsersMeanScoresByForm(
        Guid formId, [FromQuery] bool onlyWhereTutor = true)
    {
        var result = await scoreService.GetUsersMeanScoresByFormIdAsync(formId, User.ReadContextUser(), onlyWhereTutor);
        return result.ToActionResult(this);
    }
}