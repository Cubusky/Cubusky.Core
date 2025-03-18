using Cubusky.Text.Json.Serialization;
using System.Numerics;
using System.Text.Json;

#if !NET8_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Plane"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public class PlaneJsonConverter : JsonConverter<Plane, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="PlaneJsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public PlaneJsonConverter() : base(new SingleArrayContext()) { }
#else
        public PlaneJsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Plane value) => new float[] { value.Normal.X, value.Normal.Y, value.Normal.Z, value.D };

        /// <inheritdoc />
        protected override Plane Revert(float[] value) => value.Length == 4 ? new Plane(new Vector3(value[0], value[1], value[2]), value[3]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Quaternion"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public class QuaternionJsonConverter : JsonConverter<Quaternion, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="QuaternionJsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public QuaternionJsonConverter() : base(new SingleArrayContext()) { }
#else
        public QuaternionJsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Quaternion value) => new float[] { value.X, value.Y, value.Z, value.W };

        /// <inheritdoc />
        protected override Quaternion Revert(float[] value) => value.Length == 4 ? new Quaternion(value[0], value[1], value[2], value[3]) : throw new JsonException();
    }
}
