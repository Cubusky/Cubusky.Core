using Cubusky.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Xunit;

namespace Cubusky.Tests.Text.Json.Serialization
{
    public abstract class JsonConverterTests<TInput>
    {
        public readonly JsonSerializerOptions Options;

        public abstract TInput Result { get; }
        public abstract string Json { get; }

        public JsonConverterTests(params JsonConverter<TInput>[] converters)
        {
            this.Options = new JsonSerializerOptions();
            foreach (var converter in converters)
            {
                this.Options.Converters.Add(converter);
            }
        }

        [Fact]
        public virtual void Read_ValidJson_ReturnsExpectedResult()
        {
            var result = JsonSerializer.Deserialize<TInput>(Json, Options);
            Assert.Equal(Result, result);
        }

        [Fact]
        public virtual void Write_ValidInput_WritesExpectedJson()
        {
            var json = JsonSerializer.Serialize(Result, Options);
            Assert.Equal(Json, json);
        }
    }

    public class JsonConverterTests : JsonConverterTests<string>
    {
        public override string Result => "123";
        public override string Json => "{\"i\":123}";

        public JsonConverterTests() : base(new TestStructJsonConverter())
        {
        }

        private struct TestStruct
        {
            public int i { get; set; }
        }

        private class TestStructJsonConverter : JsonConverter<string, TestStruct>
        {
            public TestStructJsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }

            protected override string Revert(TestStruct value) => value.i.ToString();
            protected override TestStruct Convert(string value) => new TestStruct() { i = int.Parse(value) };
        }
    }
}
