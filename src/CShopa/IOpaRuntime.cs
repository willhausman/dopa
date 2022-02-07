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

    /// <summary>
    /// Converts a value to json and reads the result.
    /// </summary>
    /// <param name="address">The address of a value that needs to be converted to json.</param>
    /// <param name="releaseAddress">Whether or not to free shared memory after reading the value. Default is true.</param>
    /// <returns>The json stored in the shared memory buffer.</returns>
    string ReadJson(int address, bool releaseAddress = true);

    /// <summary>
    /// Allocates memory for json and writes it into the shared memory.
    /// </summary>
    /// <param name="json">The json to write into shared memory.</param>
    /// <returns>The address pointing to the json in shared memory.</returns>
    int WriteJson(string json);

    /// <summary>
    /// Release memory back to the buffer.
    /// </summary>
    /// <param name="addresses">Pointers to memory that can be released.</param>
    void ReleaseMemory(params int[] addresses);

    /// <summary>
    /// Reserve memory for writing a value.
    /// </summary>
    /// <param name="length">The length of the value to write.</param>
    /// <returns>The pointer to the allocated memory.</returns>
    public int ReserveMemory(int length);
}
