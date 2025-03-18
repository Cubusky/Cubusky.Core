using Cubusky.Text.Json.Serialization;
using System.Text.Json;

#if !NET8_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Bounds2"/> to or from JSON using an array of two <see cref="float"/> values.</summary>
    public class Bounds2JsonConverter : JsonConverter<Bounds2, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Bounds2JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Bounds2JsonConverter() : base(new SingleArrayContext()) { }
#else
        public Bounds2JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Bounds2 value) => new float[] { value.X, value.Y, value.Width, value.Height };

        /// <inheritdoc />
        protected override Bounds2 Revert(float[] value) => value.Length == 4 ? new Bounds2(value[0], value[1], value[2], value[3]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Bounds3"/> to or from JSON using an array of three <see cref="float"/> values.</summary>
    public class Bounds3JsonConverter : JsonConverter<Bounds3, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Bounds3JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Bounds3JsonConverter() : base(new SingleArrayContext()) { }
#else
        public Bounds3JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Bounds3 value) => new float[] { value.X, value.Y, value.Z, value.Width, value.Height, value.Depth };

        /// <inheritdoc />
        protected override Bounds3 Revert(float[] value) => value.Length == 6 ? new Bounds3(value[0], value[1], value[2], value[3], value[4], value[5]) : throw new JsonException();
    }
}
