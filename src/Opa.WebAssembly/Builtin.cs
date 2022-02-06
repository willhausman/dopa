using Opa.WebAssembly.Serialization;

namespace Opa.WebAssembly;

public abstract class Builtin
{
    protected readonly IOpaRuntime runtime;
    protected readonly IOpaSerializer serializer;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer)
    {
        Name = name;
        this.runtime = runtime;
        this.serializer = serializer;
    }

    public string Name { get; }

    public abstract int Invoke(params int[] argAddresses);

    protected T? GetArg<T>(int address) =>
        serializer.Deserialize<T>(runtime.ReadJson(address));
    
    protected int Return<T>(T value) =>
        runtime.WriteJson(serializer.Serialize<T>(value));

    protected void ValidateParamsLength(int[] @params, int length)
    {
        if (@params.Length != length)
        {
            throw new ArgumentException($"'{Name}' builtin called with the wrong number of parameters.");
        }
    }
}

public class Builtin<TResult> : Builtin
{
    private readonly Func<TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int Invoke(params int[] argAddresses) =>
        Return(callback());
}

public class Builtin<TArg, TResult> : Builtin
{
    private readonly Func<TArg, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int Invoke(params int[] argAddresses)
    {
        var result = callback(GetArg<TArg>(argAddresses[0])!);
        return Return(result);
    }
}

public class Builtin<TArg1, TArg2, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int Invoke(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!);
        return Return(result);
    }
}

public class Builtin<TArg1, TArg2, TArg3, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TArg3, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TArg3, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int Invoke(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!,
            GetArg<TArg3>(argAddresses[2])!);
        return Return(result);
    }
}

public class Builtin<TArg1, TArg2, TArg3, TArg4, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TArg3, TArg4, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int Invoke(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!,
            GetArg<TArg3>(argAddresses[2])!,
            GetArg<TArg4>(argAddresses[3])!);
        return Return(result);
    }
}
