using AccountService.Contracts;
using AccountService.Services;
using Core.Auth;
using Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers;

[ApiController]
[Route("users")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class UserController(
    ILogger<UserController> logger,
    IUserAvatarService avatarService,
    IUserInfoService userInfoService
)
    : ControllerBase
{
    /// <summary>
    /// Получить текущего пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet("current")]
    public async Task<ActionResult<UserInfoDto>> GetUser()
    {
        var result = await userInfoService.GetUserInfoAsync(User.ReadSid());
        return result.ToActionResult(this);
    }
    
    /// <summary>
    /// Загрузить аватар для текущего пользователя
    /// </summary>
    [HttpPost("upload-avatar")]
    public async Task<ActionResult<AvatarDto>> UploadAvatar(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var result = await avatarService.UploadAsync(file.OpenReadStream(), extension, User.ReadSid());
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Удалить текущий аватар
    /// </summary>
    [HttpDelete("delete-avatar")]
    public async Task<ActionResult> DeleteAvatar()
    {
        var result = await avatarService.DeleteCurrentAsync(User.ReadSid());
        return result.ToActionResult(this);
    }
}