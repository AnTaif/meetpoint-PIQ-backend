using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Forms;

public interface IFormService
{
    Task<Result<List<FormWithCriteriaDto>>> GetFormsWithCriteriaAsync(Guid? eventId);
}