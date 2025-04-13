using System.Reflection;
using AccountService;
using AccountService.Data;
using Core.Swagger;
using DotNetEnv;
using Swashbuckle.AspNetCore.Filters;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwaggerGen(options => { options.AddDocs(); })
    .AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.AddAuth();
builder.AddDatabase();
builder.AddServices();

var app = builder.Build();

var dataSeeder = new DataSeeder(app.Services);
await dataSeeder.SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();