using AccountService.Data;
using AccountService.Models;
using AccountService.Options;
using Amazon.S3;
using Microsoft.AspNetCore.Identity;

namespace AccountService;

public static class DependencyInjection
{
    public static IServiceCollection AddS3Client(this IServiceCollection services, IConfiguration configuration)
    {
        var s3Options = new S3Options();
        configuration.GetSection("S3Options").Bind(s3Options);

        s3Options.AccessKeyId = Environment.GetEnvironmentVariable("S3_ACCESS_KEY_ID") ?? "";
        s3Options.SecretToken = Environment.GetEnvironmentVariable("S3_SECRET_TOKEN") ?? "";

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