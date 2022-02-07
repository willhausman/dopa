namespace CShopa;

public interface IBuiltinCollection
{
    IDictionary<int, string> BuiltinMap { get; set; }

    bool AddBuiltin(Builtin callback);

    Builtin this[int address] { get; }
}
