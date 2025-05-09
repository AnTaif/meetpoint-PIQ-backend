using Core.Extensions;
using PIQService.Models.Dbo;
using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data;

public class DataSeeder : IDataSeeder
{
    private readonly Guid tutorId = Guid.Parse("0c9e1791-96ea-4533-a2be-1691cfa8a368");
    private readonly Guid templateId = Guid.Parse("d85cf73a-b8c8-4b0d-85f0-4ff242bba9c1");

    private readonly AppDbContext dbContext;
    private readonly ILogger<DataSeeder> logger;

    public DataSeeder(
        AppDbContext dbContext,
        ILogger<DataSeeder> logger
    )
    {
        this.dbContext = dbContext;
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

        SeedEventRelatedData();
        SeedAssessmentTemplates();

        await dbContext.SaveChangesAsync();
        logger.LogInformation("Database seeding completed.");
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

    private void SeedAssessmentTemplates()
    {
        var criteriaList = new[]
        {
            new CriteriaDbo { Id = NewGuid(), Name = "Вовлеченность", Description = "Описание вовлеченности" },
            new CriteriaDbo { Id = NewGuid(), Name = "Организованность", Description = "Описание организованности" },
            new CriteriaDbo { Id = NewGuid(), Name = "Обучаемость", Description = "Описание обучаемости" },
            new CriteriaDbo { Id = NewGuid(), Name = "Командность", Description = "Описание командности" },
        };
        dbContext.Criteria.AddRange(criteriaList);

        var forms = new[]
        {
            new FormDbo { Id = NewGuid(), Type = AssessmentType.Circle, CriteriaList = criteriaList },
            new FormDbo { Id = NewGuid(), Type = AssessmentType.Behavior, CriteriaList = criteriaList },
        };
        dbContext.Forms.AddRange(forms);

        var templates = new[]
        {
            new TemplateDbo { Id = templateId, Name = "Шаблон \"Стажировка\"", CircleFormId = forms[0].Id, BehaviorFormId = forms[1].Id },
        };
        dbContext.Templates.AddRange(templates);

        var questions = new[]
        {
            CreateQuestion("Проявляет инициативу в обсуждениях?", 0.5f, 0, forms[0].Id, criteriaList[0].Id),
            CreateQuestion("Делится знаниями с командой?", 0.5f, 1, forms[0].Id, criteriaList[0].Id),
            CreateQuestion("Работает системно, без авралов?", 0.5f, 2, forms[0].Id, criteriaList[1].Id),
            CreateQuestion("Документирует свои процессы?", 0.5f, 3, forms[0].Id, criteriaList[1].Id),
            CreateQuestion("Быстро осваивает новые инструменты?", 0.5f, 4, forms[0].Id, criteriaList[2].Id),
            CreateQuestion("Применяет новые знания на практике?", 0.5f, 5, forms[0].Id, criteriaList[2].Id),
            CreateQuestion("Помогает коллегам без напоминаний?", 0.5f, 6, forms[0].Id, criteriaList[3].Id),
            CreateQuestion("Конструктивно решает конфликты?", 0.5f, 7, forms[0].Id, criteriaList[3].Id),
        };
        dbContext.Questions.AddRange(questions);

        var choices = new List<ChoiceDbo>();
        foreach (var question in questions)
        {
            var tempChoices = new[]
            {
                new ChoiceDbo { Text = "-1", Value = -1, Description = "Описание выбора" },
                new ChoiceDbo { Text = "0", Value = 0, Description = "Описание выбора" },
                new ChoiceDbo { Text = "1", Value = 1, Description = "Описание выбора" },
                new ChoiceDbo { Text = "2", Value = 2, Description = "Описание выбора" },
                new ChoiceDbo { Text = "3", Value = 3, Description = "Описание выбора" },
            };

            tempChoices.Foreach(c =>
            {
                c.Id = NewGuid();
                c.QuestionId = question.Id;
                choices.Add(c);
            });
        }

        dbContext.Choices.AddRange(choices);
    }

    private static QuestionDbo CreateQuestion(string text, float weight, short order, Guid formId, Guid criteriaId)
    {
        return new QuestionDbo
        {
            Id = Guid.NewGuid(),
            QuestionText = text,
            Weight = weight,
            FormId = formId,
            CriteriaId = criteriaId,
            Order = order,
        };
    }

    private static Guid NewGuid() => Guid.NewGuid();
}