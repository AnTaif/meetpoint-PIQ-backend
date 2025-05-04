using Core.Results;
using PIQService.Models.Dto;

namespace PIQService.Application.Implementation.Assessments;

public interface IAssessmentFormsService
{
    Task<Result<IEnumerable<FormShortDto>>> GetAssessmentUsedFormsAsync(Guid assessmentId);
}