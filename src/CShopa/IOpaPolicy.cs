namespace CShopa;

/// <summary>
/// An instance of an OPA policy which can be evaluated.
/// </summary>
public interface IOpaPolicy : IOpaDisposable
{
    /// <summary>
    /// A representation of the data loaded into the policy.
    /// </summary>
    object? Data { get; }

    /// <summary>
    /// The exact json loaded into the policy as data.
    /// </summary>
    string? DataJson { get; }

    /// <summary>
    /// The possible entrypoints where the policy can be evaluated.
    /// </summary>
    IReadOnlyCollection<string> Entrypoints { get; }

    /// <summary>
    /// The default entrypoint where the policy is evaluated.
    /// </summary>
    string DefaultEntrypoint { get; set; }

    /// <summary>
    /// Evaluates the policy at the default entrypoint.
    /// </summary>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? Evaluate<T>();

    /// <summary>
    /// Evaluates the policy at the default entrypoint.
    /// </summary>
    /// <param name="responseJson">The raw json returned from the policy.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? Evaluate<T>(out string responseJson);

    /// <summary>
    /// Evaluates the policy at the default entrypoint.
    /// </summary>
    /// <param name="input">The input data to evaluate with.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? Evaluate<T>(object input);

    /// <summary>
    /// Evaluates the policy at the default entrypoint.
    /// </summary>
    /// <param name="input">The input data to evaluate with.</param>
    /// <param name="responseJson">The raw json returned from the policy.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? Evaluate<T>(object input, out string responseJson);

    /// <summary>
    /// Evaluates the policy at a specific entrypoint.
    /// </summary>
    /// <param name="entrypoint">The path to an entrypoint to evaluate.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? EvaluateAt<T>(string entrypoint);

    /// <summary>
    /// Evaluates the policy at a specific entrypoint.
    /// </summary>
    /// <param name="entrypoint">The path to an entrypoint to evaluate.</param>
    /// <param name="input">The input data to evaluate with.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? EvaluateAt<T>(string entrypoint, object input);

    /// <summary>
    /// Evaluates the policy at a specific entrypoint.
    /// </summary>
    /// <param name="entrypoint">The path to an entrypoint to evaluate.</param>
    /// <param name="responseJson">The raw json returned from the policy.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? EvaluateAt<T>(string entrypoint, out string responseJson);

    /// <summary>
    /// Evaluates the policy at a specific entrypoint.
    /// </summary>
    /// <param name="entrypoint">The path to an entrypoint to evaluate.</param>
    /// <param name="input">The input data to evaluate with.</param>
    /// <param name="responseJson">The raw json returned from the policy.</param>
    /// <typeparam name="T">The expected result type.</typeparam>
    /// <returns>The policy result.</returns>
    T? EvaluateAt<T>(string entrypoint, object input, out string responseJson);

    /// <summary>
    /// Evaluates the policy at the default entrypoint.
    /// </summary>
    /// <returns>The raw json returned from the policy.</returns>
    string EvaluateJson();

    /// <summary>
    /// Evaluates the policy at the default entrypoint.
    /// </summary>
    /// <param name="json">The input json to evaluate with.</param>
    /// <returns>The raw json returned from the policy.</returns>
    string EvaluateJson(string json);

    /// <summary>
    /// Evaluates the policy at a specific entrypoint.
    /// </summary>
    /// <param name="entrypoint">The path to an entrypoint to evaluate.</param>
    /// <returns>The raw json returned from the policy.</returns>
    string EvaluateJsonAt(string entrypoint);

    /// <summary>
    /// Evaluates the policy at a specific entrypoint.
    /// </summary>
    /// <param name="entrypoint">The path to an entrypoint to evaluate.</param>
    /// <param name="json">The in[ut json to evaluate with.</param>
    /// <returns>The raw json returned from the policy.</returns>
    string EvaluateJsonAt(string entrypoint, string json);

    /// <summary>
    /// Sets reusable data in the policy.
    /// </summary>
    /// <param name="input">The data to set.</param>
    /// <typeparam name="T"></typeparam>
    void SetData<T>(T input);

    /// <summary>
    /// Sets reusable data directly in the policy.
    /// </summary>
    /// <param name="json">The serialized json to set in the policy.</param>
    void SetDataJson(string json);

    /// <summary>
    /// Adds a custom builtin to the collection of possible callbacks.
    /// </summary>
    /// <param name="name">The fully qualified name of the builtin.</param>
    /// <param name="callback">The function to call.</param>
    /// <typeparam name="TResult">The type of the callback's result.</typeparam>
    /// <returns>True if added, otherwise false.</returns>
    bool AddBuiltin<TResult>(string name, Func<TResult> callback);

    /// <summary>
    /// Adds a custom builtin to the collection of possible callbacks.
    /// </summary>
    /// <param name="name">The fully qualified name of the builtin.</param>
    /// <param name="callback">The function to call.</param>
    /// <typeparam name="TArg">The type of the callback's arg.</typeparam>
    /// <typeparam name="TResult">The type of the callback's result.</typeparam>
    /// <returns>True if added, otherwise false.</returns>
    bool AddBuiltin<TArg, TResult>(string name, Func<TArg, TResult> callback);

    /// <summary>
    /// Adds a custom builtin to the collection of possible callbacks.
    /// </summary>
    /// <param name="name">The fully qualified name of the builtin.</param>
    /// <param name="callback">The function to call.</param>
    /// <typeparam name="TArg1">The type of the callback's first arg.</typeparam>
    /// <typeparam name="TArg2">The type of the callback's second arg.</typeparam>
    /// <typeparam name="TResult">The type of the callback's result.</typeparam>
    /// <returns>True if added, otherwise false.</returns>
    bool AddBuiltin<TArg1, TArg2, TResult>(string name, Func<TArg1, TArg2, TResult> callback);

    /// <summary>
    /// Adds a custom builtin to the collection of possible callbacks.
    /// </summary>
    /// <param name="name">The fully qualified name of the builtin.</param>
    /// <param name="callback">The function to call.</param>
    /// <typeparam name="TArg1">The type of the callback's first arg.</typeparam>
    /// <typeparam name="TArg2">The type of the callback's second arg.</typeparam>
    /// <typeparam name="TArg3">The type of the callback's third arg.</typeparam>
    /// <typeparam name="TResult">The type of the callback's result.</typeparam>
    /// <returns>True if added, otherwise false.</returns>
    bool AddBuiltin<TArg1, TArg2, TArg3, TResult>(string name, Func<TArg1, TArg2, TArg3, TResult> callback);

    /// <summary>
    /// Adds a custom builtin to the collection of possible callbacks.
    /// </summary>
    /// <param name="name">The fully qualified name of the builtin.</param>
    /// <param name="callback">The function to call.</param>
    /// <typeparam name="TArg1">The type of the callback's first arg.</typeparam>
    /// <typeparam name="TArg2">The type of the callback's second arg.</typeparam>
    /// <typeparam name="TArg3">The type of the callback's third arg.</typeparam>
    /// <typeparam name="TArg4">The type of the callback's fourth arg.</typeparam>
    /// <typeparam name="TResult">The type of the callback's result.</typeparam>
    /// <returns>True if added, otherwise false.</returns>
    bool AddBuiltin<TArg1, TArg2, TArg3, TArg4, TResult>(string name, Func<TArg1, TArg2, TArg3, TArg4, TResult> callback);
}
