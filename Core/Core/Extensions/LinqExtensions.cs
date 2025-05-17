namespace Core.Extensions;

public static class LinqExtensions
{
    public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "Source cannot be null.");
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), "Action cannot be null.");
        }

        foreach (var item in source)
        {
            action(item);
        }
    }

    public static async Task ForeachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "Source cannot be null.");
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), "Action cannot be null.");
        }

        foreach (var item in source)
        {
            await action(item);
        }
    }
}