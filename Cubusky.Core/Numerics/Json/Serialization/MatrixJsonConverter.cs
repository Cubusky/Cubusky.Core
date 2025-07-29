using Cubusky.Text.Json.Serialization;
using System;
using System.Numerics;
using System.Text.Json;

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Matrix3x2"/> to or from JSON using an array of six <see cref="float"/> values.</summary>
    public sealed class Matrix3x2JsonConverter : SpanJsonConverter<Matrix3x2, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 6;

        /// <inheritdoc />
        public override Matrix3x2 FromSpan(Span<float> span) => new Matrix3x2(span[0], span[1], span[2], span[3], span[4], span[5]);

        /// <inheritdoc />
        public override void ToSpan(Matrix3x2 value, Span<float> span)
        {
            span[0] = value.M11;
            span[1] = value.M12;
            span[2] = value.M21;
            span[3] = value.M22;
            span[4] = value.M31;
            span[5] = value.M32;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }

    /// <summary>Converts a <see cref="Matrix4x4"/> to or from JSON using an array of sixteen <see cref="float"/> values.</summary>
    public sealed class Matrix4x4JsonConverter : SpanJsonConverter<Matrix4x4, float>
    {
        /// <inheritdoc />
        public override int SpanLength => 16;

        /// <inheritdoc />
        public override Matrix4x4 FromSpan(Span<float> span) => new Matrix4x4(span[0], span[1], span[2], span[3], span[4], span[5], span[6], span[7], span[8], span[9], span[10], span[11], span[12], span[13], span[14], span[15]);

        /// <inheritdoc />
        public override void ToSpan(Matrix4x4 value, Span<float> span)
        {
            span[0] = value.M11;
            span[1] = value.M12;
            span[2] = value.M13;
            span[3] = value.M14;
            span[4] = value.M21;
            span[5] = value.M22;
            span[6] = value.M23;
            span[7] = value.M24;
            span[8] = value.M31;
            span[9] = value.M32;
            span[10] = value.M33;
            span[11] = value.M34;
            span[12] = value.M41;
            span[13] = value.M42;
            span[14] = value.M43;
            span[15] = value.M44;
        }

        /// <inheritdoc />
        protected override float ReadValue(ref Utf8JsonReader reader) => reader.GetSingle();

        /// <inheritdoc />
        protected override void WriteValue(Utf8JsonWriter writer, float value) => writer.WriteNumberValue(value);
    }
}
