using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PIQService.Api.Options;

namespace PIQService.Api;

public static class DependencyInjection
{
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
}