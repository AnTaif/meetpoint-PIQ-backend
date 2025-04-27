using System.Reflection;
using Core.Auth;
using Core.Database;
using Core.Swagger;
using DotNetEnv;
using PIQService.Application;
using PIQService.Infra;
using PIQService.Infra.Data;
using Swashbuckle.AspNetCore.Filters;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddSwaggerGen(options =>
    {
        options.AddJwtSecurity();
        options.AddDocs();
    })
    .AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddMySqlDbContext<AppDbContext>(builder.Configuration);

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

var dataSeeder = new DataSeeder(app.Services);
await dataSeeder.SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();