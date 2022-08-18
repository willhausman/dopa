using System.Diagnostics;
using DOPA.Cli.Commands;

namespace DOPA.Cli;

public static class Opa
{
    private const string windowsFileName = "opa_windows_amd64.exe";
    private const string macFileName = "opa_darwin_amd64";
    private const string linuxFileName = "opa_linux_amd64";
    private const int OK = 0;
    private static readonly string opaPath;

    static Opa()
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

    public static BuildCommand Build => new BuildCommand();

    internal static Process Execute(IEnumerable<string> arguments)
    {
        var args = string.Join(' ', arguments);
        var p = StartProcess(args);
        p.WaitForExit();

        if (p.ExitCode != OK)
        {
            var details = p.StandardOutput.ReadToEnd();
            throw new OpaCliException(opaPath, args, details);
        }

        return p;
    }

    internal static async Task<Process> ExecuteAsync(IEnumerable<string> arguments)
    {
        var args = string.Join(' ', arguments);
        var p = StartProcess(args);
        await p.WaitForExitAsync();

        if (p.ExitCode != OK)
        {
            var details = await p.StandardOutput.ReadToEndAsync();
            throw new OpaCliException(opaPath, args, details);
        }

        return p;
    }

    private static Process StartProcess(string args)
    {
        var p = new Process
        {
            StartInfo = new(opaPath)
            {
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            },
        };

        p.Start();

        return p;
    }
}
