namespace CShopa;

public interface IOpaRuntime : IDisposable
{
    /// <summary>
    /// Invokes an OPA function on the runtime.
    /// </summary>
    /// <param name="function">The name of the function to run.</param>
    /// <param name="rest">Any parameters needed for the function.</param>
    /// <typeparam name="T">The expected type to return.</typeparam>
    /// <returns>The result of executing the function.</returns>
    T? Invoke<T>(string function, params object[] rest);

    /// <summary>
    /// Invokes an OPA function on the runtime.
    /// </summary>
    /// <param name="function">The name of the function to run.</param>
    /// <param name="rest">Any parameters needed for the function.</param>
    void Invoke(string function, params object[] rest);

    /// <summary>
    /// Reads json from the shared memory buffer.
    /// </summary>
    /// <param name="address">The address to start reading at.</param>
    /// <returns>The json value in shared memory.</returns>
    string ReadValueAt(int address);

    /// <summary>
    /// Writes json to the shared memory buffer.
    /// </summary>
    /// <param name="address">The address to start writing at.</param>
    /// <param name="json">The json to put into the shared memory.</param>
    void WriteValueAt(int address, string json);
}
