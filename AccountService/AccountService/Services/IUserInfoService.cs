using AccountService.Contracts;
using Core.Results;

namespace AccountService.Services;

public interface IUserInfoService
{
    Task<Result<UserInfoDto>> GetUserInfoAsync(Guid userId);
}