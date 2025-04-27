using PIQService.Models.Dbo;
using PIQService.Models.Domain;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters;

public static class UserConverter
{
    public static UserDbo ToDboModel(this User user)
    {
        return new UserDbo
        {
            Id = user.Id,
            TeamId = user.Team?.Id,
        };
    }

    public static User ToDomainModel(this UserDbo userDbo, bool shouldLoadTeam = true)
    {
        return new User(userDbo.Id, shouldLoadTeam ? userDbo.Team?.ToDomainModel() : null);
    }

    public static UserDto ToDtoModel(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
        };
    }
}