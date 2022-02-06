namespace Opa.WebAssembly;

public interface IOpaRuntime : IDisposable
{
    BuiltinCollection Builtins { get; }

    string ReadJson(int address);

    int WriteJson(string json);
}
