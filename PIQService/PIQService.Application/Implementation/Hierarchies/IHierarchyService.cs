using Core.Auth;
using Core.Results;
using PIQService.Models.Dto.Responses;

namespace PIQService.Application.Implementation.Hierarchies;

public interface IHierarchyService
{
    Task<Result<GetHierarchyResponse>> GetHierarchyForEventByUserAsync(Guid? eventId, ContextUser contextUser, bool onlyWhereTutor = true);

    Task<Result<GetHierarchyResponse>> GetHierarchyForTeamAsync(Guid teamId, Guid? eventId, ContextUser contextUser);
}