using PIQService.Models.Dbo;
using PIQService.Models.Domain;

namespace PIQService.Models.Converters;

public static class UserConverter
{
    public static UserDbo ToDboModel(this User user)
    {
        return new UserDbo
        {
            Id = user.Id,
        };
    }

    public static User ToDomainModel(this UserDbo userDbo)
    {
        return new User(userDbo.Id);
    }
}