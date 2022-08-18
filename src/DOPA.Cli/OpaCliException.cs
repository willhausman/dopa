namespace DOPA.Cli;

public class OpaCliException : InvalidOperationException
{
    public OpaCliException(string executableFilePath, string arguments, string details)
        : base($@"Failed to run opa executable\nExecutable: {executableFilePath}\nArgs: {arguments}\n\nDetails:{details}")
    {
        ExecutableFilePath = executableFilePath;
        Arguments = arguments;
        Details = details;
    }

    public string ExecutableFilePath { get; }

    public string Arguments { get; }

    public string Details { get; }
}