using Cubusky.Text.Json.Serialization;
using System.Numerics;
using System.Text.Json;

#if NET8_0_OR_GREATER
using System.Text.Json.Serialization;
#else
using System.Text.Json.Serialization.Metadata;
#endif

namespace Cubusky.Numerics.Json.Serialization
{
#if NET8_0_OR_GREATER
    [JsonSerializable(typeof(float[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
    internal partial class SingleArrayContext : JsonSerializerContext { }
#endif

    /// <summary>Converts a <see cref="Vector2"/> to or from JSON using an array of two <see cref="float"/> values.</summary>
    public class Vector2JsonConverter : JsonConverter<Vector2, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Vector2JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Vector2JsonConverter() : base(new SingleArrayContext()) { }
#else
        public Vector2JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Vector2 value) => new float[] { value.X, value.Y };

        /// <inheritdoc />
        protected override Vector2 Revert(float[] value) => value.Length == 2 ? new Vector2(value[0], value[1]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Vector3"/> to or from JSON using an array of three <see cref="float"/> values.</summary>
    public class Vector3JsonConverter : JsonConverter<Vector3, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Vector3JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Vector3JsonConverter() : base(new SingleArrayContext()) { }
#else
        public Vector3JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Vector3 value) => new float[] { value.X, value.Y, value.Z };

        /// <inheritdoc />
        protected override Vector3 Revert(float[] value) => value.Length == 3 ? new Vector3(value[0], value[1], value[2]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Vector4"/> to or from JSON using an array of four <see cref="float"/> values.</summary>
    public class Vector4JsonConverter : JsonConverter<Vector4, float[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Vector4JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Vector4JsonConverter() : base(new SingleArrayContext()) { }
#else
        public Vector4JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override float[] Convert(Vector4 value) => new float[] { value.X, value.Y, value.Z, value.W };

        /// <inheritdoc />
        protected override Vector4 Revert(float[] value) => value.Length == 4 ? new Vector4(value[0], value[1], value[2], value[3]) : throw new JsonException();
    }
}
