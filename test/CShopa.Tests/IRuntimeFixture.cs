namespace CShopa.Tests;

public interface IRuntimeFixture : IDisposable
{
    IOpaModule ExampleModule { get; }

    IOpaModule BuiltinsModule { get; }
}
