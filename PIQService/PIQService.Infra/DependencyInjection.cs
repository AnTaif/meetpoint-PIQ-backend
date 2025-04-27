using Microsoft.Extensions.DependencyInjection;
using PIQService.Application.Implementation.Teams;
using PIQService.Infra.Data.Repositories;

namespace PIQService.Infra;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITeamRepository, TeamRepository>();
    }
}