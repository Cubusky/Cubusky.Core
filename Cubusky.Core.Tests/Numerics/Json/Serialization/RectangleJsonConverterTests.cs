using Cubusky.Numerics;
using Cubusky.Numerics.Json.Serialization;
using Cubusky.Tests.Text.Json.Serialization;

namespace Cubusky.Tests.Numerics.Json.Serialization
{
    public class RectangleJsonConverterTests : JsonConverterTests<Rectangle>
    {
        public override Rectangle Result => new Rectangle(1, 2, 3, 4);
        public override string Json => "[1,2,3,4]";

        public RectangleJsonConverterTests() : base(new RectangleJsonConverter())
        {
        }
    }

    public class BoxJsonConverterTests : JsonConverterTests<Box>
    {
        public override Box Result => new Box(1, 2, 3, 4, 5, 6);
        public override string Json => "[1,2,3,4,5,6]";

        public BoxJsonConverterTests() : base(new BoxJsonConverter())
        {
        }
    }
}
