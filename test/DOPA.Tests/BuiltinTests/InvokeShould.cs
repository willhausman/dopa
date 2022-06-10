using DOPA.Builtins;
using DOPA.Serialization;
using FluentAssertions;
using Moq;
using Xunit;

namespace DOPA.Tests.BuiltinTests;

public class InvokeShould
{
    private static readonly IOpaSerializer Serializer = new OpaSerializer();

    [Fact]
    public void GetAndReturnArgsFromRuntime()
    {
        var runtime = new Mock<IOpaRuntime>();
        runtime.Setup(r => r.ReadJson(It.IsAny<int>())).Returns("1");
        runtime.Setup(r => r.WriteJson(It.IsAny<string>())).Returns<string>(x => int.Parse(x));
        var builtins = new IBuiltin[]
        {
            new Builtin<int>("increment", runtime.Object, Serializer, () => 1),
            new Builtin<int, int>("increment", runtime.Object, Serializer, (int x) => x + 1),
            new Builtin<int, int, int>("increment", runtime.Object, Serializer, (int x, int y) => x + y + 1),
            new Builtin<int, int, int, int>("increment", runtime.Object, Serializer, (int x, int y, int z) => x + y + z + 1),
            new Builtin<int, int, int, int, int>("increment", runtime.Object, Serializer, (int x, int y, int z, int a) => x + y + z + a + 1),
        };
        
        for (var i = 0; i < builtins.Length; i++)
        {
            var @params = new int[i]; // since the mock always returns 1, don't really need to specify an input arg
            builtins[i].Invoke(@params).Should().Be(i + 1);
        }
    }

    [Fact]
    public void ValidateNumberOfParameters()
    {
        var runtime = new Mock<IOpaRuntime>();
        runtime.Setup(r => r.ReadJson(It.IsAny<int>())).Returns("1");
        runtime.Setup(r => r.WriteJson(It.IsAny<string>())).Returns<string>(x => int.Parse(x));
        var builtins = new IBuiltin[]
        {
            new Builtin<int>("increment", runtime.Object, Serializer, () => 1),
            new Builtin<int, int>("increment", runtime.Object, Serializer, (int x) => x + 1),
            new Builtin<int, int, int>("increment", runtime.Object, Serializer, (int x, int y) => x + y + 1),
            new Builtin<int, int, int, int>("increment", runtime.Object, Serializer, (int x, int y, int z) => x + y + z + 1),
            new Builtin<int, int, int, int, int>("increment", runtime.Object, Serializer, (int x, int y, int z, int a) => x + y + z + a + 1),
        };

        for (var i = 0; i < builtins.Length; i++)
        {
            var @params = new int[i + 1];
            Action act = () => builtins[i].Invoke(@params);
            act.Should().Throw<ArgumentException>();
        }
    }
}
