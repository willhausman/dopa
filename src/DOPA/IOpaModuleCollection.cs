namespace DOPA;

/// <summary>
/// Collection of loaded modules, indexed by name.
/// </summary>
public interface IOpaModuleCollection
{
    /// <summary>
    /// The collection of modules.
    /// </summary>
    IReadOnlyCollection<IOpaModule> Modules { get; }

    /// <summary>
    /// Indexes into the collection by module name.
    /// </summary>
    IOpaModule this[string name] { get; }
}
