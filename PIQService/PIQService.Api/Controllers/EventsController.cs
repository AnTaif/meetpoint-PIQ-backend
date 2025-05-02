using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Results;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs.ResponseExamples;
using PIQService.Application.Implementation.Events;
using PIQService.Models.Dto.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[Route("events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService eventService;

    public EventsController(IEventService eventService)
    {
        this.eventService = eventService;
    }

    /// <summary>
    /// Получение иерархии текущего мероприятия
    /// </summary>
    /// <remarks>
    /// Иерархия строится на основе команд, которые доступны текущему пользователю.
    /// Куратору - команды, в которых он куратор, руководителю - все команды.
    /// </remarks>
    [HttpGet("current")]
    // [Authorize(AuthenticationSchemes = "Bearer")] TODO: пофиксить авторизацию / мб ходить в аккаунт-сервис валидировать токен
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventHierarchyResponseExample))]
    [ProducesResponseType<GetEventHierarchyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetEventHierarchyResponse>> GetCurrentEvent()
    {
        // var tutorId = GetContextUserId();

        var tutorId = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368"); // TODO: ну а как же без хардкода на проектах ПП

        var result = await eventService.GetEventHierarchyByUserIdAsync(tutorId);
        return result.ToActionResult(this);
    }

    private Guid GetContextUserId()
    {
        var stringId = User.FindFirstValue(JwtRegisteredClaimNames.Sid);

        if (stringId == null)
            throw new Exception($"Failed when reading logged in userId: {stringId}");

        return Guid.Parse(stringId);
    }
}