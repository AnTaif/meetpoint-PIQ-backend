using AccountService.Contracts.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AccountService.Docs.RequestExamples;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples() => new()
    {
        Email = "temp@mail.ru",
        Password = "password"
    };
}