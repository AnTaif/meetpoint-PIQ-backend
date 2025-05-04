using AccountService.Models;
using Core.Auth;
using Core.Database;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Data;

public class DataSeeder : IDataSeeder
{
    private readonly Guid tutorId = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368");

    private readonly AccountDbContext dbContext;
    private readonly UserManager<User> userManager;
    private readonly ILogger<DataSeeder> logger;

    public DataSeeder(
        AccountDbContext dbContext,
        UserManager<User> userManager,
        ILogger<DataSeeder> logger
    )
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.logger = logger;
    }

    public async Task SeedAsync()
    {
        logger.LogInformation("Starting database seeding...");

        if (!await dbContext.Database.EnsureCreatedAsync() && dbContext.Users.Any())
        {
            logger.LogWarning("Database already has some data, skipping...");
            return;
        }

        await SeedUsersAsync();
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Database seeding completed.");
    }

    private async Task SeedUsersAsync()
    {
        var testUser = new User(tutorId, "temp@mail.ru", "Анна", "Мациева", null);

        var result = await userManager.CreateAsync(testUser, "password");

        if (result.Succeeded)
        {
            await userManager.AddToRolesAsync(testUser, [RolesConstants.Tutor]);
        }
    }
}