using Cubusky.Numerics.Json.Serialization;
using Cubusky.Tests.Text.Json.Serialization;
using System.Numerics;

namespace Cubusky.Tests.Numerics.Json.Serialization
{
    public class Vector2JsonConverterTests : JsonConverterTests<Vector2>
    {
        public override Vector2 Result => new Vector2(1, 2);
        public override string Json => "[1,2]";

        public Vector2JsonConverterTests() : base(new Vector2JsonConverter())
        {
        }
    }

    public class Vector3JsonConverterTests : JsonConverterTests<Vector3>
    {
        public override Vector3 Result => new Vector3(1, 2, 3);
        public override string Json => "[1,2,3]";

        public Vector3JsonConverterTests() : base(new Vector3JsonConverter())
        {
        }
    }

    public class Vector4JsonConverterTests : JsonConverterTests<Vector4>
    {
        public override Vector4 Result => new Vector4(1, 2, 3, 4);
        public override string Json => "[1,2,3,4]";

        public Vector4JsonConverterTests() : base(new Vector4JsonConverter())
        {
        }
    }
}
