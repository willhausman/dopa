using DOPA.Serialization;

namespace DOPA;

/// <summary>
/// A WebAssembly module loaded to create OPA policies.
/// </summary>
public interface IOpaModule : IOpaDisposable
{
    /// <summary>
    /// A name for this module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The serializer to be used by any policies created from this module.
    /// </summary>
    IOpaSerializer Serializer { get; set; }

    /// <summary>
    /// Create an instance of <see cref="IOpaPolicy" />.
    /// </summary>
    /// <returns>An <see cref="IOpaPolicy"/>.</returns>
    IOpaPolicy CreatePolicy();
}
