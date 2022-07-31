namespace DOPA.Extensions;

/// <summary>
/// Convenience extensions for dealing with enumerables.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Do an Action on every item in the enumerable.
    /// </summary>
    /// <remarks>Enumerates the enumerable.</remarks>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="callback">The Action to perform.</param>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <returns>The enumerable.</returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> callback)
    {
        foreach (T item in enumerable)
        {
            callback(item);
        }

        return enumerable;
    }

    /// <summary>
    /// Converts a dictionary to its case insensitive equivalent.
    /// </summary>
    /// <param name="dict">The dictionary.</param>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <returns>The dictionary, now with case-insensitive keys.</returns>
    public static IDictionary<string, TValue> WithCaseInsensitiveKeys<TValue>(this IDictionary<string, TValue> dict) =>
        dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
}
