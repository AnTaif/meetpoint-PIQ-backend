using Core.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Assessments.Marks;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Teams;
using PIQService.Application.Implementation.Templates;
using PIQService.Infra.Data;
using PIQService.Infra.Data.Repositories;
using PIQService.Infra.Data.Seeding;

namespace PIQService.Infra;

public static class DependencyInjection
{
    public static void AddInfraLayer(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddMySqlDbContext<AppDbContext>(config);
        
        services.AddTransient<ITemplateSeedingHelper, TemplateSeedingHelper>();
        services.AddDataSeeder<DataSeeder>();
        
        services.AddRepositories();
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IAssessmentRepository, AssessmentRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<IAssessmentMarkRepository, AssessmentMarkRepository>();
    }
}