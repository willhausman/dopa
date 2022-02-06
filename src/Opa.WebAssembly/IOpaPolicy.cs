namespace Opa.WebAssembly;

public interface IOpaPolicy : IDisposable
{
    T? Evaluate<T>(object input);

    void SetData<T>(T input);

    void AddBuiltin<TResult>(string name, Func<TResult> callback);

    void AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback);

    void AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback);

    void AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback);

    void AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback);
}
