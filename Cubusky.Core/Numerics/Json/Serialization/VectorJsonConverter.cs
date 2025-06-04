using Cubusky.Text.Json.Serialization;
using System;
using System.Numerics;
using System.Text.Json;

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Vector2"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public sealed class Vector2JsonConverter : SpanJsonConverter<Vector2, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 2;

        /// <inheritdoc />
        public override Vector2 FromSpan(Span<float> span) => new Vector2(span[0], span[1]);

        /// <inheritdoc />
        public override void ToSpan(Vector2 value, Span<float> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Vector3"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public sealed class Vector3JsonConverter : SpanJsonConverter<Vector3, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 3;

        /// <inheritdoc />
        public override Vector3 FromSpan(Span<float> span) => new Vector3(span[0], span[1], span[2]);

        /// <inheritdoc />
        public override void ToSpan(Vector3 value, Span<float> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Z;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Vector4"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public sealed class Vector4JsonConverter : SpanJsonConverter<Vector4, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 4;

        /// <inheritdoc />
        public override Vector4 FromSpan(Span<float> span) => new Vector4(span[0], span[1], span[2], span[3]);

        /// <inheritdoc />
        public override void ToSpan(Vector4 value, Span<float> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Z;
            span[3] = value.W;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }
}
