using System.Collections.Concurrent;

namespace CShopa.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> callback)
    {
        foreach (T item in enumerable)
        {
            callback(item);
        }

        return enumerable;
    }

    public static IDictionary<string, TValue> WithCaseInsensitiveKeys<TValue>(this IDictionary<string, TValue> dict) =>
        dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
}
