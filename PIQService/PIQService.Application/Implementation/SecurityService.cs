using Core.Auth;
using PIQService.Application.Implementation.EventSupporting.Teams;

namespace PIQService.Application.Implementation;

public interface ISecurityService
{
    Task<bool> IsAdminOrTeamsTutorAsync(ContextUser user, Guid teamId);
    Task<bool> IsAdminOrTeamsTutorAsync(ContextUser user, IReadOnlyCollection<Guid> teamIds);
    Task<bool> IsTeamsTutorAsync(ContextUser user, Guid teamId);
    Task<bool> IsTeamsTutorAsync(ContextUser user, IReadOnlyCollection<Guid> teamIds);
}

[RegisterScoped]
public class SecurityService(
    ITeamRepository teamRepository
)
    : ISecurityService
{
    public async Task<bool> IsAdminOrTeamsTutorAsync(ContextUser user, Guid teamId)
    {
        return user.Roles.Contains(RolesConstants.Admin) || await IsTeamsTutorAsync(user, teamId);
    }

    public async Task<bool> IsAdminOrTeamsTutorAsync(ContextUser user, IReadOnlyCollection<Guid> teamIds)
    {
        return user.Roles.Contains(RolesConstants.Admin) || await IsTeamsTutorAsync(user, teamIds);
    }

    public async Task<bool> IsTeamsTutorAsync(ContextUser user, Guid teamId)
    {
        var team = await teamRepository.FindWithoutDepsAsync(teamId) ?? throw new Exception("Error when finding assessment's team");
        return team.TutorId == user.Id;
    }

    public async Task<bool> IsTeamsTutorAsync(ContextUser user, IReadOnlyCollection<Guid> teamIds)
    {
        var teams = await teamRepository.SelectWithoutDepsAsync(teamIds);
        return teams.All(t => t.TutorId == user.Id);
    }
}