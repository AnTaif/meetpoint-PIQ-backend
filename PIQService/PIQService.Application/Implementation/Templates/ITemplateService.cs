using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Templates;

public interface ITemplateService
{
    Task<Result<List<FormWithCriteriaDto>>> GetFormsWithCriteriaAsync(Guid? eventId = null);
}