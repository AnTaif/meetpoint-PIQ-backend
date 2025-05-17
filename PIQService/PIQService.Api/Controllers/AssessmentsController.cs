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
    IAssessmentFormsService assessmentFormsService,
    IAssessmentService assessmentService
)
    : ControllerBase
{
    /// <summary>
    /// Редактирование существующего оценивания
    /// </summary>
    /// <remarks>
    /// Можно изменить только активные/предстоящие оценивания.
    /// Редактирование вариантов оценивания возможно только для предстоящих оцениваний.
    /// </remarks>
    [HttpPut("{id}")]
    [SwaggerRequestExample(typeof(EditAssessmentRequest), typeof(EditAssessmentRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AssessmentDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AssessmentDto>> EditAssessment(Guid id, EditAssessmentRequest request)
    {
        var result = await assessmentService.EditAssessmentAsync(id, request, User.ReadSid());
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
    [ProducesResponseType<string>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteAssessment(Guid id)
    {
        var result = await assessmentService.DeleteAsync(id);
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
        var result = await assessmentService.SelectUsersToAssessAsync(User.ReadSid(), assessmentId);
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
        var result = await assessmentService.SelectAssessChoicesAsync(assessmentId, User.ReadSid(), assessedUserId);
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
    public async Task<ActionResult<AssessmentMarkDto>> AssessUser(Guid assessmentId, Guid assessedUserId,
        IEnumerable<Guid> selectedChoiceIds)
    {
        var result = await assessmentService.AssessUserAsync(assessmentId, User.ReadSid(), assessedUserId, selectedChoiceIds);
        return result.ToActionResult(this, value => CreatedAtAction("AssessUser", value));
    }
}