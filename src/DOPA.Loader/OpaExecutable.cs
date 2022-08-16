using System.Diagnostics;

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
    private List<string> createdFiles = new();

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

    public async Task<string> BuildAsync(OpaArguments arguments)
    {
        var (p, bundleFilePath) = StartBuildProcess(arguments);
        await p.WaitForExitAsync();
        return bundleFilePath;
    }

    public string Build(OpaArguments arguments)
    {
        var (p, bundleFilePath) = StartBuildProcess(arguments);
        p.WaitForExit();
        return bundleFilePath;
    }

    private (Process opaProcess, string bundleFilePath) StartBuildProcess(OpaArguments arguments)
    {
        var bundleFilePath = GetNextBundlePath();

        var p = Process.Start(filePath, MakeBuildArgs(bundleFilePath, arguments));

        createdFiles.Add(bundleFilePath);

        return (p, bundleFilePath);
    }

    private string GetNextBundlePath() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"opa-bundle-{DateTimeOffset.UtcNow.Ticks}.tar.gz");

    private string MakeBuildArgs(string outputFile, OpaArguments arguments)
    {
        var args = new string[]
        {
            "build",
            "-t wasm",
            $"-o {outputFile}",
        }.Union(arguments.GetFormattedArguments());

        return string.Join(' ', args);
    }

    private void DisposeCore()
    {
        if (!disposed)
        {
            foreach (var file in createdFiles.Where(f => File.Exists(f)))
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
