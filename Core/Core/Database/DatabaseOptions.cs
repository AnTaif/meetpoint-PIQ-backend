namespace Core.Database;

public class DatabaseOptions
{
    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string User { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Host { get; set; } = null!;

    public string Port { get; set; } = null!;

    public string GetConnectionString()
    {
        return $"Host={Host};Port={Port};Database={Name};Username={User};Password={Password}";
    }
}