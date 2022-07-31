namespace DOPA;

/// <inheritdoc />
public class EntrypointCollection : IEntrypointCollection
{
    private int defaultEntrypointId = WellKnown.Values.DefaultEntrypointId;
    private string defaultEntrypoint;

    private readonly IDictionary<string, int> entrypoints;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="entrypoints">A map of names to entrypoint ids.</param>
    public EntrypointCollection(IDictionary<string, int> entrypoints)
    {
        this.entrypoints = entrypoints;

        if (!entrypoints.Values.Any(v => v == defaultEntrypointId))
        {
            throw new ArgumentException("Default entrypoint is expected.", nameof(entrypoints));
        }

        defaultEntrypoint = entrypoints.First(kvp => kvp.Value == defaultEntrypointId).Key;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<string> Entrypoints => entrypoints.Keys.ToList();

    /// <inheritdoc/>
    public string DefaultEntrypoint
    {
        get => defaultEntrypoint;
        set
        {
            if (!entrypoints.ContainsKey(value))
            {
                throw new ArgumentException($"Cannot change the default entrypoint to unknown entrypoint '{value}'");
            }

            defaultEntrypoint = value;
            defaultEntrypointId = entrypoints[defaultEntrypoint];
        }
    }

    /// <inheritdoc/>
    public int this[string entrypoint] =>
        entrypoints.TryGetValue(entrypoint, out var entrypointId) 
            ? entrypointId 
            : defaultEntrypointId;
}
