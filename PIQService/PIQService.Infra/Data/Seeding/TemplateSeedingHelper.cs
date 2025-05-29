using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PIQService.Infra.Data.Seeding.JsonConfigs;
using PIQService.Models.Dbo.Assessments;

namespace PIQService.Infra.Data.Seeding;

[RegisterTransient]
public class TemplateSeedingHelper(AppDbContext dbContext, ILogger<TemplateSeedingHelper> logger) : ITemplateSeedingHelper
{
    public async Task SeedTemplateFromJsonAsync(string templateFileName)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var embeddedResourcePath = Path.Combine("Data", "Seeding", "Templates", templateFileName);
            var resourceName = $"{assembly.GetName().Name}.{embeddedResourcePath.Replace("\\", ".").Replace("/", ".")}";

            logger.LogDebug("Reading json template from resource {resourceName}", resourceName);
            await using var stream = assembly.GetManifestResourceStream(resourceName);
            
            if (stream == null)
            {
                throw new FileNotFoundException($"Resource '{resourceName}' not found.");
            }

            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            
            var templateConfig = JsonSerializer.Deserialize<TemplateConfig>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            if (templateConfig == null)
            {
                logger.LogError("Failed to deserialize assessment template config");
                return;
            }
            
            var circleForm = CreateForm(templateConfig.CircleForm);
            var behaviorForm = CreateForm(templateConfig.BehaviorForm);

            var template = new TemplateDbo
            {
                Id = templateConfig.Id,
                Name = templateConfig.Name,
                CircleFormId = circleForm.Id,
                BehaviorFormId = behaviorForm.Id
            };

            dbContext.Templates.Add(template);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding assessment templates from JSON");
            throw;
        }
    }

    private Dictionary<string, CriteriaDbo> CreateCriteria(List<CriteriaConfig> criteriaConfigs)
    {
        var criteriaMap = new Dictionary<string, CriteriaDbo>();

        foreach (var config in criteriaConfigs)
        {
            var criteria = new CriteriaDbo
            {
                Id = Guid.NewGuid(),
                Name = config.Name,
                Description = config.Description,
            };

            dbContext.Criteria.Add(criteria);
            criteriaMap[config.Name] = criteria;
        }

        return criteriaMap;
    }

    private FormDbo CreateForm(FormConfig formConfig)
    {
        var criteriaMap = CreateCriteria(formConfig.Criteria);
        
        var formCriteria = formConfig.Criteria
            .Select(c => criteriaMap[c.Name])
            .ToList();

        var form = new FormDbo
        {
            Id = Guid.NewGuid(),
            Type = formConfig.Type,
            CriteriaList = formCriteria,
        };

        dbContext.Forms.Add(form);

        var questions = new List<QuestionDbo>();
        var choices = new List<ChoiceDbo>();

        foreach (var questionConfig in formConfig.Questions)
        {
            if (!criteriaMap.TryGetValue(questionConfig.CriteriaName, out var criteria))
            {
                throw new Exception($"Criteria '{questionConfig.CriteriaName}' not found for question: {questionConfig.Text}");
            }

            var question = new QuestionDbo
            {
                Id = Guid.NewGuid(),
                QuestionText = questionConfig.Text,
                Weight = questionConfig.Weight,
                FormId = form.Id,
                CriteriaId = criteria.Id,
                Order = questionConfig.Order,
            };

            questions.Add(question);

            choices.AddRange(questionConfig.Choices.Select(choiceConfig => new ChoiceDbo
            {
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
                Text = choiceConfig.Text,
                Value = choiceConfig.Value,
                Description = choiceConfig.Description,
            }));
        }

        dbContext.Questions.AddRange(questions);
        dbContext.Choices.AddRange(choices);

        return form;
    }
}