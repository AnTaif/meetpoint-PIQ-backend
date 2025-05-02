using PIQService.Models.Dto;
using PIQService.Models.Dto.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.ResponseExamples;

public class GetEventHierarchyResponseExample : IExamplesProvider<GetEventHierarchyResponse>
{
    public GetEventHierarchyResponse GetExamples() =>
        new()
        {
            Event = new EventDto
            {
                Id = Guid.Parse("812eea5a-7775-4ef8-a7fe-8c88f450b46b"),
                Name = "ПП Весна 2025",
                StartDate = new DateTime(2025, 2, 11),
                EndDate = new DateTime(2025, 6, 1),
                Directions =
                [
                    new DirectionDto
                    {
                        Id = Guid.Parse("5d7825ff-a113-42db-ab04-fe683daf90ad"),
                        Name = "Точка сбора",
                        Projects =
                        [
                            new ProjectDto
                            {
                                Id = Guid.Parse("ee901c82-61f5-4ab5-8567-14cf97b2b81c"),
                                Name = "ПВК",
                                Teams =
                                [
                                    new TeamDto
                                    {
                                        Id = Guid.Parse("efd4d2d8-114d-4807-89e2-c65b8d10dcaa"),
                                        Name = "ПВК 1",
                                        Tutor = new UserDto
                                        {
                                            Id = Guid.Parse("a1615f01-814d-4791-8d14-e55fbf34f57b"),
                                        },
                                        Members =
                                        [
                                            new UserDto
                                            {
                                                Id = Guid.Parse("0bd83a16-cdaf-418c-b66e-0530ec9893c4"),
                                            },
                                        ],
                                    },
                                    new TeamDto
                                    {
                                        Id = Guid.Parse("c5a0d585-2939-42ca-9c0d-5fcea9779980"),
                                        Name = "ПВК 3",
                                        Tutor = new UserDto
                                        {
                                            Id = Guid.Parse("412cf4d8-340c-4170-b2db-a0c8ac1ab93d"),
                                        },
                                        Members =
                                        [
                                            new UserDto
                                            {
                                                Id = Guid.Parse("1ca6664f-03f2-4472-b031-f2cdc23cb2af"),
                                            },
                                            new UserDto
                                            {
                                                Id = Guid.Parse("95ecd1a1-54e2-4e76-ad41-451f004ae2ec"),
                                            },
                                        ],
                                    },
                                ],
                            },
                        ],
                    },
                ],
            },
            TeamIdsForEvaluation =
            [
                Guid.Parse("efd4d2d8-114d-4807-89e2-c65b8d10dcaa"),
                Guid.Parse("c5a0d585-2939-42ca-9c0d-5fcea9779980"),
            ],
        };
}