using Core.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PIQService.Infra.Data;
using PIQService.Infra.Data.Seeding;

namespace PIQService.Infra;

public static class DependencyInjection
{
    public static void AddInfraLayer(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddMySqlDbContext<AppDbContext>(config);
        
        services.AddDataSeeder<DataSeeder>();

        services.AddPIQServiceInfra();
    }
}