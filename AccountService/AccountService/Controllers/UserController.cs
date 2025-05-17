using AccountService.Contracts;
using AccountService.Services;
using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController(
    IUserAvatarService avatarService
)
    : ControllerBase
{
    /// <summary>
    /// Загрузка аватара для авторизованного пользователя
    /// </summary>
    [HttpPost("upload-avatar")]
    public async Task<ActionResult<AvatarDto>> UploadAvatar(IFormFile file)
    {
        var result = await avatarService.UploadAsync(file.OpenReadStream(), User.ReadSid());
        return result.ToActionResult(this);
    }
}