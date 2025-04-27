using Microsoft.Extensions.DependencyInjection;
using PIQService.Application.Implementation.Teams;

namespace PIQService.Application;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITeamService, TeamService>();
    }
}