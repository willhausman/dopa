using System.Text;
using WasmerSharp;

namespace DOPA.Wasmer;

public static class MemoryExtensions
{
    public static string ReadNullTerminatedString(this Memory memory, int address)
    {
        var slice = memory.GetSpan().Slice(address);
        var terminator = slice.IndexOf((byte)0);
        if (terminator == -1)
        {
            throw new InvalidOperationException("value is not a null terminated string.");
        }
        return Encoding.UTF8.GetString(slice.Slice(0, terminator));
    }

    public static void WriteString(this Memory memory, int address, string value) =>
        Encoding.UTF8.GetBytes(value, memory.GetSpan().Slice(address));

    private static unsafe Span<byte> GetSpan(this Memory memory) => new((byte*)memory.Data, (int)memory.DataLength);
}
