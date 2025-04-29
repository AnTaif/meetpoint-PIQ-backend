using PIQService.Models.Dbo;
using PIQService.Models.Domain;

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
}