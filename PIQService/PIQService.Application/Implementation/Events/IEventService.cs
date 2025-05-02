using Core.Results;
using PIQService.Models.Dto.Responses;

namespace PIQService.Application.Implementation.Events;

public interface IEventService
{
    Task<Result<GetEventHierarchyResponse>> GetEventHierarchyByUserIdAsync(Guid userId, Guid? eventId = null);
}