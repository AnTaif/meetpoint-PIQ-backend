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

    [HttpGet("current")]
    // [Authorize(AuthenticationSchemes = "Bearer")] TODO: пофиксить авторизацию / мб ходить в аккаунт-сервис валидировать токен
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventWithIncludesResponseExample))]
    public async Task<ActionResult<GetEventWithIncludesResponse>> GetCurrentEvent()
    {
        // var tutorId = GetContextUserId();

        var tutorId = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368"); // TODO: ну а как же без хардкода на проектах ПП

        var result = await eventService.GetEventWithIncludesByUserIdAsync(tutorId);
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