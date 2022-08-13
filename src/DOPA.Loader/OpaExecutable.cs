using System.Diagnostics;

namespace DOPA.Loader;

/// <summary>
/// Information needed to run the OPA CLI tool.
/// </summary>
public class OpaExecutable
{
    private const string windowsFileName = "opa_windows_amd64.exe";
    private const string macFileName = "opa_darwin_amd64";
    private const string linuxFileName = "opa_linux_amd64";

    private static readonly string fileName;

    static OpaExecutable()
    {
        fileName = OperatingSystem.IsWindows() ? windowsFileName :
                   OperatingSystem.IsMacOS() ? macFileName :
                   OperatingSystem.IsLinux() ? linuxFileName :
                   throw new Exception("Unsupported OS detected.");
    }

    /// <summary>
    /// Build an OPA bundle.
    /// </summary>
    /// <returns>The bundle.</returns>
    public async Task<string> Build(string rego, params string[] entrypoints)
    {
        var outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{rego}.tar.gz");

        var baseArgs = new string[]
        {
            "build",
            "-t wasm",
            $"-o {outputFile}",
            rego
        };

        var args = baseArgs.Union(entrypoints.Select(e => $"-e {e}"));
        var argString = string.Join(' ', args);

        var p = Process.Start(fileName, argString);

        await p.WaitForExitAsync();

        return outputFile;
    }
}
