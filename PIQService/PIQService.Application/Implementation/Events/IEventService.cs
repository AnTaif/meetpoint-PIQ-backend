using Core.Auth;
using Core.Results;
using PIQService.Models.Domain;
using PIQService.Models.Dto.Responses;

namespace PIQService.Application.Implementation.Events;

public interface IEventService
{
    Task<Result<Event?>> FindCurrentEventAsync();

    /// <param name="onlyWhereTutor">Параметр для админов-тьюторов, если false - можно получить всю иерархию</param>
    Task<Result<GetEventHierarchyResponse>> GetEventHierarchyForUserAsync(
        ContextUser contextUser, Guid? eventId = null, bool onlyWhereTutor = true);
}