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

    private readonly Guid tutorId1 = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368");
    private readonly Guid tutorId2 = Guid.Parse("eda1fda6-a3dc-4cd5-8ece-271824102afa");
    private readonly Guid tutorId3 = Guid.Parse("18852a52-6e9b-450c-8346-abbfbffe9a2c");
    private readonly Guid tutorId4 = Guid.Parse("3cc2b920-3065-4102-b641-64666f6a05da");
    private readonly Guid templateId = Guid.Parse("d85cf73a-b8c8-4b0d-85f0-4ff242bba9c1");

    public async Task<bool> TrySeedAsync()
    {
        logger.LogInformation("Starting database seeding...");

        if (!await dbContext.Database.EnsureCreatedAsync() && dbContext.Users.Any())
        {
            logger.LogInformation("Database already has some data, skipping...");
            return false;
        }

        SeedEventRelatedData();

        await templateSeedingHelper.SeedTemplateFromJsonAsync(mainTemplateFileName);

        await dbContext.SaveChangesAsync();
        logger.LogWarning("Database seeding completed.");
        return true;
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
            new DirectionDbo { Id = NewGuid(), EventId = springEvent.Id, Name = "1С" },
            new DirectionDbo { Id = NewGuid(), EventId = springEvent.Id, Name = "Игра" },
            new DirectionDbo { Id = NewGuid(), EventId = springEvent.Id, Name = "Точка сбора" },
        };
        dbContext.Directions.AddRange(directions);

        var projects = new[]
        {
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[0].Id, Name = "Журнал тренера" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[0].Id, Name = "УНФ айки" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[0].Id, Name = "Сервис" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[0].Id, Name = "УНФ ДМ" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[1].Id, Name = "Интервью" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[2].Id, Name = "Личный Кабинет" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[2].Id, Name = "Оценка ПВК" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[2].Id, Name = "Страницы" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[2].Id, Name = "Парсер" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[2].Id, Name = "Партнёры" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[2].Id, Name = "CRM" },
            new ProjectDbo { Id = NewGuid(), DirectionId = directions[2].Id, Name = "Тесты" },
        };
        dbContext.Projects.AddRange(projects);

        var teams = new[]
        {
            new TeamDbo { Id = NewGuid(), ProjectId = projects[0].Id, TutorId = tutorId4, Name = "Журнал тренера 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[0].Id, TutorId = tutorId4, Name = "Журнал тренера 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[1].Id, TutorId = tutorId4, Name = "УНФ айки" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[2].Id, TutorId = tutorId4, Name = "Сервис 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[2].Id, TutorId = tutorId4, Name = "Сервис 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[3].Id, TutorId = tutorId4, Name = "УНФ ДМ 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[4].Id, TutorId = tutorId4, Name = "Интервью 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[5].Id, TutorId = tutorId1, Name = "Личный кабинет 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[5].Id, TutorId = tutorId1, Name = "Личный кабинет 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[6].Id, TutorId = tutorId1, Name = "ПВК 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[6].Id, TutorId = tutorId1, Name = "ПВК 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[6].Id, TutorId = tutorId1, Name = "ПВК 3" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[7].Id, TutorId = tutorId3, Name = "Страницы 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[7].Id, TutorId = tutorId3, Name = "Страницы 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[8].Id, TutorId = tutorId2, Name = "Парсер 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[8].Id, TutorId = tutorId2, Name = "Парсер 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[9].Id, TutorId = tutorId2, Name = "Партнёры 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[9].Id, TutorId = tutorId2, Name = "Партнёры 3" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[10].Id, TutorId = tutorId2, Name = "СРМ 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[11].Id, TutorId = tutorId2, Name = "Тесты 1" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[11].Id, TutorId = tutorId2, Name = "Тесты 2" },
            new TeamDbo { Id = NewGuid(), ProjectId = projects[11].Id, TutorId = tutorId2, Name = "Тесты 4" },
        };
        dbContext.Teams.AddRange(teams);

        var users = new List<UserDbo>
        {
            new() { Id = tutorId1, FirstName = "Алина", LastName = "Евсеева", },
            new() { Id = tutorId2, FirstName = "Юрий", LastName = "Пушкарь", },
            new() { Id = tutorId3, FirstName = "Анна", LastName = "Мациева", },
            new() { Id = tutorId4, FirstName = "Денис", LastName = "Смирнов", },
        };

        var teamMembers = new Dictionary<Guid, (string Last, string First)[]>
        {
            [teams[0].Id] =
            [
                ("Воротов", "Дмитрий"),
                ("Кожевин", "Артур"),
                ("Топоркова", "Анастасия"),
            ],
            [teams[1].Id] =
            [
                ("Таборский", "Максим"),
                ("Холявин", "Александр"),
            ],
            [teams[2].Id] =
            [
                ("Корелин", "Никита"),
                ("Олищук", "Владислав"),
                ("Иванов", "Максим"),
            ],
            [teams[3].Id] =
            [
                ("Гагарина", "Карина"),
                ("Зинатулин", "Тимур"),
                ("Соколов", "Владимир"),
                ("Шандер", "Саша-Ойген"),
            ],
            [teams[4].Id] =
            [
                ("Зыков", "Арсений"),
                ("Понарина", "Полина"),
            ],
            [teams[5].Id] =
            [
                ("Большаков", "Артём"),
                ("Пикулин", "Семён"),
            ],
            [teams[6].Id] =
            [
                ("Аптуликсанов", "Руслан"),
                ("Мельникова", "Виктория"),
                ("Симоненко", "Матвей"),
            ],
            [teams[7].Id] =
            [
                ("Верешко", "Егор"),
                ("Евсеева", "Алина"),
                ("Милько", "Артём"),
                ("Оликов", "Юрий"),
                ("Целищева", "Виктория"),
            ],
            [teams[8].Id] =
            [
                ("Валеева", "Алина"),
                ("Курамов", "Лев"),
                ("Сусанина", "Елена"),
                ("Ускова", "Анастасия"),
                ("Штамов", "Алексей"),
            ],
            [teams[9].Id] =
            [
                ("Зверев", "Александр"),
                ("Калугин", "Илья"),
                ("Новиков", "Антон"),
                ("Рябков", "Георгий"),
            ],
            [teams[10].Id] =
            [
                ("Анамнешев", "Николай"),
                ("Куркин", "Артём"),
                ("Лавринович", "Станислав"),
                ("Петриченко", "Максим"),
            ],
            [teams[11].Id] =
            [
                ("Мельников", "Михаил"),
                ("Килязова", "Юния"),
                ("Гавриляк", "Михаил"),
                ("Полякова", "Юлия"),
            ],
            [teams[12].Id] =
            [
                ("Валиханова", "Элина"),
                ("Гребенкина", "Лидия"),
                ("Колыванов", "Семен"),
                ("Кузнецов", "Евгений"),
            ],
            [teams[13].Id] =
            [
                ("Ганеева", "Анна"),
                ("Отставнов", "Михаил"),
                ("Петряков", "Евгений"),
                ("Удников", "Лев"),
                ("Чернюгов", "Лев"),
            ],
            [teams[14].Id] =
            [
                ("Катаева", "Арина"),
                ("Малышев", "Георгий"),
                ("Черных", "Илья"),
            ],
            [teams[15].Id] =
            [
                ("Ергин", "Игорь"),
                ("Ефтеев", "Станислав"),
                ("Микрюков", "Денис"),
                ("Стишенко", "Евгений"),
                ("Судоплатов", "Владислав"),
            ],
            [teams[16].Id] =
            [
                ("Гущин", "Максим"),
                ("Зарипов", "Загир"),
                ("Матович", "Стефан"),
                ("Ханов", "Эмиль"),
                ("Шайхутдинов", "Кирилл"),
            ],
            [teams[17].Id] =
            [
                ("Верхотуров", "Виталий"),
                ("Загидуллин", "Руслан"),
                ("Харковец ", "Ярослав"),
            ],
            [teams[18].Id] =
            [
                ("Алексеев", "Егор"),
                ("Васильцов", "Владимир"),
                ("Вяткина", "Софья"),
                ("Шляпникова", "Дарья"),
            ],
            [teams[19].Id] =
            [
                ("Егорова", "Алиса"),
                ("Петренко", "Андрей"),
                ("Пятайкина", "Валерия"),
                ("Романенко", "Максим"),
                ("Федосова", "Анастасия"),
            ],
            [teams[20].Id] =
            [
                ("Клейн", "Александр"),
                ("Павлова", "Дарья"),
                ("Ржанников", "Никита"),
                ("Старчикова", "Екатерина"),
            ],
            [teams[21].Id] =
            [
                ("Васильченко", "Максим"),
                ("Леонтьев", "Алексей"),
                ("Лукьяненко", "Алина"),
                ("Назаров", "Евгений"),
            ],
        };

        foreach (var (teamId, member) in teamMembers)
        {
            foreach (var (last, first) in member)
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