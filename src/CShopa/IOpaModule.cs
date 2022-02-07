namespace CShopa;

public interface IOpaModule : IDisposable
{
    IOpaPolicy CreatePolicy();
}
