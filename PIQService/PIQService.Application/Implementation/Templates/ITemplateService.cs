using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Templates;

public interface ITemplateService
{
    Task<Result<IReadOnlyCollection<FormWithCriteriaDto>>> GetFormsWithCriteriaAsync(Guid? eventId = null);
}