using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace DOPA.Loader;

/// <summary>
/// Collection of ways to load opa.
/// </summary>
public static class OpaLoader
{
    /// <summary>
    /// Creates an OPA WebAssembly module from a rego file.
    /// </summary>
    /// <param name="filePath">Path to the rego file.</param>
    /// <param name="entrypoints">At least one entrypoint.</param>
    /// <returns>A stream with the WebAssembly module.</returns>
    public static Task<Stream> CreateWebAssemblyModuleAsync(string filePath, params string[] entrypoints) =>
        CreateWebAssemblyModuleAsync(new OpaArguments(new[] { filePath }, entrypoints));

    /// <summary>
    /// Creates a WebAssembly module.
    /// </summary>
    /// <param name="arguments">Arguments for OPA.</param>
    /// <returns>A stream with the WebAssembly module.</returns>
    public static async Task<Stream> CreateWebAssemblyModuleAsync(OpaArguments arguments)
    {

        using var e = new OpaExecutable();

        var opaBundlePath = await e.BuildAsync(arguments);

        return ExtractWebAssemblyFromOpaBundle(opaBundlePath);
    }

    /// <summary>
    /// Creates an OPA WebAssembly module from a rego file.
    /// </summary>
    /// <param name="filePath">Path to the rego file.</param>
    /// <param name="entrypoints">At least one entrypoint.</param>
    /// <returns>A stream with the WebAssembly module.</returns>
    public static Stream CreateWebAssemblyModule(string filePath, params string[] entrypoints) =>
        CreateWebAssemblyModule(new OpaArguments(new[] { filePath }, entrypoints));

    /// <summary>
    /// Creates a WebAssembly module.
    /// </summary>
    /// <param name="arguments">Arguments for OPA.</param>
    /// <returns>A stream with the WebAssembly module.</returns>
    public static Stream CreateWebAssemblyModule(OpaArguments arguments)
    {

        using var e = new OpaExecutable();

        var opaBundlePath = e.Build(arguments);

        return ExtractWebAssemblyFromOpaBundle(opaBundlePath);
    }

    /// <summary>
    /// Opens an OPA bundle and extracts the policy.wasm.
    /// </summary>
    /// <param name="opaBundlePath">Path to the bundle.tar.gz.</param>
    /// <returns>A <see cref="Stream" /> with the wasm contents.</returns>
    public static Stream ExtractWebAssemblyFromOpaBundle(string opaBundlePath)
    {
        using var fileStream = File.OpenRead(opaBundlePath);
        using var unzippedStream = new GZipInputStream(fileStream);
        using var tarStream = new TarInputStream(unzippedStream, System.Text.Encoding.UTF8);

        var entry = tarStream.GetNextEntry();

        while (entry is not null)
        {
            if (entry.Name.EndsWith("policy.wasm", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            entry = tarStream.GetNextEntry();
        }

        if (entry is null)
        {
            tarStream.Close();
            tarStream.Dispose();
            throw new Exception("Could not find policy.wasm in the loaded bundle.");
        }

        var ms = new MemoryStream();
        tarStream.CopyTo(ms);
        ms.Position = 0;

        return ms;
    }
}
