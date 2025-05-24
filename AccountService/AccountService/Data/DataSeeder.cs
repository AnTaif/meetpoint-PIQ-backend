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

    // TODO: автоматизировать добвление пользователей (парсить из файла)
    private async Task SeedUsersAsync()
    {
        var tutors = new List<(User, string[])>
        {
            (
                new User(tutorId1, "evseeva@mail.ru", "Алина", "Евсеева", null),
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
            (
                new User(studentId38, "melnikov@mail.ru", "Михаил", "Мельников", null),
                [RolesConstants.Student]
            ),
            (
                new User(studentId39, "kilyazova@mail.ru", "Юния", "Килязова", null),
                [RolesConstants.Student]
            ),
            (
                new User(studentId40, "gavrilyak@mail.ru", "Михаил", "Гавриляк", null),
                [RolesConstants.Student]
            ),
            (
                new User(studentId41, "polyakova@mail.ru", "Юлия", "Полякова", null),
                [RolesConstants.Student]
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
    
    private readonly Guid studentId1 = Guid.Parse("accf60c2-db00-4baa-9f02-e1e5690641b9");
    private readonly Guid studentId2 = Guid.Parse("1f5079b1-0b9e-448e-8c7a-b8d7c18c7f05");
    private readonly Guid studentId3 = Guid.Parse("56251c70-f099-4b2c-af48-84e33764efa7");
    private readonly Guid studentId4 = Guid.Parse("652b8161-639f-4ee2-9f7e-d18e8905c259");
    private readonly Guid studentId5 = Guid.Parse("6a999d86-259a-40d1-978e-81489b365266");
    private readonly Guid studentId6 = Guid.Parse("38c4a4f2-9120-47bf-9f39-12fee4ffac23");
    private readonly Guid studentId7 = Guid.Parse("902eda93-fc38-4293-9f71-bfcfa7d17779");
    private readonly Guid studentId8 = Guid.Parse("f9213649-8111-4c1f-bea6-83e48bbb3abc");
    private readonly Guid studentId9 = Guid.Parse("8d19a483-80e3-4051-9f90-1f0bd46b09d3");
    private readonly Guid studentId10 = Guid.Parse("6da1758c-c4cb-49d3-9347-2797c2d1acd1");
    private readonly Guid studentId11 = Guid.Parse("4e5b2361-25cc-4bf7-a5e4-7c2c4aea8e9e");
    private readonly Guid studentId12 = Guid.Parse("362fefba-67c0-45d8-9fa4-a91df4417bfc");
    private readonly Guid studentId13 = Guid.Parse("02827d6d-c591-40e6-b7da-66a770f4108e");
    private readonly Guid studentId14 = Guid.Parse("fb13443a-1fb2-4c28-bde7-4cf048bc504e");
    private readonly Guid studentId15 = Guid.Parse("20034d24-d24a-41af-9bc0-1453c3f0ff28");
    private readonly Guid studentId16 = Guid.Parse("960778ff-20b6-47d7-a479-cfdb97fdc1fb");
    private readonly Guid studentId17 = Guid.Parse("24141fbe-06e4-4489-82d4-bea9a43abe39");
    private readonly Guid studentId18 = Guid.Parse("e9212eb7-d945-4373-9d2b-35827ac78725");
    private readonly Guid studentId19 = Guid.Parse("8bfac524-8186-44c4-92fe-b074c54b7db2");
    private readonly Guid studentId20 = Guid.Parse("8543492a-b97a-4668-857c-2bd81f376803");
    private readonly Guid studentId21 = Guid.Parse("c6441101-2de6-488c-ab1a-77efd5af5f17");
    private readonly Guid studentId22 = Guid.Parse("1df4b009-982e-4f90-aa09-4b823578e833");
    private readonly Guid studentId23 = Guid.Parse("f7bcd9b7-b420-419f-9b88-46abcebd7bdb");
    private readonly Guid studentId24 = Guid.Parse("fa28d137-2e73-4b44-b90c-709913dbf6b0");
    private readonly Guid studentId25 = Guid.Parse("e2540aaf-8478-4db4-80c7-c813ee153e89");
    private readonly Guid studentId26 = Guid.Parse("219e05c6-5658-410d-bfd8-6f7a7e31d9e9");
    private readonly Guid studentId27 = Guid.Parse("b1ebf082-0352-4ab3-aa90-db2787bb94e7");
    private readonly Guid studentId28 = Guid.Parse("eb4b60fb-4106-4354-86cf-9fed5078abcd");
    private readonly Guid studentId29 = Guid.Parse("9897c1e2-ead1-40ce-b1dd-31421f303f12");
    private readonly Guid studentId30 = Guid.Parse("cffdc61d-7df8-482d-881c-b7c7bb9e29d8");
    private readonly Guid studentId31 = Guid.Parse("956ed38f-67f6-4e9d-ae35-eed99880c813");
    private readonly Guid studentId32 = Guid.Parse("439057d2-c842-4739-9aa9-1a9e1cd9a392");
    private readonly Guid studentId33 = Guid.Parse("195e4797-c47c-41da-9659-e3368db8ae47");
    private readonly Guid studentId34 = Guid.Parse("e385bcac-0ac6-4e9e-90e2-ea54e0f36c8e");
    private readonly Guid studentId35 = Guid.Parse("40530f45-77db-45dd-a45e-32c076badbac");
    private readonly Guid studentId36 = Guid.Parse("a6aeef9a-0a23-482f-8a9f-b4353209da57");
    private readonly Guid studentId37 = Guid.Parse("98b75afb-14f2-4643-804e-123bef8fd694");
    private readonly Guid studentId38 = Guid.Parse("68447f33-8f15-40e5-a62b-faaf1908f347");
    private readonly Guid studentId39 = Guid.Parse("e17e5b92-d236-495a-a423-659daf702785");
    private readonly Guid studentId40 = Guid.Parse("4a02828f-0663-4eeb-b5d1-cf002829ae49");
    private readonly Guid studentId41 = Guid.Parse("56eed0db-966a-4b16-b6fd-1c254f4984a4");
    private readonly Guid studentId42 = Guid.Parse("c87391c4-d49f-4482-b404-c230143b9288");
    private readonly Guid studentId43 = Guid.Parse("820405c4-c526-41f8-b615-56091ee92be6");
    private readonly Guid studentId44 = Guid.Parse("21d5e8c5-12ce-4363-af9d-884e30962679");
    private readonly Guid studentId45 = Guid.Parse("8121a32a-fadd-4583-a263-be805273e54f");
    private readonly Guid studentId46 = Guid.Parse("8af328c8-68c2-456c-bed2-de9406244b5a");
    private readonly Guid studentId47 = Guid.Parse("aa47ecfd-b1f8-417b-a74a-997bc38b3d60");
    private readonly Guid studentId48 = Guid.Parse("55326628-3bf7-4080-9f61-f5b77b7b205c");
    private readonly Guid studentId49 = Guid.Parse("cd1c8547-e865-44c7-84e3-476d38523aa8");
    private readonly Guid studentId50 = Guid.Parse("8b2da13d-51f8-4556-8a85-cab82bd09f76");
    private readonly Guid studentId51 = Guid.Parse("d821b37a-dd89-4b91-9a8e-73972be893b9");
    private readonly Guid studentId52 = Guid.Parse("544ec26b-2e6c-4f0d-a846-34ca974a0618");
    private readonly Guid studentId53 = Guid.Parse("bb66ee98-b002-47ec-a03e-9b9527ce70cd");
    private readonly Guid studentId54 = Guid.Parse("2062fc84-1d66-4193-89ad-af2bfdc71cef");
    private readonly Guid studentId55 = Guid.Parse("6383540f-768c-48e3-b84e-f18a942caf30");
    private readonly Guid studentId56 = Guid.Parse("8d278ab6-8268-4662-915b-6ddd18787175");
    private readonly Guid studentId57 = Guid.Parse("40332499-f404-4a1b-aec9-7ad4b4a8c62e");
    private readonly Guid studentId58 = Guid.Parse("4e6c57e1-9f48-4c30-9a16-83b97a606f70");
    private readonly Guid studentId59 = Guid.Parse("be6a2399-edea-450a-a7ef-a8134e8d8296");
    private readonly Guid studentId60 = Guid.Parse("6e96c0c7-55bb-48c4-880b-fa2c6f7a5a45");
    private readonly Guid studentId61 = Guid.Parse("9668b533-3b59-4b53-90d8-404ab31b8371");
    private readonly Guid studentId62 = Guid.Parse("d5d8cc06-10df-43ef-941e-f88f894da9ff");
    private readonly Guid studentId63 = Guid.Parse("75470a3f-c7b0-41d5-8ef9-3c3fbfca9f3e");
    private readonly Guid studentId64 = Guid.Parse("6aec4510-38ae-4433-8c8d-acc27b602ec9");
    private readonly Guid studentId65 = Guid.Parse("7ce0de10-5a4b-4eb9-968a-cb07e96475cb");
    private readonly Guid studentId66 = Guid.Parse("3f1b1844-87ea-4d0a-9961-ddafbf449b29");
    private readonly Guid studentId67 = Guid.Parse("2ec8c221-bd56-4227-a4b3-1418ecc1ee0a");
    private readonly Guid studentId68 = Guid.Parse("fccd4ff0-2c16-4e4c-8fa5-def8a824079a");
    private readonly Guid studentId69 = Guid.Parse("529fb667-c0ea-4cea-8fb5-363db11ab9f5");
    private readonly Guid studentId70 = Guid.Parse("e59af3b6-2ebd-4b4d-993f-8f77d109b674");
    private readonly Guid studentId71 = Guid.Parse("dc95f284-19b3-42a3-a37c-136b0832fef7");
    private readonly Guid studentId72 = Guid.Parse("ba0cc8a2-e157-4121-bc83-dc03ba9d8468");
    private readonly Guid studentId73 = Guid.Parse("9d826d13-fdf3-467f-be8f-59e341800081");
    private readonly Guid studentId74 = Guid.Parse("87860666-22b0-4096-b477-d55f5a89f267");
    private readonly Guid studentId75 = Guid.Parse("527e2eaf-f658-47a7-8996-d60c311cc9b3");
    private readonly Guid studentId76 = Guid.Parse("a4fc7af8-f906-44a2-a2d3-480aa1ca2dba");
    private readonly Guid studentId77 = Guid.Parse("3f8f8bd8-c536-4746-9f86-9148d708ae42");
    private readonly Guid studentId78 = Guid.Parse("57f20ec1-deff-4d36-bf6b-2bc45f92d72b");
    private readonly Guid studentId79 = Guid.Parse("bb8ef661-f057-431e-903b-d1fe14f7d4e9");
    private readonly Guid studentId80 = Guid.Parse("19086ef9-fe46-48d2-94e2-dfe4cf1a4a51");
    private readonly Guid studentId81 = Guid.Parse("a4c71c76-d617-455a-9961-16e551d71886");
    private readonly Guid studentId82 = Guid.Parse("36d68710-9af4-4f28-8eed-48bb07385ce5");
    private readonly Guid studentId83 = Guid.Parse("68cb77b0-9725-4ed8-8999-44130efa924c");
}