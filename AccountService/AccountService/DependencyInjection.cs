using AccountService.Data;
using AccountService.Models;
using AccountService.Options;
using AccountService.Providers;
using AccountService.Services;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Identity;

namespace AccountService;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<IAuthService, AuthService>();
    }

    public static IServiceCollection AddS3Storage(this IServiceCollection services, IConfiguration configuration)
    {
        var s3Options = new S3Options();
        configuration.GetSection("S3Options").Bind(s3Options);

        services.Configure<S3Options>(options =>
        {
            options.Profile = s3Options.Profile;
            options.Region = s3Options.Region;
            options.AccessToken = Environment.GetEnvironmentVariable("S3_SECRET_TOKEN") ??
                                  throw new Exception("S3_SECRET_TOKEN not found in env");
            options.SecretToken = Environment.GetEnvironmentVariable("S3_ACCESS_TOKEN") ??
                                  throw new Exception("S3_ACCESS_TOKEN not found in env");
            options.Url = s3Options.Url;
        });

        services.AddAWSService<IAmazonS3>(new AWSOptions
        {
            Profile = s3Options.Profile,
            Region = RegionEndpoint.GetBySystemName(s3Options.Region),
            Credentials = new BasicAWSCredentials(s3Options.AccessToken, s3Options.SecretToken),
        });
        services.AddTransient<IS3FileStorage, S3FileStorage>();

        return services;
    }

    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
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
    }
}