using System.Diagnostics;
using System.Security.AccessControl;

namespace DOPA.Loader;

/// <summary>
/// Information needed to run the OPA CLI tool.
/// </summary>
internal class OpaExecutable : IDisposable
{
    private const string windowsFileName = "opa_windows_amd64.exe";
    private const string macFileName = "opa_darwin_amd64";
    private const string linuxFileName = "opa_linux_amd64";

    private static readonly string filePath;
    private bool disposed;
    private List<string> files = new();

    static OpaExecutable()
    {
        var fileName = OperatingSystem.IsWindows() ? windowsFileName :
                   OperatingSystem.IsMacOS() ? macFileName :
                   OperatingSystem.IsLinux() ? linuxFileName :
                   throw new Exception("Unsupported OS detected.");

        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        if (!OperatingSystem.IsWindows())
        {
            var p = Process.Start("chmod", $"755 {filePath}");
            p.WaitForExit();
        }
    }

    public async Task<string> Build(OpaArguments arguments)
    {
        var outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"opa-bundle-{DateTimeOffset.UtcNow.Ticks}.tar.gz");

        var p = Process.Start(filePath, MakeBuildArgs(outputFile, arguments));

        await p.WaitForExitAsync();

        files.Add(outputFile);

        return outputFile;
    }

    private string MakeBuildArgs(string outputFile, OpaArguments arguments)
    {
        var baseArgs = new string[]
        {
            "build",
            "-t wasm",
            $"-o {outputFile}",
        };

        var args = baseArgs
            .Union(arguments.Entrypoints.Select(e => $"-e {e}"))
            .Union(arguments.FilePaths)
            ;

        if (!string.IsNullOrWhiteSpace(arguments.Capabilities))
        {
            args = args.Append($"--capabilities {arguments.Capabilities}");
        }

        return string.Join(' ', args);
    }

    private void DisposeCore()
    {
        if (!disposed)
        {
            foreach (var file in files.Where(f => File.Exists(f)))
            {
                File.Delete(file);
            }

            disposed = true;
        }
    }

    ~OpaExecutable()
    {
        DisposeCore();
    }

    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }
}
