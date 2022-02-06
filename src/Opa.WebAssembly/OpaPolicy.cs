namespace Opa.WebAssembly;

using Serialization;

public class OpaPolicy : Disposable, IOpaPolicy
{
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;

    private BuiltinCollection builtins => runtime.Builtins;

    public OpaPolicy(IOpaRuntime runtime, IOpaSerializer serializer)
    {
        this.runtime = runtime;
        this.serializer = serializer;
    }

    public T Evaluate<T>(object input)
    {
        throw new NotImplementedException();
    }

    public void SetData<T>(T input)
    {
        throw new NotImplementedException();
    }

    public void AddBuiltin<TResult>(string name, Func<TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TResult>(name, runtime, serializer, callback));

    public void AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg, TResult>(name, runtime, serializer, callback));

    public void AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TResult>(name, runtime, serializer, callback));

    public void AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TArg3, TResult>(name, runtime, serializer, callback));

    public void AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback) =>
        builtins.AddBuiltin(new Builtin<TArg1, TArg2, TArg3, TArg4, TResult>(name, runtime, serializer, callback));

    protected override void DisposeManaged()
    {
        this.runtime.Dispose();
    }
}
