namespace AccountService.Options;

public class S3Options
{
    public string Profile { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string ServiceUrl { get; set; } = null!;

    public string AccessKeyId { get; set; } = null!;

    public string SecretToken { get; set; } = null!;

    public string BucketName { get; set; } = null!;
}