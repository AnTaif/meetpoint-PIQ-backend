using AccountService.Contracts;
using Core.Results;

namespace AccountService.Services;

public interface IUserAvatarService
{
    Task<Result<AvatarDto>> UploadAsync(Stream imageStream, string fileExtension, Guid userId);

    Task<Result> DeleteCurrentAsync(Guid userId);
}