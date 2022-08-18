namespace DOPA.Cli.Commands;

public sealed record BuildCommand
{
    private List<string> Arguments { get; init; }

    public BuildCommand()
    {
        Arguments = new() { "build" };
    }

    public BuildCommand WebAssembly() => this with { Arguments = Arguments.Append("-t wasm").ToList() };

    public BuildCommand Files(params string[] filePaths) => this with { Arguments = Arguments.Union(filePaths).ToList() };

    public BuildCommand Entrypoints(params string[] entrypoints) => this with { Arguments = Arguments.Union(entrypoints.Select(e => $"-e {e}")).ToList() };

    public BuildCommand Capabilities(string capabilities) => this with { Arguments = Arguments.Append($"--capabilities {capabilities}").ToList() };

    public Bundle Execute()
    {
        var outputPath = GetNextBundlePath();
        using var _ = Opa.Execute(Arguments.Append($"-o {outputPath}"));

        return new Bundle(outputPath);
    }

    public async Task<Bundle> ExecuteAsync()
    {
        var outputPath = GetNextBundlePath();
        using var _ = await Opa.ExecuteAsync(Arguments.Append($"-o {outputPath}"));

        return new Bundle(outputPath);
    }

    private string GetNextBundlePath() => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"opa-bundle-{DateTimeOffset.UtcNow.Ticks}.tar.gz");
}