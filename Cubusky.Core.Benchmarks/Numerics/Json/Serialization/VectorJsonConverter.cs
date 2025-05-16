using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Cubusky.Numerics.Json.Serialization;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubusky.Benchmarks.Numerics.Json.Serialization
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [ReturnValueValidator(failOnError: true)]
    public class VectorJsonConverter
    {
        public static readonly Vector4 Value = new Vector4(1, 2, 3, 4);
        public const string JsonObject = "{\"X\":1,\"Y\":2,\"Z\":3,\"W\":4}";
        public const string JsonArray = "[1,2,3,4]";

        public static class Category
        {
            public const string Options = nameof(Options);
            public const string JsonTypeInfo = nameof(JsonTypeInfo);
        }

        public static readonly JsonSerializerOptions IncludeFields = new JsonSerializerOptions()
        {
            IncludeFields = true,
        };

        public static readonly JsonSerializerOptions Vector4JsonConverter = new JsonSerializerOptions()
        {
            Converters = { new Vector4JsonConverter() }
        };

        public static readonly JsonSerializerOptions Vector4JsonConverterDirect = new JsonSerializerOptions()
        {
            Converters = { new Vector4JsonConverterDirect() }
        };
    }

    public class VectorJsonConverterSerialize : VectorJsonConverter
    {
        [Benchmark(Baseline = true), BenchmarkCategory(Category.Options)]
        public void SerializeDefault() => JsonSerializer.Serialize(Value, IncludeFields);

        [Benchmark, BenchmarkCategory(Category.Options)]
        public string SerializeVector4JsonConverter() => JsonSerializer.Serialize(Value, Vector4JsonConverter);

        [Benchmark, BenchmarkCategory(Category.Options)]
        public string SerializeVector4JsonConverterDirect() => JsonSerializer.Serialize(Value, Vector4JsonConverterDirect);
    }

    public class VectorJsonConverterDeserialize : VectorJsonConverter
    {
        [Benchmark(Baseline = true), BenchmarkCategory(Category.Options)]
        public Vector4 DeserializeDefault() => JsonSerializer.Deserialize<Vector4>(JsonObject, IncludeFields);

        [Benchmark, BenchmarkCategory(Category.Options)]
        public Vector4 DeserializeVector4JsonConverter() => JsonSerializer.Deserialize<Vector4>(JsonArray, Vector4JsonConverter);

        [Benchmark, BenchmarkCategory(Category.Options)]
        public Vector4 DeserializeVector4JsonConverterDirect() => JsonSerializer.Deserialize<Vector4>(JsonArray, Vector4JsonConverterDirect);
    }

    internal class Vector4JsonConverterDirect : JsonConverter<Vector4>
    {
        public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Vector4 value = default;

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            value.X = reader.GetSingle();

            if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            value.Y = reader.GetSingle();

            if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            value.Z = reader.GetSingle();

            if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }
            value.W = reader.GetSingle();

            return !reader.Read() || reader.TokenType != JsonTokenType.EndArray
                ? throw new JsonException()
                : value;
        }

        public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Z);
            writer.WriteNumberValue(value.W);
            writer.WriteEndArray();
        }
    }
}
