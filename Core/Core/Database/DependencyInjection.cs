using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Database;

public static class DependencyInjection
{
    public static void AddMySqlDbContext<TContext>(this IServiceCollection services, ConfigurationManager configuration)
        where TContext : DbContext
    {
        var dbOptions = new DatabaseOptions();
        configuration.GetSection("DatabaseOptions").Bind(dbOptions);
        dbOptions.Host = Environment.GetEnvironmentVariable("DB_CONTAINER") ?? "localhost";
        dbOptions.Port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "3306";
        dbOptions.User = Environment.GetEnvironmentVariable("DATABASE_USER")!;
        dbOptions.Password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD")!;

        services.Configure<DatabaseOptions>(options =>
        {
            options.Name = dbOptions.Name;
            options.Version = dbOptions.Version;
            options.Host = dbOptions.Host;
            options.Port = dbOptions.Port;
            options.User = dbOptions.User;
            options.Password = dbOptions.Password;
        });

        services.AddDbContext<TContext>(options =>
        {
            options.UseMySql(dbOptions.GetConnectionString(), new MySqlServerVersion(dbOptions.Version));
        });
    }

    public static void AddDataSeeder<TDataSeeder>(this IServiceCollection services)
        where TDataSeeder : class, IDataSeeder
    {
        services.AddTransient<IDataSeeder, TDataSeeder>();
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await dataSeeder.SeedAsync();
    }
}