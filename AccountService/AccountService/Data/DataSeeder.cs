using AccountService.Models;
using Core.Auth;
using Core.Database;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Data;

public class DataSeeder(
    AccountDbContext dbContext,
    UserManager<User> userManager,
    ILogger<DataSeeder> logger
)
    : IDataSeeder
{
    private readonly Guid tutorId1 = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368");
    private readonly Guid tutorId2 = Guid.Parse("eda1fda6-a3dc-4cd5-8ece-271824102afa");
    private readonly Guid tutorId3 = Guid.Parse("18852a52-6e9b-450c-8346-abbfbffe9a2c");
    private readonly Guid tutorId4 = Guid.Parse("3cc2b920-3065-4102-b641-64666f6a05da");

    public async Task<bool> TrySeedAsync()
    {
        logger.LogInformation("Starting database seeding...");

        if (!await dbContext.Database.EnsureCreatedAsync() && dbContext.Users.Any())
        {
            logger.LogInformation("Database already has some data, skipping...");
            return false;
        }

        await SeedUsersAsync();
        await dbContext.SaveChangesAsync();
        logger.LogWarning("Database seeding completed.");
        return true;
    }

    private async Task SeedUsersAsync()
    {
        var tutors = new List<(User, string[])>
        {
            (
                new User(tutorId1, "temp@mail.ru", "Алина", "Евсеева", null),
                [RolesConstants.Tutor]
            ),
            (
                new User(tutorId2, "pushkar@mail.ru", "Юрий", "Пушкарь", null),
                [RolesConstants.Tutor]
            ),
            (
                new User(tutorId3, "matsieva@mail.ru", "Анна", "Мациева", null),
                [RolesConstants.Tutor]
            ),
            (
                new User(tutorId4, "smirnov@mail.ru", "Денис", "Смирнов", null),
                [RolesConstants.Tutor, RolesConstants.Admin]
            ),
        };

        await CreateUsersAsync(tutors);
    }

    private async Task CreateUsersAsync(IEnumerable<(User, string[])> users)
    {
        foreach (var (user, roles) in users)
        {
            var result = await userManager.CreateAsync(user, "password");

            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(user, roles);
            }
        }
    }
}