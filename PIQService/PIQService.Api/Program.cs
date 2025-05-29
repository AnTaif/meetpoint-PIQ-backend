using System.Reflection;
using Core.Auth;
using Core.Database;
using Core.Swagger;
using Core.Logger;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using PIQService.Application;
using PIQService.Infra;
using Swashbuckle.AspNetCore.Filters;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwaggerGen(options =>
    {
        options.AddJwtSecurity();
        options.AddDocs();
    })
    .AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
builder.Services
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddApplicationLayer();
builder.Services.AddInfraLayer(builder.Configuration);

builder.Services.AddHealthChecks();

builder.Host.UseSerilogLogging(builder.Configuration);

var corsOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]?>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (corsOrigins != null)
            policy.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod();
        else
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();
await app.TrySeedDatabaseAsync();

if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();