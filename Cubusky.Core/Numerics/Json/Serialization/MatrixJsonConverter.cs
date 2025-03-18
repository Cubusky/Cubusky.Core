using Cubusky.Text.Json.Serialization;
using System.Numerics;
using System.Text.Json;

#if !NET8_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Matrix3x2"/> to or from JSON using an array of six <see cref="float"/> values.</summary>
    public class Matrix3x2JsonConverter : JsonConverter<Matrix3x2, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Matrix3x2JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Matrix3x2JsonConverter() : base(new SingleArrayContext()) { }
#else
        public Matrix3x2JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Matrix3x2 value) => new float[] { value.M11, value.M12, value.M21, value.M22, value.M31, value.M32 };

        /// <inheritdoc />
        protected override Matrix3x2 Revert(float[] value) => value.Length == 6 ? new Matrix3x2(value[0], value[1], value[2], value[3], value[4], value[5]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Matrix4x4"/> to or from JSON using an array of sixteen <see cref="float"/> values.</summary>
    public class Matrix4x4JsonConverter : JsonConverter<Matrix4x4, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Matrix4x4JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Matrix4x4JsonConverter() : base(new SingleArrayContext()) { }
#else
        public Matrix4x4JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Matrix4x4 value) => new float[] { value.M11, value.M12, value.M13, value.M14, value.M21, value.M22, value.M23, value.M24, value.M31, value.M32, value.M33, value.M34, value.M41, value.M42, value.M43, value.M44 };

        /// <inheritdoc />
        protected override Matrix4x4 Revert(float[] value) => value.Length == 16 ? new Matrix4x4(value[0], value[1], value[2], value[3], value[4], value[5], value[6], value[7], value[8], value[9], value[10], value[11], value[12], value[13], value[14], value[15]) : throw new JsonException();
    }
}
