namespace Opa.WebAssembly;

using Serialization;

public class OpaPolicy : Disposable, IOpaPolicy
{
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;

    private readonly IBuiltinCollection builtins;

    public OpaPolicy(IOpaRuntime runtime, IOpaSerializer serializer, IBuiltinCollection builtins)
    {
        this.runtime = runtime;
        this.serializer = serializer;
        this.builtins = builtins;
    }

    public T? Evaluate<T>(object input)
    {
        var responseJson = runtime.EvaluateJson(serializer.Serialize(input));

        return serializer.Deserialize<T>(responseJson);
    }

    public string EvaluateJson(string input) => runtime.EvaluateJson(input);

    public void SetData<T>(T input)
    {
        runtime.SetDataJson(serializer.Serialize(input));
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
}
