namespace CShopa;

public interface IOpaModule : IDisposable
{
    string Name { get; }

    IOpaPolicy CreatePolicy();
}
