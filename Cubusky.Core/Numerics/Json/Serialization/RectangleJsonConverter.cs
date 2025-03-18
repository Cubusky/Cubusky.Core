using Cubusky.Text.Json.Serialization;
using System.Text.Json;

#if !NET8_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace Cubusky.Numerics.Json.Serialization
{
    /// <summary>Converts a <see cref="Rectangle"/> to or from JSON using an array of two <see cref="int"/> values.</summary>
    public class RectangleJsonConverter : JsonConverter<Rectangle, int[]>
    {
        /// <summary>Initializes a new instance of the <see cref="RectangleJsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public RectangleJsonConverter() : base(new Int32ArrayContext()) { }
#else
        public RectangleJsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override int[] Convert(Rectangle value) => new int[] { value.X, value.Y, value.Width, value.Height };

        /// <inheritdoc />
        protected override Rectangle Revert(int[] value) => value.Length == 4 ? new Rectangle(value[0], value[1], value[2], value[3]) : throw new JsonException();
    }

    /// <summary>Converts a <see cref="Box"/> to or from JSON using an array of three <see cref="int"/> values.</summary>
    public class BoxJsonConverter : JsonConverter<Box, int[]>
    {
        /// <summary>Initializes a new instance of the <see cref="BoxJsonConverter"/> class.</summary>
#if NET8_0_OR_GREATER
        public BoxJsonConverter() : base(new Int32ArrayContext()) { }
#else
        public BoxJsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc />
        protected override int[] Convert(Box value) => new int[] { value.X, value.Y, value.Z, value.Width, value.Height, value.Depth };

        /// <inheritdoc />
        protected override Box Revert(int[] value) => value.Length == 6 ? new Box(value[0], value[1], value[2], value[3], value[4], value[5]) : throw new JsonException();
    }
}
