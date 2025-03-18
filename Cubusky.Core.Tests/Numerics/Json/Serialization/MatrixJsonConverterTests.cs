using Cubusky.Numerics.Json.Serialization;
using Cubusky.Tests.Text.Json.Serialization;
using System.Numerics;

namespace Cubusky.Tests.Numerics.Json.Serialization
{
    public class Matrix3x2JsonConverterTests : JsonConverterTests<Matrix3x2>
    {
        public override Matrix3x2 Result => new Matrix3x2(11, 12, 21, 22, 31, 32);
        public override string Json => "[11,12,21,22,31,32]";

        public Matrix3x2JsonConverterTests() : base(new Matrix3x2JsonConverter())
        {
        }
    }

    public class Matrix4x4JsonConverterTests : JsonConverterTests<Matrix4x4>
    {
        public override Matrix4x4 Result => new Matrix4x4(11, 12, 13, 14, 21, 22, 23, 24, 31, 32, 33, 34, 41, 42, 43, 44);
        public override string Json => "[11,12,13,14,21,22,23,24,31,32,33,34,41,42,43,44]";

        public Matrix4x4JsonConverterTests() : base(new Matrix4x4JsonConverter())
        {
        }
    }
}
