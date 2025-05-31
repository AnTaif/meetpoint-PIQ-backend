using Core.Database;
using DotNetEnv;
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

        services.AddRedis();
    }

    private static void AddRedis(this IServiceCollection services)
    {
        var aboba = Env.GetString("REDIS_HOST", "localhost");
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Env.GetString("REDIS_HOST", "localhost");
    
            var port = Env.GetString("REDIS_PORT");
            if (port != null)
            {
                options.Configuration += $":{port}";
            }
    
            var password = Env.GetString("REDIS_PASSWORD");
            if (!string.IsNullOrEmpty(password))
            {
                options.Configuration += $",password={password}";
            }
    
            options.InstanceName = Env.GetString("REDIS_INSTANCE_NAME", "PIQCache_");
        });
    }
}