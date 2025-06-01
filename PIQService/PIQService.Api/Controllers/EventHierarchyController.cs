using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs.ResponseExamples;
using PIQService.Application.Implementation.Hierarchies;
using PIQService.Models.Dto.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[ApiController]
[Authorize]
public class EventHierarchyController(IHierarchyService hierarchyService) : ControllerBase
{
    /// <summary>
    /// Получение иерархии текущего мероприятия
    /// </summary>
    /// <remarks>
    /// Иерархия строится на основе команд, которые доступны текущему пользователю.
    /// Студентам и кураторам - команды, в которых они состоят; руководителю - все команды.
    /// </remarks>
    /// <param name="byTutor">
    /// Необязательный параметр, доступный только кураторам и админам.
    /// Если true - возвращаются команды, где текущий пользователь куратор
    /// </param>
    /// <param name="byTeams">
    /// Необязательный параметр, чтобы получить иерархию включая только переданные id команд
    /// </param>
    [HttpGet("events/current/event-hierarchy")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetHierarchyResponseExample))]
    [ProducesResponseType<GetHierarchyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetHierarchyResponse>> GetHierarchy([FromQuery] bool byTutor = true, [FromQuery] List<Guid>? byTeams = null)
    {
        var result = await hierarchyService.GetHierarchyForEventByUserAsync(null, User.ReadContextUser(), byTutor, byTeams);
        return result.ToActionResult(this);
    }
}