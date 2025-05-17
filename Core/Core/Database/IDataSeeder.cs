namespace Core.Database;

public interface IDataSeeder
{
    Task<bool> TrySeedAsync();
}