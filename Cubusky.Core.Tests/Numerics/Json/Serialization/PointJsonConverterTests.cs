using Cubusky.Numerics;
using Cubusky.Numerics.Json.Serialization;
using Cubusky.Tests.Text.Json.Serialization;

namespace Cubusky.Tests.Numerics.Json.Serialization
{
    public class Point2JsonConverterTests : JsonConverterTests<Point2>
    {
        public override Point2 Result => new Point2(1, 2);
        public override string Json => "[1,2]";

        public Point2JsonConverterTests() : base(new Point2JsonConverter())
        {
        }
    }

    public class Point3JsonConverterTests : JsonConverterTests<Point3>
    {
        public override Point3 Result => new Point3(1, 2, 3);
        public override string Json => "[1,2,3]";

        public Point3JsonConverterTests() : base(new Point3JsonConverter())
        {
        }
    }

    public class Point4JsonConverterTests : JsonConverterTests<Point4>
    {
        public override Point4 Result => new Point4(1, 2, 3, 4);
        public override string Json => "[1,2,3,4]";

        public Point4JsonConverterTests() : base(new Point4JsonConverter())
        {
        }
    }
}
