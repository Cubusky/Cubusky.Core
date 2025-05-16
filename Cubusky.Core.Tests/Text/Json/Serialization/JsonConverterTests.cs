using Cubusky.Text.Json;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Cubusky.Tests.Text.Json.Serialization
{
    public abstract class JsonConverterTests<TInput>
    {
        public readonly JsonSerializerOptions Options;

        public abstract TInput Result { get; }
        public abstract string Json { get; }

        public virtual void Validate(TInput expected, TInput actual) => Assert.StrictEqual(expected, actual);

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
            Assert.NotNull(result);
            Validate(Result, result);
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

        public JsonConverterTests() : base(new StringToPropertyIJsonConverter())
        {
        }

        private class StringToPropertyIJsonConverter : JsonConverter<string>
        {
            public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                reader.ThrowIfNotTokenType(JsonTokenType.StartObject);
                reader.ReadOrThrow("i");
                reader.ThrowIfNotTokenType(JsonTokenType.Number);

                string value = reader.GetInt32().ToString();

                reader.ReadOrThrow(JsonTokenType.EndObject);

                return value;
            }

            public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteNumber("i", int.Parse(value));
                writer.WriteEndObject();
            }
        }
    }
}
