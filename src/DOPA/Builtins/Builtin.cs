using DOPA.Serialization;

namespace DOPA.Builtins;

/// <inheritdoc />
public abstract class Builtin : IBuiltin
{
    private readonly IOpaRuntime runtime;
    private readonly IOpaSerializer serializer;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="name">The name of the delegate.</param>
    /// <param name="runtime">The runtime.</param>
    /// <param name="serializer">The serializer.</param>
    protected Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer)
    {
        Name = name;
        this.runtime = runtime;
        this.serializer = serializer;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public abstract int NumberOfArguments { get; }

    /// <inheritdoc />
    public int Invoke(params int[] argAddresses)
    {
        if (argAddresses.Length != NumberOfArguments)
        {
            throw new ArgumentException($"'{Name}' builtin called with '{argAddresses.Length}' parameters. Expected '{NumberOfArguments}'.");
        }

        return InvokeCore(argAddresses);
    }

    /// <summary>
    /// Invoke the delegate.
    /// </summary>
    /// <param name="argAddresses">Addresses of args from the runtime.</param>
    /// <returns>The address of data written back to the runtime.</returns>
    protected abstract int InvokeCore(params int[] argAddresses);

    /// <summary>
    /// Get a deserialized arg from the runtime.
    /// </summary>
    /// <param name="address">The address of the arg.</param>
    /// <typeparam name="T">The type of the arg.</typeparam>
    /// <returns>The value of the arg.</returns>
    protected T? GetArg<T>(int address) =>
        serializer.Deserialize<T>(runtime.ReadJson(address));

    /// <summary>
    /// Write the final result back to the runtime.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <returns>The address of data written back to the runtime.</returns>
    protected int Return<T>(T value) =>
        runtime.WriteJson(serializer.Serialize(value));
}

/// <summary>
/// A named delegate with no paramters.
/// </summary>
/// <typeparam name="TResult">The type of the result from the delegate.</typeparam>
public sealed class Builtin<TResult> : Builtin
{
    private readonly Func<TResult> callback;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="name">The name of the delegate.</param>
    /// <param name="runtime">The runtime.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="callback">The delegate.</param>
    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    /// <inheritdoc />
    public override int NumberOfArguments => 0;

    /// <inheritdoc />
    protected override int InvokeCore(params int[] argAddresses) =>
        Return(callback());
}

/// <summary>
/// A named delegate with one parameter.
/// </summary>
/// <typeparam name="TArg">The type of the parameter.</typeparam>
/// <typeparam name="TResult">The type of the result from the delegate.</typeparam>
public sealed class Builtin<TArg, TResult> : Builtin
{
    private readonly Func<TArg, TResult> callback;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="name">The name of the delegate.</param>
    /// <param name="runtime">The runtime.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="callback">The delegate.</param>
    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    /// <inheritdoc />
    public override int NumberOfArguments => 1;

    /// <inheritdoc />
    protected override int InvokeCore(params int[] argAddresses)
    {
        var result = callback(GetArg<TArg>(argAddresses[0])!);
        return Return(result);
    }
}

/// <summary>
/// A named delegate with two parameters.
/// </summary>
/// <typeparam name="TArg1">The type of the first parameter.</typeparam>
/// <typeparam name="TArg2">The type of the second parameter.</typeparam>
/// <typeparam name="TResult">The type of the result from the delegate.</typeparam>
public sealed class Builtin<TArg1, TArg2, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TResult> callback;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="name">The name of the delegate.</param>
    /// <param name="runtime">The runtime.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="callback">The delegate.</param>
    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    /// <inheritdoc />
    public override int NumberOfArguments => 2;

    /// <inheritdoc />
    protected override int InvokeCore(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!);
        return Return(result);
    }
}

/// <summary>
/// A named delegate with three parameters.
/// </summary>
/// <typeparam name="TArg1">The type of the first parameter.</typeparam>
/// <typeparam name="TArg2">The type of the second parameter.</typeparam>
/// <typeparam name="TArg3">The type of the third parameter.</typeparam>
/// <typeparam name="TResult">The type of the result from the delegate.</typeparam>
public sealed class Builtin<TArg1, TArg2, TArg3, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TArg3, TResult> callback;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="name">The name of the delegate.</param>
    /// <param name="runtime">The runtime.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="callback">The delegate.</param>
    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TArg3, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    /// <inheritdoc />
    public override int NumberOfArguments => 3;

    /// <inheritdoc />
    protected override int InvokeCore(params int[] argAddresses)
    {
        var result = callback(
            GetArg<TArg1>(argAddresses[0])!,
            GetArg<TArg2>(argAddresses[1])!,
            GetArg<TArg3>(argAddresses[2])!);
        return Return(result);
    }
}

/// <summary>
/// A named delegate with four parameters.
/// </summary>
/// <typeparam name="TArg1">The type of the first parameter.</typeparam>
/// <typeparam name="TArg2">The type of the second parameter.</typeparam>
/// <typeparam name="TArg3">The type of the third parameter.</typeparam>
/// <typeparam name="TArg4">The type of the fourth parameter.</typeparam>
/// <typeparam name="TResult">The type of the result from the delegate.</typeparam>
public sealed class Builtin<TArg1, TArg2, TArg3, TArg4, TResult> : Builtin
{
    private readonly Func<TArg1, TArg2, TArg3, TArg4, TResult> callback;

    /// <summary>
    /// Initializes the class.
    /// </summary>
    /// <param name="name">The name of the delegate.</param>
    /// <param name="runtime">The runtime.</param>
    /// <param name="serializer">The serializer.</param>
    /// <param name="callback">The delegate.</param>
    public Builtin(string name, IOpaRuntime runtime, IOpaSerializer serializer, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback)
        : base(name, runtime, serializer)
    {
        this.callback = callback;
    }

    /// <inheritdoc />
    public override int NumberOfArguments => 4;

    /// <inheritdoc />
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
