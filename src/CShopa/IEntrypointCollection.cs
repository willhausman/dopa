namespace CShopa;

public interface IEntrypointCollection
{
    /// <summary>
    /// The known entrypoints in this collection.
    /// </summary>
    IReadOnlyCollection<string> Entrypoints { get; }

    /// <summary>
    /// Indexes into the collection by name and returns the opa entrypoint id, or the default.
    /// </summary>
    int this[string entrypoint] { get; }

    /// <summary>
    /// The default entrypoint where the policy is evaluated.
    /// </summary>
    string DefaultEntrypoint { get; set; }
}
