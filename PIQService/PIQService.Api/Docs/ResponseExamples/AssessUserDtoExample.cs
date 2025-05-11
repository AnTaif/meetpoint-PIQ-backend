using PIQService.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace PIQService.Api.Docs.ResponseExamples;

public class AssessUserDtoExample : IExamplesProvider<AssessUserDto>
{
    public AssessUserDto GetExamples()
    {
        return new AssessUserDto
        {
            User = new UserDto
            {
                Id = Guid.Parse("ce3aa3a0-31da-4f30-ac71-d531a93f1e0f"),
                FullName = "Мельников Михаил Евгеньевич",
            },
            Assessed = false,
        };
    }
}

public class EnumerableAssessUserDtoExample : IExamplesProvider<IEnumerable<AssessUserDto>>
{
    public IEnumerable<AssessUserDto> GetExamples()
    {
        return
        [
            new AssessUserDto
            {
                User = new UserDto
                {
                    Id = Guid.Parse("ce3aa3a0-31da-4f30-ac71-d531a93f1e0f"),
                    FullName = "Мельников Михаил Евгеньевич",
                },
                Assessed = false,
            },
            new AssessUserDto
            {
                User = new UserDto
                {
                    Id = Guid.Parse("57e6abe1-5a62-43ef-82dc-10f0ce7ea7ac"),
                    FullName = "Гавриляк Михаил Витальевич",
                },
                Assessed = true,
            },
        ];
    }
}