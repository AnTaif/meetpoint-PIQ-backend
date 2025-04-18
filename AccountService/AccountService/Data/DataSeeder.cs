using AccountService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        
        if (dbContext.Users.Any())
        {
            return;
        }

        await dbContext.Database.MigrateAsync();
        await SeedUsersAsync(userManager, dbContext);
    }

    private static async Task SeedUsersAsync(UserManager<User> userManager, AccountDbContext dbContext)
    {
        var testUser = new User("temp@mail.ru", "Имя", "Фамилия", "Отчество");

        var result = await userManager.CreateAsync(testUser, "password");
        await dbContext.SaveChangesAsync();
    }
}