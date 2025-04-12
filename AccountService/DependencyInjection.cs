using AccountService.Data;
using AccountService.Models;
using AccountService.Options;
using AccountService.Providers;
using AccountService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AccountService;

public static class DependencyInjection
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        builder.Services.AddScoped<ITokenProvider, JwtTokenProvider>();
        builder.Services.AddScoped<IAuthService, AuthService>();
    }
    
    public static void AddAuth(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        var jwtOptions = new JwtOptions();
        configuration.GetSection("JwtOptions").Bind(jwtOptions);

        services.Configure<JwtOptions>(options =>
        {
            options.Audience = jwtOptions.Audience;
            options.Issuer = jwtOptions.Issuer;
            options.Secret = jwtOptions.Secret;
            options.ExpiryMinutes = jwtOptions.ExpiryMinutes;
        });

        jwtOptions.Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
                            throw new Exception("JWT_SECRET environment variable is not set");

        services.AddAuthentication(options =>
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
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
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
        
        var dbHost = Environment.GetEnvironmentVariable("DB_CONTAINER") ?? "localhost";
        var dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "3306";
        var dbName = builder.Configuration.GetSection("DatabaseName").Value
                     ?? throw new Exception("Cannot parse DatabaseName from appsettings");
        var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER")!;
        var dbPassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD")!;

        var dbVersion = builder.Configuration.GetSection("DatabaseVersion").Value
                        ?? throw new Exception("Cannot parse DatabaseVersion from appsettings");

        var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

        builder.Services.AddDbContext<AccountDbContext>(options =>
        {
            options.UseMySql(connectionString, new MySqlServerVersion(dbVersion));
        });
    }
}