using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Cubusky.Text.Json.Serialization
{
    /// <summary>Converts an object or value to or from JSON using a secondary data structure.</summary>
    /// <typeparam name="TInput">The type of the object value to convert to or from JSON.</typeparam>
    /// <typeparam name="TOutput">The type of the JSON data structure.</typeparam>
    /// <remarks>
    /// When it is desirable to represent a type in JSON using a different data structure, implement this helper class. For example, to represent a <see cref="Vector3"/> as an array of three <see cref="float"/> values, implement the following class:
    /// <code><![CDATA[ Vector3JsonConverter : JsonConverter<Vector3, float[]> ]]></code>
    /// </remarks>
    [Obsolete("Use JsonConverter<T> from System.Text.Json.Serialization instead. This class will be removed in a future version.")]
    public abstract class JsonConverter<TInput, TOutput> : JsonConverter<TInput>
    {
        private readonly IJsonTypeInfoResolver jsonTypeInfoResolver;

        /// <summary>Initializes a new instance of the <see cref="JsonConverter{TInput, TOutput}"/> class. Use <see langword="new"/> <see cref="DefaultJsonTypeInfoResolver.DefaultJsonTypeInfoResolver"/> to use the default resolver.</summary>
        /// <param name="jsonTypeInfoResolver">The object for resolving <see cref="JsonTypeInfo{TOutput}"/>. Set to <see langword="new"/> <see cref="DefaultJsonTypeInfoResolver.DefaultJsonTypeInfoResolver"/> to use the default resolver.</param>
        public JsonConverter(IJsonTypeInfoResolver jsonTypeInfoResolver)
        {
            this.jsonTypeInfoResolver = jsonTypeInfoResolver;
        }

        private JsonTypeInfo<TOutput> TypeInfo(JsonSerializerOptions options) => (JsonTypeInfo<TOutput>)(jsonTypeInfoResolver.GetTypeInfo(typeof(TOutput), options) ?? throw new JsonException());

        /// <summary>Converts the specified value to the JSON data structure.</summary>
        /// <param name="value">The value to convert to a JSON data structure.</param>
        /// <returns>A populated JSON data structure.</returns>
        abstract protected TOutput Convert(TInput value);

        /// <summary>Reverts the specified JSON data structure to the value.</summary>
        /// <param name="value">The JSON data structure to revert to an object.</param>
        /// <returns>A populated object.</returns>
        abstract protected TInput Revert(TOutput value);

        /// <inheritdoc />
        [return: MaybeNull]
        public override TInput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = JsonSerializer.Deserialize(ref reader, TypeInfo(options));
            return value is null ? default : Revert(value);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, TInput value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, Convert(value), TypeInfo(options));
    }
}
