using System.Text.Json.Serialization;

namespace PIQService.Models.Domain.Assessments;

public class Criteria
{
    [JsonInclude]
    public Guid Id { get; private set; }

    [JsonInclude]
    public string Name { get; private set; }

    [JsonInclude]
    public string Description { get; private set; }

    [JsonConstructor]
    public Criteria(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}