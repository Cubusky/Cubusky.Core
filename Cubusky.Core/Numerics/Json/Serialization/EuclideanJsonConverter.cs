using Cubusky.Text.Json.Serialization;
using System;
using System.Numerics;
using System.Text.Json;

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Plane"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public sealed class PlaneJsonConverter : SpanJsonConverter<Plane, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 4;

        /// <inheritdoc />
        public override Plane FromSpan(Span<float> span) => new Plane(span[0], span[1], span[2], span[3]);

        /// <inheritdoc />
        public override void ToSpan(Plane value, Span<float> span)
        {
            span[0] = value.Normal.X;
            span[1] = value.Normal.Y;
            span[2] = value.Normal.Z;
            span[3] = value.D;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Quaternion"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public sealed class QuaternionJsonConverter : SpanJsonConverter<Quaternion, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 4;

        /// <inheritdoc />
        public override Quaternion FromSpan(Span<float> span) => new Quaternion(span[0], span[1], span[2], span[3]);

        /// <inheritdoc />
        public override void ToSpan(Quaternion value, Span<float> span)
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
