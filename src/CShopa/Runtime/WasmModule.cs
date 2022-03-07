using Wasmtime;

namespace CShopa.Runtime;

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

    public string Name { get; }

    public ICollection<string> Exports { get; }

    public IOpaRuntime CreateRuntime(IBuiltinCollection collection)
    {
        using var linker = new Linker(engine);
        var store = new Store(engine);
        var memory = new Memory(store, WellKnown.Requirements.MinimumMemorySize);
        
        linker.LinkForOpa(store, memory, collection);

        return new OpaRuntime(store, memory, linker, module);
    }

    public static IOpaModule FromFile(string filePath) => FromFile(filePath, filePath);

    public static IOpaModule FromFile(string name, string filePath)
    {
        var engine = new Engine();
        var module = new WasmModule(engine, Module.FromFile(engine, filePath), name);
        return new OpaModule(module);
    }

    public static IOpaModule FromStream(string name, Stream stream)
    {
        var engine = new Engine();
        var module = new WasmModule(engine, Module.FromStream(engine, name, stream), name);
        return new OpaModule(module);
    }

    public static IOpaModule FromBytes(string name, byte[] content)
    {
        var engine = new Engine();
        var module = new WasmModule(engine, Module.FromBytes(engine, name, content), name);
        return new OpaModule(module);
    }

    protected override void DisposeManaged()
    {
        module.Dispose();
        engine.Dispose();
    }
}
