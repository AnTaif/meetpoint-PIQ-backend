using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs;
using PIQService.Api.Docs.RequestExamples;
using PIQService.Api.Docs.ResponseExamples;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Assessments.Requests;
using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[ApiController]
[Authorize]
[Route("assessments")]
public class AssessmentsController(
    IAssessmentScoringService assessmentScoringService,
    IAssessmentFormsService assessmentFormsService,
    IAssessmentService assessmentService
)
    : ControllerBase
{
    /// <summary>
    /// Получение оценивания по id
    /// </summary>
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AssessmentDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssessmentDto>> GetAssessment(Guid id)
    {
        var result = await assessmentService.GetAssessmentAsync(id, User.ReadContextUser());
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Редактирование существующего оценивания
    /// </summary>
    /// <remarks>
    /// Можно изменить только активные/предстоящие оценивания.
    /// Редактирование вариантов оценивания возможно только для предстоящих оцениваний.
    /// </remarks>
    [HttpPut("{id}")]
    [Authorize(Roles = RolesConstants.AdminTutor)]
    [SwaggerRequestExample(typeof(EditAssessmentRequest), typeof(EditAssessmentRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AssessmentDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssessmentDto>> EditAssessment(Guid id, EditAssessmentRequest request)
    {
        var result = await assessmentService.EditAssessmentAsync(id, request, User.ReadContextUser());
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Получение выбранных в оценивании форм из текущего шаблона
    /// </summary>
    [HttpGet("{id}/used-forms")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(FormShortDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<FormShortDto>>> GetAssessmentUsedForms(Guid id)
    {
        var result = await assessmentFormsService.GetAssessmentUsedFormsAsync(id);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Удаление существующего оценивание
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = RolesConstants.AdminTutor)]
    [ProducesResponseType<string>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteAssessment(Guid id)
    {
        var result = await assessmentService.DeleteAsync(id, User.ReadContextUser());
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Получение пользователей для оценивания
    /// </summary>
    [HttpGet("{assessmentId}/assess-users")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EnumerableAssessUserDtoExample))]
    [ProducesResponseType<IEnumerable<AssessUserDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<AssessUserDto>>> GetAssessUsers(Guid assessmentId)
    {
        var result = await assessmentScoringService.GetUsersToScoreAsync(assessmentId, User.ReadContextUser());
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Получение выбранных вариантов оценивания для пользователя
    /// </summary>
    /// <param name="assessmentId"></param>
    /// <param name="assessedUserId"></param>
    /// <returns></returns>
    [HttpGet("{assessmentId}/assess-users/{assessedUserId}/choices")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EnumerableAssessChoiceDtoExample))]
    [ProducesResponseType<IEnumerable<AssessChoiceDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<AssessChoiceDto>>> GetChoicesForAssessedUser(Guid assessmentId, Guid assessedUserId)
    {
        var result = await assessmentScoringService.GetChoicesAsync(assessmentId, assessedUserId, User.ReadContextUser());
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Оценка пользователя по выбранным вариантам из формы
    /// </summary>
    [HttpPost("{assessmentId}/assess-users/{assessedUserId}/assess")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AssessmentMarkDtoExample))]
    [ProducesResponseType<AssessmentMarkDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssessmentMarkDto>> AssessUser(Guid assessmentId, Guid assessedUserId, IReadOnlyCollection<Guid> choiceIds)
    {
        var result = await assessmentScoringService.ScoreAsync(assessmentId, assessedUserId, User.ReadContextUser(), choiceIds);
        return result.ToActionResult(this, value => CreatedAtAction("AssessUser", value));
    }
}