using Microsoft.Extensions.DependencyInjection;

namespace PIQService.Application;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddPIQServiceApplication();
    }
}