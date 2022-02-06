namespace Opa.WebAssembly;

public interface IOpaRuntime : IDisposable
{
    string ReadJson(int address);

    int WriteJson(string json);

    IDictionary<int, string> GetBuiltins();

    IDictionary<string, int> GetEntrypoints();
}
