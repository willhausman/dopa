using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Opa.WebAssembly.Exceptions;
using Opa.WebAssembly.Extensions;
using Opa.WebAssembly.Serialization;
using Wasmtime;

namespace Opa.WebAssembly.Wasmtime;

internal sealed class Runtime : Disposable, IOpaRuntime
{
    private readonly Store store;
    private readonly Linker linker;

    [NotNull]
    private Instance? instance;

    [NotNull]
    private Memory? memory;

    public Runtime(Engine engine, Module module)
    {
        store = new Store(engine);
        linker = new Linker(engine);
        memory = new Memory(store);

        Link();

        instance = linker.Instantiate(store, module);

        Builtins = new BuiltinCollection(GetBuiltins());
    }

    public BuiltinCollection Builtins { get; }

    public string ReadJson(int address)
    {
        var jsonAddress = Invoke<int>(WellKnown.Export.opa_json_dump, address);
        var result = memory.ReadNullTerminatedString(store, jsonAddress);

        ReleaseMemory(address, jsonAddress);

        return result;
    }

    public int WriteJson(string json)
    {
        var address = ReserveMemory(json.Length);
        memory.WriteString(store, address, json);

        var resultAddress = Invoke<int>(WellKnown.Export.opa_json_parse, json.Length);

        ReleaseMemory(address);
        
        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    private string ReadJson(string function) =>
        ReadJson(Invoke<int>(function));

    private void ReleaseMemory(params int[] addresses) =>
        addresses.ForEach(a => Invoke(WellKnown.Export.opa_free, a));
    
    private int ReserveMemory(int length) =>
        Invoke<int>(WellKnown.Export.opa_malloc, length);

    private void Invoke(string function, params object[] rest)
    {
        var run = instance.GetFunction(store, function);

        if (run is Function)
        {
            run.Invoke(store, rest);
        }
        else
        {
            throw new InvalidOperationException($"Could not invoke '{function}'");
        }
    }

    private T? Invoke<T>(string function, params object[] rest)
    {
        var run = instance.GetFunction(store, function);

        if (run is Function)
        {
            return (T?)run.Invoke(store, rest);
        }

        throw new InvalidOperationException($"Could not invoke '{function}'");
    }

    private IDictionary<int, string> GetBuiltins()
    {
        var json = ReadJson(WellKnown.Export.builtins);
        var builtins = JsonSerializer.Deserialize<IDictionary<int, string>>(json, OpaSerializerOptions.Default);

        return builtins is not null ? builtins : new Dictionary<int, string>();
    }

    private void Link()
    {
        LinkGlobalImports();
        LinkBuiltins();
    }

    private void LinkGlobalImports()
    {
        Define(WellKnown.Imports.memory, memory);

        Define(
            WellKnown.Imports.opa_abort,
            Function.FromCallback(
                store,
                (Caller caller, int address) =>
                    throw OpaAbortException.Because(memory.ReadNullTerminatedString(store, address))));
    }

    private void LinkBuiltins()
    {
        Define(WellKnown.Imports.opa_builtin0, Function.FromCallback(store, (Caller caller, int builtinId, int _) => Builtins[builtinId].Invoke()));
        Define(WellKnown.Imports.opa_builtin1, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1) => Builtins[builtinId].Invoke(address1)));
        Define(WellKnown.Imports.opa_builtin2, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1, int address2) => Builtins[builtinId].Invoke(address1, address2)));
        Define(WellKnown.Imports.opa_builtin3, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1, int address2, int address3) => Builtins[builtinId].Invoke(address1, address2, address3)));
        Define(WellKnown.Imports.opa_builtin4, Function.FromCallback(store, (Caller caller, int builtinId, int _, int address1, int address2, int address3, int address4) => Builtins[builtinId].Invoke(address1, address2, address3, address4)));
    }

    private void Define(string name, object item) => linker.Define(WellKnown.Imports.Namespace, name, item);

    ~Runtime()
    {
        Dispose(false);
    }

    protected override void DisposeManaged()
    {
        this.store.Dispose();
        this.linker.Dispose();
    }

    protected override void DisposeUnmanaged()
    {
        this.instance = null;
        this.memory = null;
    }
}
