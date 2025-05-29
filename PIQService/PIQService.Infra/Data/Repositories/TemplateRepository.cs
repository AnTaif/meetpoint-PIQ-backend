using Microsoft.EntityFrameworkCore;
using PIQService.Application.Implementation.Templates;
using PIQService.Models.Converters.Assessments;
using PIQService.Models.Dbo.Assessments;
using PIQService.Models.Domain.Assessments;

namespace PIQService.Infra.Data.Repositories;

[RegisterScoped]
public class TemplateRepository(AppDbContext dbContext) : ITemplateRepository
{
    public async Task<Template?> FindAsync(Guid templateId)
    {
        var dbo = await dbContext.Templates
            .Include(t => t.CircleForm)
            .ThenInclude(f => f.CriteriaList)
            .Include(t => t.CircleForm)
            .ThenInclude(f => f.Questions)
            .ThenInclude(q => q.Criteria)
            .Include(t => t.CircleForm)
            .ThenInclude(f => f.Questions)
            .ThenInclude(q => q.Choices)
            .Include(t => t.BehaviorForm)
            .ThenInclude(f => f.CriteriaList)
            .Include(t => t.BehaviorForm)
            .ThenInclude(f => f.Questions)
            .ThenInclude(q => q.Criteria)
            .Include(t => t.BehaviorForm)
            .ThenInclude(f => f.Questions)
            .ThenInclude(q => q.Choices)
            .SingleOrDefaultAsync(t => t.Id == templateId);

        if (dbo == null) return null;

        dbo.CircleForm.CriteriaList = dbo.CircleForm.CriteriaList.OrderBy(c => c.Name).ToList();
        dbo.BehaviorForm.CriteriaList = dbo.BehaviorForm.CriteriaList.OrderBy(c => c.Name).ToList();

        var circleQuestions = new List<QuestionDbo>();
        foreach (var question in dbo.CircleForm.Questions.OrderBy(q => q.Order))
        {
            question.Choices = question.Choices.OrderBy(c => c.Value).ToList();
            circleQuestions.Add(question);
        }

        var behaviorQuestions = new List<QuestionDbo>();
        foreach (var question in dbo.BehaviorForm.Questions.OrderBy(q => q.Order))
        {
            question.Choices = question.Choices.OrderBy(c => c.Value).ToList();
            behaviorQuestions.Add(question);
        }

        dbo.CircleForm.Questions = circleQuestions;
        dbo.BehaviorForm.Questions = behaviorQuestions;

        return dbo.ToDomainModel();
    }
}