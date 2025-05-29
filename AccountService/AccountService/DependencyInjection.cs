using AccountService.Data;
using AccountService.Models;
using AccountService.Options;
using AccountService.Providers;
using AccountService.Services;
using Amazon.S3;
using Microsoft.AspNetCore.Identity;

namespace AccountService;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserInfoService, UserInfoService>();
        services.AddScoped<IUserAvatarService, UserAvatarService>();
    }

    public static IServiceCollection AddS3Storage(this IServiceCollection services, IConfiguration configuration)
    {
        var s3Options = new S3Options();
        configuration.GetSection("S3Options").Bind(s3Options);

        s3Options.AccessKeyId = Environment.GetEnvironmentVariable("S3_ACCESS_KEY_ID") ??
                                throw new Exception("S3_ACCESS_KEY_ID not found in env");
        s3Options.SecretToken = Environment.GetEnvironmentVariable("S3_SECRET_TOKEN") ??
                                throw new Exception("S3_SECRET_TOKEN not found in env");

        services.Configure<S3Options>(options =>
        {
            options.Profile = s3Options.Profile;
            options.Region = s3Options.Region;
            options.ServiceUrl = s3Options.ServiceUrl;
            options.AccessKeyId = s3Options.AccessKeyId;
            options.SecretToken = s3Options.SecretToken;
            options.BucketName = s3Options.BucketName;
        });

        var config = new AmazonS3Config()
        {
            ServiceURL = s3Options.ServiceUrl,
        };

        services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Options.AccessKeyId, s3Options.SecretToken, config));
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