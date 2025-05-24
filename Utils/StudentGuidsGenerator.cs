namespace Utils;

public static class StudentGuidsGenerator
{
    private const string privateReadonlyGuidFieldTemplate = "private readonly Guid {0}{1} = Guid.Parse(\"{2}\");";

    public static List<string> GenerateGuidFields(int count)
    {
        const string fieldName = "studentId";

        var fields = new List<string>();
        for (var i = 1; i <= count; i++)
        {
            var field = string.Format(
                privateReadonlyGuidFieldTemplate,
                fieldName, i, Guid.NewGuid()
            );
            fields.Add(field);
        }

        return fields;
    }
}