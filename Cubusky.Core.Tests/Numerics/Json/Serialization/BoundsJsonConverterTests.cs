using Cubusky.Numerics;
using Cubusky.Numerics.Json.Serialization;
using Cubusky.Tests.Text.Json.Serialization;

namespace Cubusky.Tests.Numerics.Json.Serialization
{
    public class Bounds2JsonConverterTests : JsonConverterTests<Bounds2>
    {
        public override Bounds2 Result => new Bounds2(1, 2, 3, 4);
        public override string Json => "[1,2,3,4]";

        public Bounds2JsonConverterTests() : base(new Bounds2JsonConverter())
        {
        }
    }

    public class Bounds3JsonConverterTests : JsonConverterTests<Bounds3>
    {
        public override Bounds3 Result => new Bounds3(1, 2, 3, 4, 5, 6);
        public override string Json => "[1,2,3,4,5,6]";

        public Bounds3JsonConverterTests() : base(new Bounds3JsonConverter())
        {
        }
    }
}
