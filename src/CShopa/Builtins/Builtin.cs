using CShopa.Serialization;

namespace CShopa.Builtins;

public abstract class Builtin : IBuiltin
{
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer)
    {
        Name = name;
        this.runtime = runtime;
        this.serializer = serializer;
    }

    public string Name { get; }

    public abstract int NumberOfArguments { get; }

    public int Invoke(params int[] argAddresses)
    {
        if (argAddresses.Length != NumberOfArguments)
        {
            throw new ArgumentException($"'{Name}' builtin called with '{argAddresses.Length}' parameters. Expected '{NumberOfArguments}'.");
        }

        return InvokeCore(argAddresses);
    }

    protected abstract int InvokeCore(params int[] argAddresses);

    protected T? GetArg<T>(int address) =>
        serializer.Deserialize<T>(runtime.ReadJson(address));

    protected int Return<T>(T value) =>
        runtime.WriteJson(serializer.Serialize(value));
}

public sealed class Builtin<TResult> : Builtin
{
    private readonly Func<TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int NumberOfArguments => 0;

    protected override int InvokeCore(params int[] argAddresses) =>
        Return(callback());
}

public sealed class Builtin<TArg, TResult> : Builtin
{
    private readonly Func<TArg, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int NumberOfArguments => 1;

    protected override int InvokeCore(params int[] argAddresses)
    {
        var result = callback(GetArg<TArg>(argAddresses[0])!);
        return Return(result);
    }
}

public sealed class Builtin<TArg1, TArg2, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int NumberOfArguments => 2;

    protected override int InvokeCore(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!);
        return Return(result);
    }
}

public sealed class Builtin<TArg1, TArg2, TArg3, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TArg3, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TArg3, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int NumberOfArguments => 3;

    protected override int InvokeCore(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!,
            GetArg<TArg3>(argAddresses[2])!);
        return Return(result);
    }
}

public sealed class Builtin<TArg1, TArg2, TArg3, TArg4, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TArg3, TArg4, TResult> callback;

    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    public override int NumberOfArguments => 4;

    protected override int InvokeCore(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!,
            GetArg<TArg3>(argAddresses[2])!,
            GetArg<TArg4>(argAddresses[3])!);
        return Return(result);
    }
}
