namespace CShopa;

public interface IOpaModule : IOpaDisposable
{
    string Name { get; }

    IOpaPolicy CreatePolicy();
}
