using PIQService.Models.Dbo;
using PIQService.Models.Domain;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters;

public static class TeamConverter
{
    public static TeamDbo ToDboModel(this Team team)
    {
        return new TeamDbo
        {
            Id = team.Id,
            ProjectId = team.Project.Id,
            TutorId = team.Tutor.Id,
            Name = team.Name,
        };
    }

    public static Team ToDomainModel(this TeamDbo teamDbo)
    {
        return new Team(
            teamDbo.Id,
            teamDbo.Project.ToDomainModel(),
            teamDbo.Tutor.ToDomainModel(),
            teamDbo.Name,
            teamDbo.Members.Select(u => u.ToDomainModel(false)).ToArray()
        );
    }

    public static TeamDto ToDtoModel(this Team team)
    {
        return new TeamDto
        {
            Id = team.Id,
            Project = team.Project.ToDtoModel(),
            Tutor = team.Tutor.ToDtoModel(),
            Name = team.Name,
            Members = team.Users.Select(u => u.ToDtoModel()).ToList(),
        };
    }
}