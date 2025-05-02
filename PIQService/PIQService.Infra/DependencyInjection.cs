using Microsoft.Extensions.DependencyInjection;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Teams;
using PIQService.Application.Implementation.Templates;
using PIQService.Infra.Data.Repositories;

namespace PIQService.Infra;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IAssessmentRepository, AssessmentRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
    }
}