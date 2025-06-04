using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubusky.Text.Json.Serialization
{
    /// <summary>Converts an object or value to or from JSON as an array.</summary>
    /// <typeparam name="TInput">The type of the object or value to convert to or from JSON.</typeparam>
    /// <typeparam name="TSpanValue">The element type of the array.</typeparam>
    /// <remarks>
    /// When it is desirable to represent a type in JSON as an array, implement this helper class. For example, to represent a <see cref="Vector3"/> as an array of three <see cref="float"/> values, implement the following class:
    /// <code><![CDATA[ Vector3JsonConverter : SpanJsonConverter<Vector3, float> ]]></code>
    /// </remarks>
    public abstract class SpanJsonConverter<TInput, TSpanValue> : JsonConverter<TInput>
        where TSpanValue : unmanaged
    {
        /// <summary>The length of the span used to represent the object in JSON.</summary>
        public abstract int SpanLength { get; }

        /// <summary>Creates a new value from the provided span.</summary>
        /// <param name="span">The provided span to create the new value from.</param>
        /// <returns>The new value created from the provided span.</returns>
        public abstract TInput FromSpan(Span<TSpanValue> span);

        /// <summary>Populates a span from the provided value.</summary>
        /// <param name="value">The provided value to populate the span from.</param>
        /// <param name="span">The span to populate from the provided value.</param>
        public abstract void ToSpan(TInput value, Span<TSpanValue> span);

        /// <summary>Reads the next JSON token value from the source and parses it to a value.</summary>
        protected abstract TSpanValue ReadValue(ref Utf8JsonReader reader);

        /// <summary>Writes a value as an element of a JSON array.</summary>
        protected abstract void WriteValue(Utf8JsonWriter writer, TSpanValue value);

        /// <inheritdoc />
        [return: MaybeNull]
        public override TInput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            Span<TSpanValue> span = stackalloc TSpanValue[SpanLength];
            for (int i = 0; i < SpanLength; i++)
            {
                if (!reader.Read())
                {
                    throw new JsonException();
                }

                span[i] = ReadValue(ref reader);
            }

            return !reader.Read() || reader.TokenType != JsonTokenType.EndArray
                ? throw new JsonException()
                : FromSpan(span);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, TInput value, JsonSerializerOptions options)
        {
            Span<TSpanValue> span = stackalloc TSpanValue[SpanLength];
            ToSpan(value, span);

            writer.WriteStartArray();
            for (int i = 0; i < SpanLength; i++)
            {
                WriteValue(writer, span[i]);
            }
            writer.WriteEndArray();
        }
    }
}
