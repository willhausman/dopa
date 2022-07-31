namespace DOPA;

/// <summary>
/// A WebAssembly module that can create runtimes.
/// </summary>
public interface IWasmModule : IOpaDisposable
{
    /// <summary>
    /// A name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Create a runtime to interact with the module.
    /// </summary>
    /// <param name="collection">A collection of custom builtins.</param>
    /// <returns>An <see cref="IOpaRuntime" />.</returns>
    IOpaRuntime CreateRuntime(IBuiltinCollection collection);

    /// <summary>
    /// The exports available in this module.
    /// </summary>
    ICollection<string> Exports { get; }
}
