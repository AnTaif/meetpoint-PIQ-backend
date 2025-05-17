using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs;
using PIQService.Api.Docs.RequestExamples;
using PIQService.Api.Docs.ResponseExamples;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Events;
using PIQService.Models.Dto;
using PIQService.Models.Dto.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[ApiController]
[Authorize]
[Route("events")]
public class EventsController(
    IAssessmentService assessmentService,
    IEventService eventService
)
    : ControllerBase
{
    /// <summary>
    /// Получение иерархии текущего мероприятия
    /// </summary>
    /// <remarks>
    /// Иерархия строится на основе команд, которые доступны текущему пользователю.
    /// Куратору - команды, в которых он куратор, руководителю - все команды.
    /// </remarks>
    [HttpGet("current")]
    [Authorize]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventHierarchyResponseExample))]
    [ProducesResponseType<GetEventHierarchyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetEventHierarchyResponse>> GetCurrentEvent()
    {
        var result = await eventService.GetEventHierarchyByUserIdAsync(User.ReadSid());
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Создание нового оценивания для нескольких команд
    /// </summary>
    [HttpPost("assessments")]
    [SwaggerRequestExample(typeof(CreateTeamsAssessmentRequest), typeof(CreateTeamsAssessmentRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(EnumerableAssessmentDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<AssessmentDto>>> CreateNewAssessment(CreateTeamsAssessmentRequest request)
    {
        var result = await assessmentService.CreateTeamsAssessmentAsync(request);

        return result.ToActionResult(this, dto => CreatedAtAction("CreateNewAssessment", dto));
    }
}