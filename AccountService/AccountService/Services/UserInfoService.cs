using AccountService.Contracts;
using AccountService.Models;
using Core.Results;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Services;

public class UserInfoService(UserManager<User> userManager) : IUserInfoService
{
    public async Task<Result<UserInfoDto>> GetUserInfoAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return StatusError.NotFound("User not found");
        }

        return new UserInfoDto
        {
            FullName = user.GetFullName(),
            Email = user.Email!,
            AvatarUrl = user.AvatarUrl ?? "https://ibb.co/0R5GkRft",
        };
    }
}