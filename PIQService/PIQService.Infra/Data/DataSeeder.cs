using Microsoft.Extensions.DependencyInjection;
using PIQService.Models.Dbo;

namespace PIQService.Infra.Data;

public class DataSeeder
{
    private static readonly Guid tutor1Id = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368");
    private static readonly Guid member1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid event1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid direction1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid project1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");
    private static readonly Guid team1Id = Guid.Parse("9d8ee7c8-c5af-46b5-8b09-df7fa5729ef5");

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

        SeedUsers(dbContext);
        SeedTeams(dbContext);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Seeding ended...");
    }

    private static void SeedUsers(AppDbContext dbContext)
    {
        var tutor1 = new UserDbo
        {
            Id = tutor1Id,
        };

        var member1 = new UserDbo
        {
            Id = member1Id,
            TeamId = team1Id,
        };

        dbContext.Users.AddRange(tutor1, member1);
    }

    private static void SeedTeams(AppDbContext dbContext)
    {
        var event1 = new EventDbo
        {
            Id = event1Id,
            Name = "Событие 1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(4),
        };

        var direction1 = new DirectionDbo
        {
            Id = direction1Id,
            EventId = event1Id,
            Name = "Направление 1",
        };

        var project1 = new ProjectDbo
        {
            Id = project1Id,
            DirectionId = direction1Id,
            Name = "Проект 1",
        };

        var team1 = new TeamDbo
        {
            Id = team1Id,
            ProjectId = project1Id,
            TutorId = tutor1Id,
            Name = "Команда 1",
        };

        dbContext.Events.AddRange(event1);
        dbContext.Directions.AddRange(direction1);
        dbContext.Projects.AddRange(project1);
        dbContext.Teams.AddRange(team1);
    }
}