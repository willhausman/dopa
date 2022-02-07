namespace CShopa;
using Serialization;

public class OpaPolicy : Disposable, IOpaPolicy
{
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;
    private readonly IBuiltinCollection builtins;

    // memory is structured to re-evaluate from heap pointer following this general structure: | data | input data | <- evaluation starts after input data
    private int initialHeapPointer;
    private int dataAddress;
    private int inputAddress;

    public OpaPolicy(IOpaRuntime runtime, IOpaSerializer serializer, IBuiltinCollection builtins)
    {
        this.runtime = runtime;
        this.serializer = serializer;
        this.builtins = builtins;

        inputAddress = dataAddress = initialHeapPointer = runtime.Invoke<int>(WellKnown.Export.opa_heap_ptr_get);
        this.builtins.BuiltinMap = GetBuiltins();
    }

    public T? Evaluate<T>(object input) => Evaluate<T>(input, out var _);

    public T? Evaluate<T>(object input, out string responseJson)
    {
        responseJson = EvaluateJson(serializer.Serialize(input));
        var response = serializer.Deserialize<IEnumerable<IDictionary<string, T>>>(responseJson)?.Select(d => d["result"]);
        return response is not null ? response.FirstOrDefault() : default;
    }

    public string EvaluateJson(string json)
    {
        var opaReservedParam = 0;
        var jsonFormat = 0;
        var entrypoint = 0;
        runtime.WriteValueAt(inputAddress, json); // always reset input json

        var address = runtime.Invoke<int>(
            WellKnown.Export.opa_eval,
            opaReservedParam,
            entrypoint,
            dataAddress, // address of stored data
            inputAddress, // where we wrote the input json to. This is actually the current execution heap pointer. We're offsetting it
            json.Length, // length of input json to offset the current execution heap pointer
            inputAddress + json.Length, // new heap pointer to start execution
            jsonFormat);

        var result = runtime.ReadValueAt(address);

        runtime.ReleaseMemory(address);

        return result;
    }

    public void SetData<T>(T input) =>
        SetDataJson(serializer.Serialize(input));

    public void SetDataJson(string json)
    {
        runtime.Invoke(WellKnown.Export.opa_heap_ptr_set, initialHeapPointer); // rewind time and start over
        dataAddress = runtime.WriteJson(json);
        inputAddress = runtime.Invoke<int>(WellKnown.Export.opa_heap_ptr_get);
    }

    public bool AddBuiltin<TResult>(string name, Func<TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TResult>(name, runtime, serializer, callback));

    public bool AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg, TResult>(name, runtime, serializer, callback));

    public bool AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TResult>(name, runtime, serializer, callback));

    public bool AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TArg3, TResult>(name, runtime, serializer, callback));

    public bool AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TArg3, TArg4, TResult>(name, runtime, serializer, callback));

    protected override void DisposeManaged()
    {
        this.runtime.Dispose();
    }

    private IDictionary<int, string> GetBuiltins()
    {
        var json = ReadJson(WellKnown.Export.builtins, releaseFunction: false);
        var builtins = serializer.Deserialize<IDictionary<int, string>>(json);

        return builtins is not null ? builtins : new Dictionary<int, string>();
    }

    private IDictionary<string, int> GetEntrypoints()
    {
        var json = ReadJson(WellKnown.Export.entrypoints, releaseFunction: false);
        var entrypoints = serializer.Deserialize<IDictionary<string, int>>(json);

        return entrypoints is not null ? entrypoints : new Dictionary<string, int>();
    }

    private string ReadJson(string function, bool releaseFunction) =>
        runtime.ReadJson(runtime.Invoke<int>(function), releaseFunction);
}
