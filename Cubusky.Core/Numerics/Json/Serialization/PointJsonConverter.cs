using Cubusky.Text.Json.Serialization;
using System.Text.Json;

#if NET8_0_OR_GREATER
using System.Text.Json.Serialization;
#else
using System.Text.Json.Serialization.Metadata;
#endif

namespace Cubusky.Numerics.Json.Serialization
{
#if NET8_0_OR_GREATER
    [JsonSerializable(typeof(int[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
    internal partial class Int32ArrayContext : JsonSerializerContext { }
#endif

    /// <summary>Converts a <see cref="Point2"/> to or from JSON using an array of two <see cref="int"/> values.</summary>
    public class Point2JsonConverter : JsonConverter<Point2, int[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Point2JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Point2JsonConverter() : base(new Int32ArrayContext()) { }
#else
        public Point2JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override int[] Convert(Point2 value) => new int[] { value.X, value.Y };

        /// <inheritdoc />
        protected override Point2 Revert(int[] value) => value.Length == 2 ? new Point2(value[0], value[1]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Point3"/> to or from JSON using an array of three <see cref="int"/> values.</summary>
    public class Point3JsonConverter : JsonConverter<Point3, int[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Point3JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Point3JsonConverter() : base(new Int32ArrayContext()) { }
#else
        public Point3JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override int[] Convert(Point3 value) => new int[] { value.X, value.Y, value.Z };

        /// <inheritdoc />
        protected override Point3 Revert(int[] value) => value.Length == 3 ? new Point3(value[0], value[1], value[2]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Point4"/> to or from JSON using an array of four <see cref="int"/> values.</summary>
    public class Point4JsonConverter : JsonConverter<Point4, int[]>
    {
        /// <summary>Initializes a new instance of the <see cref="Point4JsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public Point4JsonConverter() : base(new Int32ArrayContext()) { }
#else
        public Point4JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override int[] Convert(Point4 value) => new int[] { value.X, value.Y, value.Z, value.W };

        /// <inheritdoc />
        protected override Point4 Revert(int[] value) => value.Length == 4 ? new Point4(value[0], value[1], value[2], value[3]) : throw new JsonException();
    }
}
