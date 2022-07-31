using DOPA.Runtime;
using Wasmtime;

namespace DOPA;

/// <summary>
/// A WebAssembly module that can create <see cref="OpaRuntime"/>s with Wasmtime.
/// </summary>
public class WasmModule : Disposable, IWasmModule
{
    private readonly Engine engine;
    private readonly Module module;

    private WasmModule(Engine engine, Module module, string name)
    {
        this.engine = engine;
        this.module = module;
        Name = name;
        Exports = module.Exports.Select(e => e.Name).ToHashSet();
    }

    /// <summary>
    /// The name of this module.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The exports in this module.
    /// </summary>
    public ICollection<string> Exports { get; }

    /// <inheritdoc />
    public IOpaRuntime CreateRuntime(IBuiltinCollection collection)
    {
        using var linker = new Linker(engine);
        var store = new Store(engine);
        var memory = new Memory(store, WellKnown.Requirements.MinimumMemorySize);
        
        linker.LinkForOpa(store, memory, collection);
        var instance = linker.Instantiate(store, module);

        return new OpaRuntime(store, memory, instance);
    }

    /// <summary>
    /// Factory method to create a module.
    /// </summary>
    /// <param name="filePath">The path to a compiled .wasm file.</param>
    /// <returns>An instance of <see cref="IOpaModule" />.</returns>
    public static IOpaModule FromFile(string filePath) => FromFile(filePath, filePath);

    /// <summary>
    /// Factory method to create a module.
    /// </summary>
    /// <param name="name">The name of the module.</param>
    /// <param name="filePath">The path to a compiled .wasm file.</param>
    /// <returns>An instance of <see cref="IOpaModule" />.</returns>
    public static IOpaModule FromFile(string name, string filePath)
    {
        var engine = new Engine();
        var module = new WasmModule(engine, Module.FromFile(engine, filePath), name);
        return new OpaModule(module);
    }

    /// <summary>
    /// Factory method to create a module.
    /// </summary>
    /// <param name="name">The name of the module.</param>
    /// <param name="stream">A <see cref="Stream" /> with the .wasm contents.</param>
    /// <returns>An instance of <see cref="IOpaModule" />.</returns>
    public static IOpaModule FromStream(string name, Stream stream)
    {
        var engine = new Engine();
        var module = new WasmModule(engine, Module.FromStream(engine, name, stream), name);
        return new OpaModule(module);
    }

    /// <summary>
    /// Factory method to create a module.
    /// </summary>
    /// <param name="name">The name of the module.</param>
    /// <param name="content">The .wasm contents.</param>
    /// <returns>An instance of <see cref="IOpaModule" />.</returns>
    public static IOpaModule FromBytes(string name, byte[] content)
    {
        var engine = new Engine();
        var module = new WasmModule(engine, Module.FromBytes(engine, name, content), name);
        return new OpaModule(module);
    }

    /// <inheritdoc />
    protected override void DisposeManaged()
    {
        module.Dispose();
        engine.Dispose();
    }
}
