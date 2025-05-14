using Core.Database;
using Microsoft.Extensions.Logging;
using PIQService.Models.Dbo;

namespace PIQService.Infra.Data.Seeding;

public class DataSeeder(
    ITemplateSeedingHelper templateSeedingHelper,
    AppDbContext dbContext,
    ILogger<DataSeeder> logger
)
    : IDataSeeder
{
    private const string mainTemplateFileName = "mainTemplate.json";
    
    private readonly Guid tutorId = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368");
    private readonly Guid templateId = Guid.Parse("d85cf73a-b8c8-4b0d-85f0-4ff242bba9c1");

    public async Task SeedAsync()
    {
        logger.LogInformation("Starting database seeding...");

        if (!await dbContext.Database.EnsureCreatedAsync() && dbContext.Users.Any())
        {
            logger.LogInformation("Database already has some data, skipping...");
            return;
        }

        SeedEventRelatedData();

        var root = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.ToString(), "PIQService.Infra");
        var mainTemplateJsonPath = Path.Combine(root, "Data", "Seeding", mainTemplateFileName);
        await templateSeedingHelper.SeedTemplateFromJsonAsync(mainTemplateJsonPath);

        await dbContext.SaveChangesAsync();
        logger.LogWarning("Database seeding completed.");
    }

    private void SeedEventRelatedData()
    {
        var springEvent = new EventDbo
        {
            Id = Guid.NewGuid(),
            Name = "ПП Весна 2025",
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.UtcNow.AddMonths(5),
            TemplateId = templateId,
        };
        dbContext.Events.Add(springEvent);

        var directions = new[]
        {
            new DirectionDbo { Id = NewGuid(), EventId = springEvent.Id, Name = "Точка сбора" },
            new DirectionDbo { Id = NewGuid(), EventId = springEvent.Id, Name = "1С" },
        };
        dbContext.Directions.AddRange(directions);

        var projects = new[]
        {
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[0].Id, Name = "Оценка ПВК" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[0].Id, Name = "Личный Кабинет" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[1].Id, Name = "УНФ айки" },
        };
        dbContext.Projects.AddRange(projects);

        var teams = new[]
        {
            new TeamDbo { Id = NewGuid(), ProjectId = projects[0].Id, TutorId = tutorId, Name = "ПВК 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[0].Id, TutorId = tutorId, Name = "ПВК 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[0].Id, TutorId = tutorId, Name = "ПВК 3" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[2].Id, TutorId = tutorId, Name = "УНФ айки" },
        };
        dbContext.Teams.AddRange(teams);

        var users = new List<UserDbo>
        {
            new UserDbo
            {
                Id = tutorId,
                FirstName = "Анна",
                LastName = "Мациева",
            },
        };

        var teamMembers = new Dictionary<Guid, (string Last, string First)[]>
        {
            [teams[0].Id] =
            [
                ("Зверев", "Александр"),
                ("Калугин", "Илья"),
                ("Новиков", "Антон"),
                ("Рябков", "Георгий"),
            ],
            [teams[1].Id] =
            [
                ("Анамнешев", "Николай"),
                ("Куркин", "Артём"),
                ("Лавринович", "Станислав"),
                ("Петриченко", "Максим"),
            ],
            [teams[2].Id] =
            [
                ("Мельников", "Михаил"),
                ("Килязова", "Юния"),
                ("Гавриляк", "Михаил"),
                ("Полякова", "Юлия"),
            ],
            [teams[3].Id] =
            [
                ("Корелин", "Никита"),
                ("Олищук", "Владислав"),
                ("Иванов", "Максим"),
            ],
        };

        foreach (var (teamId, roster) in teamMembers)
        {
            foreach (var (last, first) in roster)
            {
                users.Add(new UserDbo
                {
                    Id = NewGuid(),
                    FirstName = first,
                    LastName = last,
                    TeamId = teamId,
                });
            }
        }

        dbContext.Users.AddRange(users);
    }

    private static Guid NewGuid() => Guid.NewGuid();
}