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

    private static readonly string opaPath;
    private bool disposed;
    private List<string> createdFiles = new();

    static OpaExecutable()
    {
        var fileName = OperatingSystem.IsWindows() ? windowsFileName :
                   OperatingSystem.IsMacOS() ? macFileName :
                   OperatingSystem.IsLinux() ? linuxFileName :
                   throw new Exception("Unsupported OS detected.");

        opaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        if (!OperatingSystem.IsWindows())
        {
            var p = Process.Start("chmod", $"755 {opaPath}");
            p.WaitForExit();
        }
    }

    public async Task<string> BuildAsync(OpaArguments arguments)
    {
        var (p, bundleFilePath) = StartBuildProcess(arguments);
        await p.WaitForExitAsync();

        if (p.ExitCode != 0)
        {
            var message = await p.StandardOutput.ReadToEndAsync();
            throw new InvalidOperationException($"Could not build opa bundle:\n{opaPath}\n{message}");
        }

        p.Close();
        return bundleFilePath;
    }

    public string Build(OpaArguments arguments)
    {
        var (p, bundleFilePath) = StartBuildProcess(arguments);
        p.WaitForExit();

        if (p.ExitCode != 0)
        {
            var message = p.StandardOutput.ReadToEnd();
            throw new InvalidOperationException($"Could not build opa bundle:\n{opaPath}\n{message}");
        }

        p.Close();
        return bundleFilePath;
    }

    private (Process opaProcess, string bundleFilePath) StartBuildProcess(OpaArguments arguments)
    {
        var bundleFilePath = GetNextBundlePath();

        var p = new Process
        {
            StartInfo = new(opaPath)
            {
                Arguments = MakeBuildArgs(bundleFilePath, arguments),
                RedirectStandardOutput = true,
                UseShellExecute = false,
            },
        };

        p.Start();

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
