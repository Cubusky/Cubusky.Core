using Cubusky.Text.Json.Serialization;
using System;
using System.Text.Json;

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Rectangle"/> to or from JSON using an array of two <see cref="int"/> values.</summary>
    public sealed class RectangleJsonConverter : SpanJsonConverter<Rectangle, int>
    {
        /// <inheritdoc />
        public override int SpanLength => 4;

        /// <inheritdoc />
        public override Rectangle FromSpan(Span<int> span) => new Rectangle(span[0], span[1], span[2], span[3]);

        /// <inheritdoc />
        public override void ToSpan(Rectangle value, Span<int> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Width;
            span[3] = value.Height;
        }

        /// <inheritdoc />
        protected override int ReadValue(ref Utf8JsonReader reader) => reader.GetInt32();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, int value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Box"/> to or from JSON using an array of three <see cref="int"/> values.</summary>
    public sealed class BoxJsonConverter : SpanJsonConverter<Box, int>
    {
        /// <inheritdoc />
        public override int SpanLength => 6;

        /// <inheritdoc />
        public override Box FromSpan(Span<int> span) => new Box(span[0], span[1], span[2], span[3], span[4], span[5]);

        /// <inheritdoc />
        public override void ToSpan(Box value, Span<int> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Z;
            span[3] = value.Width;
            span[4] = value.Height;
            span[5] = value.Depth;
        }

        /// <inheritdoc />
        protected override int ReadValue(ref Utf8JsonReader reader) => reader.GetInt32();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, int value) => writer.WriteNumberValue(value);
    }
}
