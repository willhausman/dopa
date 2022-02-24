namespace CShopa.DependencyInjection;

internal class OpaPolicy<TConsumer> : IOpaPolicy<TConsumer>
{
    private readonly IOpaPolicy policy;

    public OpaPolicy(IOpaPolicy policy)
    {
        this.policy = policy;
    }

    public object? Data => policy.Data;

    public string? DataJson => policy.DataJson;

    public IReadOnlyCollection<string> Entrypoints => policy.Entrypoints;

    public string DefaultEntrypoint { get => policy.DefaultEntrypoint; set => policy.DefaultEntrypoint = value; }

    public bool AddBuiltin<TResult>(string name, Func<TResult> callback) =>
        policy.AddBuiltin(name, callback);

    public bool AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback) =>
        policy.AddBuiltin(name, callback);

    public bool AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback) =>
        policy.AddBuiltin(name, callback);

    public bool AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback) =>
        policy.AddBuiltin(name, callback);

    public bool AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback) =>
        policy.AddBuiltin(name, callback);


    public void Dispose() =>
        policy.Dispose();

    public T? Evaluate<T>() =>
        policy.Evaluate<T>();

    public T? Evaluate<T>(out string responseJson) =>
        policy.Evaluate<T>(out responseJson);

    public T? Evaluate<T>(object input) =>
        policy.Evaluate<T>(input);

    public T? Evaluate<T>(object input, out string responseJson) =>
        policy.Evaluate<T>(input, out responseJson);

    public T? EvaluateAt<T>(string entrypoint) =>
        policy.EvaluateAt<T>(entrypoint);

    public T? EvaluateAt<T>(string entrypoint, object input) =>
        policy.EvaluateAt<T>(entrypoint, input);

    public T? EvaluateAt<T>(string entrypoint, out string responseJson) =>
        policy.EvaluateAt<T>(entrypoint, out responseJson);

    public T? EvaluateAt<T>(string entrypoint, object input, out string responseJson) =>
        policy.EvaluateAt<T>(entrypoint, input, out responseJson);

    public string EvaluateJson() =>
        policy.EvaluateJson();

    public string EvaluateJson(string json) =>
        policy.EvaluateJson(json);

    public string EvaluateJsonAt(string entrypoint) =>
        policy.EvaluateJsonAt(entrypoint);

    public string EvaluateJsonAt(string entrypoint, string json) =>
        policy.EvaluateJsonAt(entrypoint, json);

    public void SetData<T>(T input) =>
        policy.SetData(input);

    public void SetDataJson(string json) =>
        policy.SetDataJson(json);
}
