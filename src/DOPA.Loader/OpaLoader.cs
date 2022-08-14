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
    public static Task<Stream> CreateWebAssemblyStream(string filePath, params string[] entrypoints) =>
        CreateWebAssemblyStream(new OpaArguments(new[] { filePath }, entrypoints));

    /// <summary>
    /// Creates a WebAssembly module.
    /// </summary>
    /// <param name="arguments">Arguments for OPA.</param>
    /// <returns>A stream with the WebAssembly module.</returns>
    public static async Task<Stream> CreateWebAssemblyStream(OpaArguments arguments)
    {

        using var e = new OpaExecutable();

        var opaBundle = await e.Build(arguments);

        return CreateStream(opaBundle);
    }

    private static Stream CreateStream(string wasmFilePath)
    {
        using var fileStream = File.OpenRead(wasmFilePath);
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
