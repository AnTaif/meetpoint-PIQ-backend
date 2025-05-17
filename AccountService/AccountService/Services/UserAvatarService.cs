using AccountService.Contracts;
using AccountService.Models;
using Core.Results;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Services;

public class UserAvatarService(
    IS3FileStorage s3FileStorage,
    UserManager<User> userManager
)
    : IUserAvatarService
{
    private const string bucketName = "avatars";

    public async Task<Result<AvatarDto>> UploadAsync(Stream imageStream, Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return StatusError.NotFound("User not found");
        }

        var url = await s3FileStorage.UploadAsync(imageStream, bucketName, Guid.NewGuid().ToString());

        if (url == null)
        {
            return StatusError.BadRequest("Cannot upload this file");
        }

        user.SetAvatar(url);
        await userManager.UpdateAsync(user);

        return new AvatarDto
        {
            Url = url,
        };
    }

    public async Task<Result> DeleteCurrentAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return StatusError.NotFound("User not found");
        }

        if (user.AvatarUrl == null) return Result.Success;

        user.SetAvatar(null);

        await userManager.UpdateAsync(user);
        await s3FileStorage.DeleteAsync(user.AvatarUrl);

        return Result.Success;
    }
}