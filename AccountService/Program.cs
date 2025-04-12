using AccountService.Data;
using AccountService.Models;
using AccountService.Options;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AccountDbContext>()
    .AddDefaultTokenProviders();

var jwtOptions = new JwtOptions();
builder.Configuration.GetSection("JwtOptions").Bind(jwtOptions);

jwtOptions.Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
                    throw new ArgumentNullException(nameof(Program), "JWT_SECRET environment variable is not set");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(),
            ClockSkew = TimeSpan.Zero
        };
    });

var dbHost = Environment.GetEnvironmentVariable("DB_CONTAINER") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "3306";
var dbName = builder.Configuration.GetSection("DatabaseName").Value
             ?? throw new NullReferenceException("Cannot parse DatabaseName from appsettings");
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER")!;
var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD")!;

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

builder.Services.AddDbContext<AccountDbContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 2)));
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
app.MapControllers();

app.Run();