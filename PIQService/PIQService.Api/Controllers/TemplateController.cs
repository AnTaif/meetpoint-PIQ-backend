using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PIQService.Api.Docs.ResponseExamples;
using PIQService.Application.Implementation.Forms;
using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Controllers;

[ApiController]
[Route("templates")]
[Authorize]
public class TemplateController(IFormService formService) : ControllerBase
{
    [HttpGet("current/forms-with-criteria")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EnumerableFormWithCriteriaDtoExample))]
    [ProducesResponseType<AssessmentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<FormWithCriteriaDto>>> GetFormsWithCriteria()
    {
        var result = await formService.GetFormsWithCriteriaAsync(null);
        return result.ToActionResult(this);
    }
}