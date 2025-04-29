using AccountService.Models;
using Core.Auth;
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
        var testUser = new User(
            Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368"),
            "temp@mail.ru",
            "Анна",
            "Мациева",
            null
        );

        var result = await userManager.CreateAsync(testUser, "password");

        if (result.Succeeded)
        {
            await userManager.AddToRolesAsync(testUser, [RolesConstants.Tutor]);
        }
    }
}