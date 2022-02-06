namespace Opa.WebAssembly;

using Serialization;

public class OpaPolicy : Disposable, IOpaPolicy
{
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;

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

    public void AddBuiltin<TResult>(string name, Func<TResult> callback)
    {
        var builtin = new Builtin<TResult>(name, runtime, serializer, callback);
        runtime.Builtins.AddBuiltin(builtin);
    }

    public void AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback)
    {
        var builtin = new Builtin<TArg, TResult>(name, runtime, serializer, callback);
        runtime.Builtins.AddBuiltin(builtin);
    }

    public void AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback)
    {
        var builtin = new Builtin<TArg1, TArg2, TResult>(name, runtime, serializer, callback);
        runtime.Builtins.AddBuiltin(builtin);
    }

    public void AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback)
    {
        var builtin = new Builtin<TArg1, TArg2, TArg3, TResult>(name, runtime, serializer, callback);
        runtime.Builtins.AddBuiltin(builtin);
    }

    public void AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback)
    {
        var builtin = new Builtin<TArg1, TArg2, TArg3, TArg4, TResult>(name, runtime, serializer, callback);
        runtime.Builtins.AddBuiltin(builtin);
    }

    protected override void DisposeManaged()
    {
        this.runtime.Dispose();
    }
}
