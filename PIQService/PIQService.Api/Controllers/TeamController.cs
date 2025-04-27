using Core.Results;
using Microsoft.AspNetCore.Mvc;
using PIQService.Application.Implementation.Teams;
using PIQService.Models.Dto;

namespace PIQService.Api.Controllers;

[ApiController]
[Route("teams")]
public class TeamController : ControllerBase
{
    private readonly ITeamService teamService;

    public TeamController(ITeamService teamService)
    {
        this.teamService = teamService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetTutorTeams([FromQuery] Guid tutorId)
    {
        var result = await teamService.GetTeamsByTutorIdAsync(tutorId);
        return result.ToActionResult(this);
    }
}