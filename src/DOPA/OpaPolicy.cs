namespace DOPA;

using DOPA.Builtins;
using Serialization;

/// <summary>
/// An instance of an OPA policy which can be evaluated.
/// </summary>
public sealed class OpaPolicy : Disposable, IOpaPolicy
{
    private const string EmptyInput = "";
    private const string EmptyJson = "\"\"";
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;
    private readonly IBuiltinCollection builtins;
    private readonly IEntrypointCollection entrypoints;

    // the pointers and addresses returned from the OPA assembly are int32.
    private readonly int initialHeapPointer;
    private int executionHeapPointer;
    private int dataAddress;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="runtime">A runtime for evaluating queries.</param>
    /// <param name="serializer">The serializer settings to be used.</param>
    /// <param name="builtins">A collection of builtin delegates, if any.</param>
    /// <param name="entrypoints">A collection of entrypoints to evaluate from.</param>
    public OpaPolicy(IOpaRuntime runtime, IOpaSerializer serializer, IBuiltinCollection builtins,  IEntrypointCollection entrypoints)
    {
        this.runtime = runtime;
        this.serializer = serializer;
        this.builtins = builtins;
        this.entrypoints = entrypoints;

        dataAddress = initialHeapPointer = executionHeapPointer = runtime.GetCurrentHeap();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<string> Entrypoints => entrypoints.Entrypoints;

    /// <inheritdoc />
    public string DefaultEntrypoint
    {
        get => entrypoints.DefaultEntrypoint;
        set => entrypoints.DefaultEntrypoint = value;
    }

    /// <inheritdoc />
    public object? Data { get; private set; }

    /// <inheritdoc />
    public string? DataJson { get; private set; }

    /// <inheritdoc />
    public T? Evaluate<T>() => EvaluateAt<T>(DefaultEntrypoint, EmptyInput, out var _);

    /// <inheritdoc />
    public T? Evaluate<T>(out string responseJson) => EvaluateAt<T>(DefaultEntrypoint, EmptyInput, out responseJson);

    /// <inheritdoc />
    public T? Evaluate<T>(object input) => EvaluateAt<T>(DefaultEntrypoint, input, out var _);

    /// <inheritdoc />
    public T? Evaluate<T>(object input, out string responseJson) => EvaluateAt<T>(DefaultEntrypoint, input, out responseJson);

    /// <inheritdoc />
    public T? EvaluateAt<T>(string entrypoint) => EvaluateAt<T>(entrypoint, EmptyInput, out var _);

    /// <inheritdoc />
    public T? EvaluateAt<T>(string entrypoint, out string responseJson) => EvaluateAt<T>(entrypoint, EmptyInput, out responseJson);

    /// <inheritdoc />
    public T? EvaluateAt<T>(string entrypoint, object input) => EvaluateAt<T>(entrypoint, input, out var _);

    /// <inheritdoc />
    public T? EvaluateAt<T>(string entrypoint, object input, out string responseJson)
    {
        responseJson = EvaluateJsonAt(entrypoint, serializer.Serialize(input));
        var response = serializer.Deserialize<IEnumerable<IDictionary<string, T>>>(responseJson)?.Select(d => d["result"]);
        return response is not null ? response.FirstOrDefault() : default;
    }

    /// <inheritdoc />
    public string EvaluateJson() => EvaluateJsonAt(DefaultEntrypoint, EmptyJson);

    /// <inheritdoc />
    public string EvaluateJson(string json) => EvaluateJsonAt(DefaultEntrypoint, json);

    /// <inheritdoc />
    public string EvaluateJsonAt(string entrypoint) => EvaluateJsonAt(entrypoint, EmptyJson);

    /// <inheritdoc />
    public string EvaluateJsonAt(string entrypoint, string json)
    {
        var entrypointId = entrypoints[entrypoint];
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

    /// <inheritdoc />
    public void SetData<T>(T input)
    {
        SetDataJson(serializer.Serialize(input));
        Data = input;
    }

    /// <inheritdoc />
    public void SetDataJson(string json)
    {
        runtime.ResetHeapTo(initialHeapPointer); // rewind time and start over
        dataAddress = runtime.WriteJson(json);
        executionHeapPointer = runtime.GetCurrentHeap();
        DataJson = json;
    }

    /// <inheritdoc />
    public bool AddBuiltin<TResult>(string name, Func<TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TResult>(name, runtime, serializer, callback));

    /// <inheritdoc />
    public bool AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg, TResult>(name, runtime, serializer, callback));

    /// <inheritdoc />
    public bool AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TResult>(name, runtime, serializer, callback));

    /// <inheritdoc />
    public bool AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TArg3, TResult>(name, runtime, serializer, callback));

    /// <inheritdoc />
    public bool AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TArg3, TArg4, TResult>(name, runtime, serializer, callback));

    /// <inheritdoc />
    protected override void DisposeManaged()
    {
        this.runtime.Dispose();
    }
}
