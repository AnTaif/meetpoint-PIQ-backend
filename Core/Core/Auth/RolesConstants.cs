using System.Reflection;

namespace Core.Auth;

public static class RolesConstants
{
    public const string Admin = "Admin";
    public const string Tutor = "Tutor";
    public const string Student = "Member";

    public const string AdminTutor = $"{Admin},{Tutor}";

    public static IReadOnlyCollection<string> GetRoles()
    {
        return typeof(RolesConstants)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f is { IsLiteral: true, IsInitOnly: false } && f.FieldType == typeof(string))
            .Select(f => (string)f.GetRawConstantValue()!)
            .ToArray();
    }
}