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
[Route("assessments")]
public class AssessmentsController : ControllerBase
{
    private readonly IAssessmentFormsService assessmentFormsService;
    private readonly IAssessmentService assessmentService;

    public AssessmentsController(
        IAssessmentFormsService assessmentFormsService,
        IAssessmentService assessmentService
    )
    {
        this.assessmentFormsService = assessmentFormsService;
        this.assessmentService = assessmentService;
    }

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
        var result = await assessmentService.EditAssessmentAsync(id, request, User.GetSid());
        return result.ToActionResult(this);
    }

    [HttpGet("{id}/used-forms")]
    public async Task<ActionResult<IEnumerable<FormShortDto>>> GetAssessmentUsedForms(Guid id)
    {
        var result = await assessmentFormsService.GetAssessmentUsedFormsAsync(id);
        return result.ToActionResult(this);
    }
}