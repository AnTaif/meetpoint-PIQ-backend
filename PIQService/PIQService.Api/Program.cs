using Core.Extensions;
using DotNetEnv;
using PIQService.Api;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../../.env");

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.AddJwtSecurity();
});

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