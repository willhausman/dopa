using CShopa.Serialization;

namespace CShopa;

public interface IOpaModule : IOpaDisposable
{
    string Name { get; }

    IOpaSerializer Serializer { get; set; }

    IOpaPolicy CreatePolicy();
}
