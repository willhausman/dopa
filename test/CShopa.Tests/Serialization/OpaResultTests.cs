using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace CShopa.Tests.Serialization;

public class OpaResultTests
{
    [Fact]
    public void CasingConventions()
    {
        var camelJson = @"[{ ""result"": ""value"" }]";

        var pascalOptions = new JsonSerializerOptions { PropertyNamingPolicy = null };
        var camelOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DictionaryKeyPolicy = JsonNamingPolicy.CamelCase };

        DeserializeResult<string>(camelJson, pascalOptions).Should().Be("value");
        DeserializeResult<string>(camelJson, camelOptions).Should().Be("value");
    }

    private T? DeserializeResult<T>(string json, JsonSerializerOptions options)
    {
        var result = JsonSerializer.Deserialize<IEnumerable<IDictionary<string, T>>>(json, options);
        var resultingList = result?.Select(d => d["result"]);
        return resultingList is not null ? resultingList.SingleOrDefault() : default;
    }
}
