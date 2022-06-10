namespace DOPA;

/// <summary>
/// Represents a collection of registered delegates that can be used as custom builtins in OPA.
/// </summary>
public interface IBuiltinCollection
{
    /// <summary>
    /// Maps named builtins to their function ids in OPA. Must be called before OPA can call a delegate.
    /// </summary>
    /// <param name="builtinIds">The map of builtins from OPA.</param>
    /// <returns>True if successfully updated. False if already configured.</returns>
    bool ConfigureBuiltinIds(IDictionary<string, int> builtinIds);

    /// <summary>
    /// The collection of registered builtins.
    /// </summary>
    IReadOnlyCollection<IBuiltin> Builtins { get; }

    /// <summary>
    /// Adds an <see cref="IBuiltin" /> to the collection.
    /// </summary>
    /// <param name="builtin">The implemented builtin.</param>
    /// <returns>True if successfully added, otherwise false.</returns>
    bool AddBuiltin(IBuiltin builtin);

    /// <summary>
    /// Indexes into the collection by builtin id.
    /// </summary>
    IBuiltin this[int builtinId] { get; }
}
