namespace AccountService.Options;

public class S3Options
{
    public string Profile { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string SecretToken { get; set; } = null!;
    
    public string Url { get; set; } = null!;
}