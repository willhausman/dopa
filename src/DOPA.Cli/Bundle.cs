using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace DOPA.Cli;

public class Bundle : Disposable
{
    public Bundle(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }

    protected override void DisposeUnmanaged()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
    }

    public Stream ExtractWebAssemblyModule() => ExtractFile("policy.wasm");

    public Stream ExtractData() => ExtractFile("data.json");

    public Stream ExtractFile(string archivePath)
    {
        using var fileStream = File.OpenRead(FilePath);
        using var unzippedStream = new GZipInputStream(fileStream);
        using var tarStream = new TarInputStream(unzippedStream, System.Text.Encoding.UTF8);

        var entry = tarStream.GetNextEntry();

        while (entry is not null)
        {
            if (entry.Name.EndsWith(archivePath, StringComparison.OrdinalIgnoreCase))
            {
                var ms = new MemoryStream();
                tarStream.CopyTo(ms);
                ms.Position = 0;

                return ms;
            }

            entry = tarStream.GetNextEntry();
        }

        throw new ArgumentException($"Could not find '{archivePath}' in the bundle.");
    }

    ~Bundle() => Dispose(disposing: false);
}