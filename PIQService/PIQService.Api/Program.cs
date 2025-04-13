using System.Reflection;
using Core.Swagger;
using DotNetEnv;
using PIQService.Api;
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

builder.AddAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();