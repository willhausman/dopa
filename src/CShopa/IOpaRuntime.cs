namespace CShopa;

public interface IOpaRuntime : IDisposable
{
    /// <summary>
    /// Evaluates the policy with provided input.
    /// </summary>
    /// <param name="json">The input serialized as json.</param>
    /// <returns>Json response from evaluation.</returns>
    string EvaluateJson(string json);

    /// <summary>
    /// Data to replace the data used in execution.
    /// </summary>
    /// <param name="json">The data serialized as json.</param>
    void SetDataJson(string json);

    /// <summary>
    /// Read data at a known address.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    string ReadJson(int address);

    int WriteJson(string json);

    IDictionary<int, string> GetBuiltins();

    IDictionary<string, int> GetEntrypoints();
}
