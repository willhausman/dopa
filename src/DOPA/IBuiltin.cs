namespace DOPA;

/// <summary>
/// Represents a named delegate that can be invoked from an OPA custom builtin.
/// </summary>
public interface IBuiltin
{
    /// <summary>
    /// The name of the delegate registered as a builtin.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The number of expected arguments to call with Invoke.
    /// </summary>
    int NumberOfArguments { get; }

    /// <summary>
    /// Invokes the delegate.
    /// </summary>
    /// <param name="argAddresses">Passed in arguments as int addresses to data in the OPA shared memory.</param>
    /// <returns>The address of a result written to the shared memory.</returns>
    int Invoke(params int[] argAddresses);       
}
