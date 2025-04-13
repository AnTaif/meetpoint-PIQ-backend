namespace AccountService.Options;

public class DatabaseOptions
{
    public string Name { get; set; }

    public string Version { get; set; }

    public string User { get; set; }

    public string Password { get; set; }

    public string Host { get; set; }

    public string Port { get; set; }

    public string GetConnectionString()
    {
        return $"Host={Host};Port={Port};Database={Name};Username={User};Password={Password}";
    }
}