using Cubusky.Text.Json.Serialization;
using System;
using System.Text.Json;

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Bounds2"/> to or from JSON using an array of two <see cref="float"/> values.</summary>
    public sealed class Bounds2JsonConverter : SpanJsonConverter<Bounds2, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 4;

        /// <inheritdoc />
        public override Bounds2 FromSpan(Span<float> span) => new Bounds2(span[0], span[1], span[2], span[3]);

        /// <inheritdoc />
        public override void ToSpan(Bounds2 value, Span<float> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Width;
            span[3] = value.Height;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Bounds3"/> to or from JSON using an array of three <see cref="float"/> values.</summary>
    public sealed class Bounds3JsonConverter : SpanJsonConverter<Bounds3, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 6;

        /// <inheritdoc />
        public override Bounds3 FromSpan(Span<float> span) => new Bounds3(span[0], span[1], span[2], span[3], span[4], span[5]);

        /// <inheritdoc />
        public override void ToSpan(Bounds3 value, Span<float> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Z;
            span[3] = value.Width;
            span[4] = value.Height;
            span[5] = value.Depth;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }
}
