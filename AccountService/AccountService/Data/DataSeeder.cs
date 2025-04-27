using AccountService.Models;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Data;

public class DataSeeder
{
    private readonly IServiceProvider serviceProvider;

    public DataSeeder(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task SeedAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
     
        Console.WriteLine("Start seeding...");

        if (!await dbContext.Database.EnsureCreatedAsync())
        {
            return;
        }

        await SeedUsersAsync(userManager, dbContext);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Seeding ended...");
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager, AccountDbContext dbContext)
    {
        var testUser = new User("temp@mail.ru", "Имя", "Фамилия", "Отчество");

        var result = await userManager.CreateAsync(testUser, "password");
    }
}