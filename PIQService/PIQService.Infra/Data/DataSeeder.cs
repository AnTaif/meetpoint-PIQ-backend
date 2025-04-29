using Microsoft.Extensions.DependencyInjection;
using PIQService.Models.Dbo;

namespace PIQService.Infra.Data;

public class DataSeeder
{
    private static readonly Guid tutor1Id = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368");
    private static readonly Guid member1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid event1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid direction1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid direction2Id = Guid.Parse("73d9ad25-ab11-410c-8cd9-cc89dc4d6130");
    private static readonly Guid project11Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid project12Id = Guid.Parse("41d9f5f3-61f9-42c6-91bb-1c2c414f794f");
    private static readonly Guid project21Id = Guid.Parse("acc9a174-7730-4f0c-9a77-63cdd9246982");
    private static readonly Guid team1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid team2Id = Guid.Parse("afa6c00c-9934-4ad9-b13f-81d128195478");
    private static readonly Guid team3Id = Guid.Parse("bd038431-7ced-4b8f-83ef-a8b2cd03298e");
    private static readonly Guid team21Id = Guid.Parse("e8aae664-a9e7-4b76-b614-9942c8d77485");

    private readonly IServiceProvider serviceProvider;

    public DataSeeder(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task SeedAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Console.WriteLine("Start seeding...");

        if (!await dbContext.Database.EnsureCreatedAsync())
        {
            return;
        }

        SeedTeams(dbContext);
        SeedUsers(dbContext);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Seeding ended...");
    }

    private static void SeedUsers(AppDbContext dbContext)
    {
        var tutor1 = new UserDbo
        {
            Id = tutor1Id,
            FirstName = "Анна",
            LastName = "Мациева",
        };

        var user11 = GetUser("Зверев", "Александр", team1Id);
        var user12 = GetUser("Калугин", "Илья", team1Id);
        var user13 = GetUser("Новиков", "Антон", team1Id);
        var user14 = GetUser("Рябков", "Георгий", team1Id);

        var user21 = GetUser("Анамнешев", "Николай", team2Id);
        var user22 = GetUser("Куркин", "Артём", team2Id);
        var user23 = GetUser("Лавринович", "Станислав", team2Id);
        var user24 = GetUser("Петриченко", "Максим", team2Id);

        var user31 = GetUser("Мельников", "Михаил", team3Id);
        var user32 = GetUser("Килязова", "Юния", team3Id);
        var user33 = GetUser("Гавриляк", "Михаил", team3Id);
        var user34 = GetUser("Полякова", "Юлия", team3Id);

        var user211 = GetUser("Корелин", "Никита", team21Id);
        var user212 = GetUser("Олищук", "Владислав", team21Id);
        var user213 = GetUser("Иванов", "Максим", team21Id);

        dbContext.Users.AddRange(tutor1,
            user11, user12, user13, user14,
            user21, user22, user23, user24,
            user31, user32, user33, user34,
            user211, user212, user213
        );
    }

    private static void SeedTeams(AppDbContext dbContext)
    {
        var event1 = new EventDbo
        {
            Id = event1Id,
            Name = "ПП Весна 2025",
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.Now.AddMonths(5),
        };

        var direction1 = new DirectionDbo
        {
            Id = direction1Id,
            EventId = event1Id,
            Name = "Точка сбора",
        };

        var direction2 = new DirectionDbo
        {
            Id = direction2Id,
            EventId = event1Id,
            Name = "1С",
        };

        var project11 = new ProjectDbo
        {
            Id = project11Id,
            DirectionId = direction1Id,
            Name = "Оценка ПВК",
        };

        var project12 = new ProjectDbo
        {
            Id = project12Id,
            DirectionId = direction1Id,
            Name = "Личный Кабинет",
        };

        var project21 = new ProjectDbo
        {
            Id = project21Id,
            DirectionId = direction2Id,
            Name = "УНФ айки",
        };

        var team1 = new TeamDbo
        {
            Id = team1Id,
            ProjectId = project11Id,
            TutorId = tutor1Id,
            Name = "ПВК 1",
        };

        var team2 = new TeamDbo
        {
            Id = team2Id,
            ProjectId = project11Id,
            TutorId = tutor1Id,
            Name = "ПВК 2",
        };

        var team3 = new TeamDbo
        {
            Id = team3Id,
            ProjectId = project11Id,
            TutorId = tutor1Id,
            Name = "ПВК 3",
        };

        var team21 = new TeamDbo
        {
            Id = team21Id,
            ProjectId = project21Id,
            TutorId = tutor1Id,
            Name = "УНФ айки",
        };

        dbContext.Events.AddRange(event1);
        dbContext.Directions.AddRange(direction1, direction2);
        dbContext.Projects.AddRange(project11, project12, project21);
        dbContext.Teams.AddRange(team1, team2, team3, team21);
    }

    private static UserDbo GetUser(string lastName, string firstName, Guid teamId)
    {
        return new UserDbo
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            MiddleName = null,
            TeamId = teamId,
        };
    }
}