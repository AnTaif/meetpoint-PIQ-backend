using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers;

[ApiController]
[Route("health-check")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public Task<ActionResult<string>> Check()
    {
        return Task.FromResult<ActionResult<string>>("OK");
    }
}