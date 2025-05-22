using Microsoft.Extensions.DependencyInjection;
using PIQService.Application.Implementation.Assessments;
using PIQService.Application.Implementation.Events;
using PIQService.Application.Implementation.Templates;

namespace PIQService.Application;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IAssessmentService, AssessmentService>();
        services.AddScoped<IAssessmentFormsService, AssessmentFormsService>();
        services.AddScoped<ITemplateService, TemplateService>();
    }
}