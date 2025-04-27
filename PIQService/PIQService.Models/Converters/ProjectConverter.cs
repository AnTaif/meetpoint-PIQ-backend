using PIQService.Models.Dbo;
using PIQService.Models.Domain;
using PIQService.Models.Dto;

namespace PIQService.Models.Converters;

public static class ProjectConverter
{
    public static ProjectDbo ToDboModel(this Project project)
    {
        return new ProjectDbo
        {
            Id = project.Id,
            DirectionId = project.Direction.Id,
            Name = project.Name,
        };
    }

    public static Project ToDomainModel(this ProjectDbo projectDbo)
    {
        return new Project(
            projectDbo.Id,
            projectDbo.Direction.ToDomainModel(),
            projectDbo.Name
        );
    }

    public static ProjectDto ToDtoModel(this Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Direction = project.Direction.ToDtoModel(),
            Name = project.Name,
        };
    }
}