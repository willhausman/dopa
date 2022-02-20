namespace CShopa;

using CShopa.Builtins;
using CShopa.Extensions;
using Serialization;

public class OpaPolicy : Disposable, IOpaPolicy
{
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;
    private readonly IBuiltinCollection builtins;
    private readonly IDictionary<string, int> entrypoints;

    // the pointers and addresses returned from the OPA assembly are int32.
    private int initialHeapPointer;
    private int executionHeapPointer;
    private int dataAddress;

    public OpaPolicy(IOpaRuntime runtime, IOpaSerializer serializer, IBuiltinCollection builtins)
    {
        this.runtime = runtime;
        this.serializer = serializer;
        this.builtins = builtins;

        dataAddress = initialHeapPointer = executionHeapPointer = runtime.GetCurrentHeap();

        this.builtins.ConfigureBuiltinIds(GetBuiltins());
        this.entrypoints = GetEntrypoints();
    }

    public IReadOnlyCollection<string> Entrypoints => entrypoints.Keys.ToList();

    public object? Data { get; private set; }

    public string? DataJson { get; private set; }

    public T? Evaluate<T>() => EvaluateAt<T>("", "", out var _);

    public T? Evaluate<T>(out string responseJson) => EvaluateAt<T>("", "", out responseJson);

    public T? Evaluate<T>(object input) => EvaluateAt<T>("", input, out var _);

    public T? Evaluate<T>(object input, out string responseJson) => EvaluateAt<T>("", input, out responseJson);

    public T? EvaluateAt<T>(string entrypoint) => EvaluateAt<T>(entrypoint, "", out var _);

    public T? EvaluateAt<T>(string entrypoint, out string responseJson) => EvaluateAt<T>(entrypoint, "", out responseJson);

    public T? EvaluateAt<T>(string entrypoint, object input) => EvaluateAt<T>(entrypoint, input, out var _);

    public T? EvaluateAt<T>(string entrypoint, object input, out string responseJson)
    {
        responseJson = EvaluateJsonAt(entrypoint, serializer.Serialize(input));
        var response = serializer.Deserialize<IEnumerable<IDictionary<string, T>>>(responseJson)?.Select(d => d["result"]);
        return response is not null ? response.FirstOrDefault() : default;
    }

    public string EvaluateJson() => EvaluateJsonAt("", "\"\"");

    public string EvaluateJson(string json) => EvaluateJsonAt("", json);

    public string EvaluateJsonAt(string entrypoint) => EvaluateJsonAt(entrypoint, "\"\"");

    public string EvaluateJsonAt(string entrypoint, string json)
    {
        var entrypointId = 0;
        entrypoints.TryGetValue(entrypoint, out entrypointId);
        return Evaluate(entrypointId, json);
    }

    private string Evaluate(int entrypoint, string json)
    {
        var opaReservedParam = 0;
        var jsonFormat = 0;

        runtime.ResetHeapTo(executionHeapPointer);
        var inputAddress = runtime.WriteValue(json);

        var address = runtime.Invoke<int>(
            WellKnown.Export.opa_eval,
            opaReservedParam,
            entrypoint,
            dataAddress,
            inputAddress,
            json.Length,
            inputAddress + json.Length,
            jsonFormat);

        var result = runtime.ReadValueAt(address);

        runtime.ReleaseMemory(address, inputAddress);

        return result;
    }

    public void SetData<T>(T input)
    {
        SetDataJson(serializer.Serialize(input));
        Data = input;
    }

    public void SetDataJson(string json)
    {
        runtime.ResetHeapTo(initialHeapPointer); // rewind time and start over
        dataAddress = runtime.WriteJson(json);
        executionHeapPointer = runtime.GetCurrentHeap();
        DataJson = json;
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

    private IDictionary<string, int> GetBuiltins()
    {
        var json = runtime.ReadJson(WellKnown.Export.builtins);
        var builtins = serializer.Deserialize<Dictionary<string, int>>(json) ?? new();

        return builtins.WithCaseInsensitiveKeys();
    }

    private IDictionary<string, int> GetEntrypoints()
    {
        var json = runtime.ReadJson(WellKnown.Export.entrypoints);
        var entrypoints = serializer.Deserialize<Dictionary<string, int>>(json) ?? new();

        return entrypoints.WithCaseInsensitiveKeys();
    }
}
