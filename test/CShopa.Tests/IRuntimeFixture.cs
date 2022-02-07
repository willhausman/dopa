namespace CShopa.Tests;

public interface IRuntimeFixture : IDisposable
{
    IOpaModule ExampleModule { get; }
}
