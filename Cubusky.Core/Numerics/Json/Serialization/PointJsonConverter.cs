using Cubusky.Text.Json.Serialization;
using System;
using System.Text.Json;

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Point2"/> to or from JSON using an array of two <see cref="int"/> values.</summary>
    public sealed class Point2JsonConverter : SpanJsonConverter<Point2, int>
    {
        /// <inheritdoc />
        public override int SpanLength => 2;

        /// <inheritdoc />
        public override Point2 FromSpan(Span<int> span) => new Point2(span[0], span[1]);

        /// <inheritdoc />
        public override void ToSpan(Point2 value, Span<int> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
        }

        /// <inheritdoc />
        protected override int ReadValue(ref Utf8JsonReader reader) => reader.GetInt32();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, int value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Point3"/> to or from JSON using an array of three <see cref="int"/> values.</summary>
    public sealed class Point3JsonConverter : SpanJsonConverter<Point3, int>
    {
        /// <inheritdoc />
        public override int SpanLength => 3;

        /// <inheritdoc />
        public override Point3 FromSpan(Span<int> span) => new Point3(span[0], span[1], span[2]);

        /// <inheritdoc />
        public override void ToSpan(Point3 value, Span<int> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Z;
        }

        /// <inheritdoc />
        protected override int ReadValue(ref Utf8JsonReader reader) => reader.GetInt32();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, int value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Point4"/> to or from JSON using an array of four <see cref="int"/> values.</summary>
    public sealed class Point4JsonConverter : SpanJsonConverter<Point4, int>
    {
        /// <inheritdoc />
        public override int SpanLength => 4;

        /// <inheritdoc />
        public override Point4 FromSpan(Span<int> span) => new Point4(span[0], span[1], span[2], span[3]);

        /// <inheritdoc />
        public override void ToSpan(Point4 value, Span<int> span)
        {
            span[0] = value.X;
            span[1] = value.Y;
            span[2] = value.Z;
            span[3] = value.W;
        }

        /// <inheritdoc />
        protected override int ReadValue(ref Utf8JsonReader reader) => reader.GetInt32();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, int value) => writer.WriteNumberValue(value);
    }
}
