using System.Reflection;
using AccountService;
using AccountService.Data;
using Core.Auth;
using Core.Database;
using Core.Swagger;
using DotNetEnv;
using Swashbuckle.AspNetCore.Filters;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwaggerGen(options => { options.AddDocs(); })
    .AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddMySqlDbContext<AccountDbContext>(builder.Configuration);
builder.Services.AddServices();

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