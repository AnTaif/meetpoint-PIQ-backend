using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.ResponseExamples;

public class UserMeanScoreDtoExample : IExamplesProvider<UserMeanScoreDto>
{
    public UserMeanScoreDto GetExamples() =>
        new()
        {
            UserId = Guid.NewGuid(),
            FullName = "Гавриляк Михаил Витальевич",
            TeamId = Guid.NewGuid(),
            TeamName = "ПВК 3",
            ScoreByCriteriaIds = new Dictionary<Guid, double>
            {
                { Guid.Parse("0f7b2cb4-5981-4976-b08b-e4209a3f50bd"), 1.5 },
                { Guid.Parse("995f61f3-3e97-40e3-b781-ba11a9e5c11e"), 1 },
                { Guid.Parse("d68c4df2-0bdf-4541-bcf5-c7827e78d333"), 2.3 },
                { Guid.Parse("f054770d-9c6f-417c-9144-40fdbdc2d730"), 2.8 },
            },
        };
}

public class EnumerableUserMeanScoreDtoExample : IExamplesProvider<IEnumerable<UserMeanScoreDto>>
{
    public IEnumerable<UserMeanScoreDto> GetExamples() =>
    [
        new()
        {
            UserId = Guid.NewGuid(),
            FullName = "Гавриляк Михаил Витальевич",
            TeamId = Guid.Parse("ad773a1b-99ea-45a1-905b-da18573c4f87"),
            TeamName = "ПВК 3",
            ScoreByCriteriaIds = new Dictionary<Guid, double>
            {
                { Guid.Parse("0f7b2cb4-5981-4976-b08b-e4209a3f50bd"), 1.5 },
                { Guid.Parse("995f61f3-3e97-40e3-b781-ba11a9e5c11e"), 1 },
                { Guid.Parse("d68c4df2-0bdf-4541-bcf5-c7827e78d333"), 2.3 },
                { Guid.Parse("f054770d-9c6f-417c-9144-40fdbdc2d730"), 2.8 },
            },
        },
        new()
        {
            UserId = Guid.NewGuid(),
            FullName = "Полякова Юлия Андреевна",
            TeamId = Guid.Parse("ad773a1b-99ea-45a1-905b-da18573c4f87"),
            TeamName = "ПВК 3",
            ScoreByCriteriaIds = new Dictionary<Guid, double>
            {
                { Guid.Parse("0f7b2cb4-5981-4976-b08b-e4209a3f50bd"), 2.45 },
                { Guid.Parse("995f61f3-3e97-40e3-b781-ba11a9e5c11e"), 0.99 },
                { Guid.Parse("d68c4df2-0bdf-4541-bcf5-c7827e78d333"), 2.5 },
                { Guid.Parse("f054770d-9c6f-417c-9144-40fdbdc2d730"), 1 },
            },
        },
    ];
}