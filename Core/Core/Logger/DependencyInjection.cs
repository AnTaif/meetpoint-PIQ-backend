using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Core.Logger;

public static class DependencyInjection
{
    public static void UseSerilogLogging(this ConfigureHostBuilder hostBuilder, ConfigurationManager config)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .Enrich.FromLogContext()
            .Enrich.With(new RemovePropertiesEnricher())
            .CreateLogger();

        hostBuilder.UseSerilog();
    }
}