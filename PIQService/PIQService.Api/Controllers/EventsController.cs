using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs;
using PIQService.Api.Docs.RequestExamples;
using PIQService.Api.Docs.ResponseExamples;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Application.Implementation.Hierarchies;
using PIQService.Models.Dto;
using PIQService.Models.Dto.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[ApiController]
[Authorize]
[Route("events")]
public class EventsController(
    IAssessmentCreationService assessmentCreationService,
    IAssessmentService assessmentService,
    IHierarchyService hierarchyService
)
    : ControllerBase
{
    /// <summary>
    /// Получение иерархии текущего мероприятия
    /// </summary>
    /// <remarks>
    /// Иерархия строится на основе команд, которые доступны текущему пользователю.
    /// Студентам и кураторам - команды, в которых они состоят; руководителю - все команды.
    /// </remarks>
    /// <param name="onlyWhereTutor">
    /// Необязательный параметр, доступный только кураторам и админам.
    /// Если true - возвращаются команды, где текущий пользователь куратор
    /// </param>
    [Obsolete("Пересесть на GET events/current/event-hierarchy")]
    [HttpGet("current")]
    [Authorize]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetHierarchyResponseExample))]
    [ProducesResponseType<GetHierarchyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetHierarchyResponse>> GetCurrentEvent([FromQuery] bool onlyWhereTutor = true)
    {
        var result = await hierarchyService.GetHierarchyForEventByUserAsync(null, User.ReadContextUser(), onlyWhereTutor: onlyWhereTutor);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Создание нового оценивания для нескольких команд
    /// </summary>
    [HttpPost("assessments")]
    [Authorize(Roles = RolesConstants.AdminTutor)]
    [SwaggerRequestExample(typeof(CreateTeamsAssessmentRequest), typeof(CreateTeamsAssessmentRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(EnumerableAssessmentDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<AssessmentDto>>> CreateNewAssessment(CreateTeamsAssessmentRequest request)
    {
        var result = await assessmentCreationService.CreateAssessmentsForTeamsAsync(request, User.ReadContextUser());

        return result.ToActionResult(this, dto => CreatedAtAction("CreateNewAssessment", dto));
    }
}