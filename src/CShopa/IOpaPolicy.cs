namespace CShopa;

public interface IOpaPolicy : IDisposable
{
    T? Evaluate<T>();

    T? Evaluate<T>(out string responseJson);

    T? Evaluate<T>(object input);

    T? Evaluate<T>(object input, out string responseJson);

    T? EvaluateAt<T>(string entrypoint);

    T? EvaluateAt<T>(string entrypoint, object input);

    T? EvaluateAt<T>(string entrypoint, out string responseJson);

    T? EvaluateAt<T>(string entrypoint, object input, out string responseJson);

    string EvaluateJson();

    string EvaluateJson(string json);

    string EvaluateJsonAt(string entrypoint);

    string EvaluateJsonAt(string entrypoint, string json);

    void SetData<T>(T input);

    void SetDataJson(string json);

    bool AddBuiltin<TResult>(string name, Func<TResult> callback);

    bool AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback);

    bool AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback);

    bool AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback);

    bool AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback);
}
