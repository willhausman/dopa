using DOPA.Serialization;

namespace DOPA;

public interface IOpaModule : IOpaDisposable
{
    string Name { get; }

    IOpaSerializer Serializer { get; set; }

    IOpaPolicy CreatePolicy();
}
