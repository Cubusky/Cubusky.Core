using Cubusky.Numerics.Json.Serialization;
using Cubusky.Tests.Text.Json.Serialization;
using System.Numerics;

namespace Cubusky.Tests.Numerics.Json.Serialization
{
    public class PlaneJsonConverterTests : JsonConverterTests<Plane>
    {
        public override Plane Result => new Plane(1, 2, 3, 4);
        public override string Json => "[1,2,3,4]";

        public PlaneJsonConverterTests() : base(new PlaneJsonConverter())
        {
        }
    }

    public class QuaternionJsonConverterTests : JsonConverterTests<Quaternion>
    {
        public override Quaternion Result => new Quaternion(1, 2, 3, 4);
        public override string Json => "[1,2,3,4]";

        public QuaternionJsonConverterTests() : base(new QuaternionJsonConverter())
        {
        }
    }
}
