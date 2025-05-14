namespace PIQService.Infra.Data.Seeding;

public interface ITemplateSeedingHelper
{
    Task SeedTemplateFromJsonAsync(string jsonFilePath);
}