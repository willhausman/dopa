namespace DOPA.Loader;

/// <summary>
/// A collection of arguments to be used with the opa cli.
/// </summary>
public record OpaArguments
{
    /// <summary>
    /// Initializes the record.
    /// </summary>
    /// <param name="filePaths">The paths to a rego or directory of regos.</param>
    /// <param name="entrypoints">The entrypoints to expose.</param>
    public OpaArguments(IEnumerable<string> filePaths, IEnumerable<string> entrypoints)
    {
        ArgumentNullException.ThrowIfNull(filePaths);
        ArgumentNullException.ThrowIfNull(entrypoints);

        FilePaths = filePaths.ToList();
        Entrypoints = entrypoints.ToList();

        if (!FilePaths.Any())
        {
            throw new ArgumentException("At least one file path is required.", nameof(filePaths));
        }

        if (!Entrypoints.Any())
        {
            throw new ArgumentException("At least one entrypoint is required.", nameof(entrypoints));
        }
    }

    /// <summary>
    /// Initializes the record.
    /// </summary>
    /// <param name="filePath">The path to a rego or directory of regos.</param>
    /// <param name="entrypoints">The entrypoints to expose.</param>
    public OpaArguments(string filePath, params string[] entrypoints)
        : this(new[] { filePath }, entrypoints)
    {
    }
    
    // TODO: Data.json files as well, I assume?
    /// <summary>
    /// The paths to a rego or directory of regos to bundle into a wasm.
    /// </summary>
    public IReadOnlyCollection<string> FilePaths { get; init; }

    /// <summary>
    /// The exposed entrypoints in the resulting wasm.
    /// </summary>
    /// <remarks>
    /// Each entrypoint is a package/resource/path. e.g. `example/hello`
    /// </remarks>
    public IReadOnlyCollection<string> Entrypoints { get; init; }

    /// <summary>
    /// Path or version for capabilities.json.
    /// </summary>
    public string? Capabilities { get; init; }
}
