namespace CShopa;

public interface IBuiltinCollection
{
    IDictionary<string, int> BuiltinMap { get; set; }

    bool AddBuiltin(Builtin callback);

    Builtin this[int address] { get; }
}
