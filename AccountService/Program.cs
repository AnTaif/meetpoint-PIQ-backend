using AccountService;
using AccountService.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

builder.Services.AddSwaggerGen();
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