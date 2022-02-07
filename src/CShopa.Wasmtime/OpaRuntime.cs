using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using CShopa.Extensions;
using CShopa.Serialization;
using Wasmtime;

namespace CShopa.Wasmtime;

internal sealed class OpaRuntime : Disposable, IOpaRuntime
{
    private readonly Store store;

    // memory is structured to re-evaluate from heap pointer following this general structure: | data | input data | <- evaluation starts after input data
    private int initialHeapPointer;
    private int dataAddress;
    private int inputAddress;

    [NotNull]
    private Instance? instance;

    [NotNull]
    private Memory? memory;

    public OpaRuntime(Store store, Memory memory, Linker linker, Module module)
    {
        this.store = store;
        this.memory = memory;

        instance = linker.Instantiate(store, module);
		inputAddress = dataAddress = initialHeapPointer = Invoke<int>(WellKnown.Export.opa_heap_ptr_get);
    }

    public string EvaluateJson(string json)
    {
        var opaReservedParam = 0;
        var jsonFormat = 0;
        var entrypoint = 0;
        memory.WriteString(store, inputAddress, json); // always reset input json

        var address = Invoke<int>(
            WellKnown.Export.opa_eval,
            opaReservedParam,
            entrypoint,
            dataAddress, // address of stored data
            inputAddress, // where we wrote the input json to. This is actually the current execution heap pointer. We're offsetting it
            json.Length, // length of input json to offset the current execution heap pointer
            inputAddress + json.Length, // new heap pointer to start execution
            jsonFormat);

        var result = memory.ReadNullTerminatedString(store, address);

        ReleaseMemory(address);

        return result;
    }

    public string ReadJson(int address) => ReadJson(address, true);

    public int WriteJson(string json)
    {
        var address = ReserveMemory(json.Length);
        memory.WriteString(store, address, json);

        var resultAddress = Invoke<int>(WellKnown.Export.opa_json_parse, address, json.Length);

        ReleaseMemory(address);
        
        return resultAddress != 0 ? resultAddress : throw new ArgumentException("OPA failed to parse the input json.", nameof(json));
    }

    public IDictionary<int, string> GetBuiltins()
    {
        var json = ReadJson(WellKnown.Export.builtins, releaseFunction: false);
        var builtins = JsonSerializer.Deserialize<IDictionary<int, string>>(json, OpaSerializerOptions.Default);

        return builtins is not null ? builtins : new Dictionary<int, string>();
    }

    public IDictionary<string, int> GetEntrypoints()
    {
        var json = ReadJson(WellKnown.Export.entrypoints, releaseFunction: false);
        var entrypoints = JsonSerializer.Deserialize<IDictionary<string, int>>(json, OpaSerializerOptions.Default);

        return entrypoints is not null ? entrypoints : new Dictionary<string, int>();
    }

    public void SetDataJson(string json)
    {
        Invoke(WellKnown.Export.opa_heap_ptr_set, initialHeapPointer); // rewind time and start over
        dataAddress = WriteJson(json);
        inputAddress = Invoke<int>(WellKnown.Export.opa_heap_ptr_get);
    }

    private string ReadJson(string function, bool releaseFunction = true) =>
        ReadJson(Invoke<int>(function), releaseFunction);
    
    private string ReadJson(int address, bool releaseAddress = true)
    {
        var jsonAddress = Invoke<int>(WellKnown.Export.opa_json_dump, address);
        var result = memory.ReadNullTerminatedString(store, jsonAddress);

        ReleaseMemory(jsonAddress);

        if (releaseAddress)
        {
            ReleaseMemory(address);
        }

        return result;
    }

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
            throw new InvalidOperationException($"Could not invoke '{function}' builtin.");
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

    ~OpaRuntime()
    {
        Dispose(false);
    }

    protected override void DisposeManaged()
    {
        this.store.Dispose();
    }

    protected override void DisposeUnmanaged()
    {
        this.instance = null;
        this.memory = null;
    }
}
