using Cubusky.Heatmaps;
using Cubusky.Numerics;
using Cubusky.Tests.Text.Json.Serialization;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Heatmaps.Json.Serialization
{
    public class Heatmap2JsonConverterTests : JsonConverterTests<Heatmap2>
    {
        public override Heatmap2 Result => new Heatmap2(new Matrix3x2(11, 12, 21, 22, 31, 32))
        {
            { new Point2(-3, -3), 7 },
            { new Point2(0, -3), 6 },
            { new Point2(3, -3), 8 },

            { new Point2(-3, 0), 5 },
            { new Point2(3, 0), 2 },

            { new Point2(-3, 3), 9 },
            { new Point2(0, 3), 3 },
            { new Point2(3, 3), 4 },
        };

        public override string Json => "{\"Matrix\":[11,12,21,22,31,32],\"Bounds\":[-3,-3,7,7],\"Strengths\":[7,-2,6,-2,8,-14,5,-5,2,-14,9,-2,3,-2,4]}";

        public override void Validate(Heatmap2 expected, Heatmap2 actual)
        {
            Assert.Equal(expected.CellToPositionMatrix, actual.CellToPositionMatrix);
            Assert.Equal(expected.Count, actual.Count);
            foreach (var (cell, strength) in expected)
            {
                Assert.Equal(actual[cell], strength);
            }
        }
    }

    public class Heatmap3to2XYJsonConverterTests : JsonConverterTests<Heatmap3to2>
    {
        public override Heatmap3to2 Result => new Heatmap3to2(new Matrix4x4(0.2971f, 0.4537f, 0.0191f, 0.8069f, 0.4482f, 0.1602f, 0.3329f, 0.4238f, 0.3899f, 0.9365f, 0.4164f, 0.2365f, 0.6978f, 0.9152f, 0.8771f, 0.0860f), Swizzle3to2.XY)
        {
            { new Point2(-3, -3), 7 },
            { new Point2(0, -3), 6 },
            { new Point2(3, -3), 8 },

            { new Point2(-3, 0), 5 },
            { new Point2(3, 0), 2 },

            { new Point2(-3, 3), 9 },
            { new Point2(0, 3), 3 },
            { new Point2(3, 3), 4 },
        };

        public override string Json => "{\"Matrix\":" +
#if NET8_0_OR_GREATER
            "[0.2971,0.4537,0.0191,0.8069,0.4482,0.1602,0.3329,0.4238,0.3899,0.9365,0.4164,0.2365,0.6978,0.9152,0.8771,0.086]" +
#else
            "[0.297100008,0.453700006,0.0190999992,0.806900024,0.448199987,0.1602,0.332899988,0.423799992,0.389899999,0.936500013,0.416399986,0.236499995,0.697799981,0.915199995,0.877099991,0.0860000029]" +
#endif
            ",\"Swizzle\":0,\"Bounds\":[-3,-3,7,7],\"Strengths\":[7,-2,6,-2,8,-14,5,-5,2,-14,9,-2,3,-2,4]}";

        public override void Validate(Heatmap3to2 expected, Heatmap3to2 actual)
        {
            Assert.Equal(expected.CellToPositionMatrix, actual.CellToPositionMatrix);
            Assert.Equal(expected.Swizzle, actual.Swizzle);
            Assert.Equal(expected.Count, actual.Count);
            foreach (var (cell, strength) in expected)
            {
                Assert.Equal(actual[cell], strength);
            }
        }
    }

    public class Heatmap3to2XZJsonConverterTests : JsonConverterTests<Heatmap3to2>
    {
        public override Heatmap3to2 Result => new Heatmap3to2(new Matrix4x4(0.2971f, 0.4537f, 0.0191f, 0.8069f, 0.4482f, 0.1602f, 0.3329f, 0.4238f, 0.3899f, 0.9365f, 0.4164f, 0.2365f, 0.6978f, 0.9152f, 0.8771f, 0.0860f), Swizzle3to2.XZ)
        {
            { new Point2(-3, -3), 7 },
            { new Point2(0, -3), 6 },
            { new Point2(2, -3), 8 },

            { new Point2(-3, 0), 5 },
            { new Point2(3, 0), 2 },

            { new Point2(-3, 2), 9 },
            { new Point2(0, 3), 3 },
            { new Point2(3, 3), 4 },
        };

        public override string Json => "{\"Matrix\":" +
#if NET8_0_OR_GREATER
            "[0.2971,0.4537,0.0191,0.8069,0.4482,0.1602,0.3329,0.4238,0.3899,0.9365,0.4164,0.2365,0.6978,0.9152,0.8771,0.086]" +
#else
            "[0.297100008,0.453700006,0.0190999992,0.806900024,0.448199987,0.1602,0.332899988,0.423799992,0.389899999,0.936500013,0.416399986,0.236499995,0.697799981,0.915199995,0.877099991,0.0860000029]" +
#endif
            ",\"Swizzle\":1,\"Bounds\":[-3,-3,7,7],\"Strengths\":[7,-2,6,-1,8,-15,5,-5,2,-7,9,-9,3,-2,4]}";

        public override void Validate(Heatmap3to2 expected, Heatmap3to2 actual)
        {
            Assert.Equal(expected.CellToPositionMatrix, actual.CellToPositionMatrix);
            Assert.Equal(expected.Swizzle, actual.Swizzle);
            Assert.Equal(expected.Count, actual.Count);
            foreach (var (cell, strength) in expected)
            {
                Assert.Equal(actual[cell], strength);
            }
        }
    }

    public class Heatmap3to2YZJsonConverterTests : JsonConverterTests<Heatmap3to2>
    {
        public override Heatmap3to2 Result => new Heatmap3to2(new Matrix4x4(0.2971f, 0.4537f, 0.0191f, 0.8069f, 0.4482f, 0.1602f, 0.3329f, 0.4238f, 0.3899f, 0.9365f, 0.4164f, 0.2365f, 0.6978f, 0.9152f, 0.8771f, 0.0860f), Swizzle3to2.YZ)
        {
            { new Point2(-1, -3), 7 },
            { new Point2(0, -3), 6 },
            { new Point2(3, -3), 8 },

            { new Point2(-3, 0), 5 },
            { new Point2(3, 0), 2 },

            { new Point2(-3, 3), 9 },
            { new Point2(0, 3), 3 },
            { new Point2(3, 3), 4 },
        };

        public override string Json => "{\"Matrix\":" +
#if NET8_0_OR_GREATER
            "[0.2971,0.4537,0.0191,0.8069,0.4482,0.1602,0.3329,0.4238,0.3899,0.9365,0.4164,0.2365,0.6978,0.9152,0.8771,0.086]" +
#else
            "[0.297100008,0.453700006,0.0190999992,0.806900024,0.448199987,0.1602,0.332899988,0.423799992,0.389899999,0.936500013,0.416399986,0.236499995,0.697799981,0.915199995,0.877099991,0.0860000029]" +
#endif
            ",\"Swizzle\":2,\"Bounds\":[-3,-3,7,7],\"Strengths\":[-2,7,6,-2,8,-14,5,-5,2,-14,9,-2,3,-2,4]}";

        public override void Validate(Heatmap3to2 expected, Heatmap3to2 actual)
        {
            Assert.Equal(expected.CellToPositionMatrix, actual.CellToPositionMatrix);
            Assert.Equal(expected.Swizzle, actual.Swizzle);
            Assert.Equal(expected.Count, actual.Count);
            foreach (var (cell, strength) in expected)
            {
                Assert.Equal(actual[cell], strength);
            }
        }
    }

    public class Heatmap3JsonConverterTests : JsonConverterTests<Heatmap3>
    {
        public override Heatmap3 Result => new Heatmap3(new Matrix4x4(0.2971f, 0.4537f, 0.0191f, 0.8069f, 0.4482f, 0.1602f, 0.3329f, 0.4238f, 0.3899f, 0.9365f, 0.4164f, 0.2365f, 0.6978f, 0.9152f, 0.8771f, 0.0860f))
        {
            { new Point3(-3, -3, -3), 1 },
            { new Point3(0, -3, -3), 2 },
            { new Point3(2, -3, -3), 3 },

            { new Point3(-3, 0, -3), 4 },
            { new Point3(0, 0, -3), 5 },
            { new Point3(2, 2, -3), 6 },

            { new Point3(-3, 3, -3), 7 },
            { new Point3(0, 3, -3), 8 },
            { new Point3(3, 3, -3), 9 },



            { new Point3(-3, -3, -1), 10 },
            { new Point3(0, -3, 0), 11 },
            { new Point3(3, -3, 0), 12 },

            { new Point3(-3, 0, 0), 13 },
            { new Point3(0, 0, 0), 14 },
            { new Point3(3, 0, 0), 15 },

            { new Point3(-3, 3, 0), 16 },
            { new Point3(0, 3, 0), 17 },
            { new Point3(3, 3, 2), 18 },



            { new Point3(-3, -3, 3), 19 },
            { new Point3(0, -3, 3), 20 },
            { new Point3(3, -3, 3), 21 },

            { new Point3(-3, 0, 3), 22 },
            { new Point3(0, 0, 3), 23 },
            { new Point3(3, 0, 3), 24 },

            { new Point3(-3, 3, 3), 25 },
            { new Point3(0, 3, 3), 26 },
            { new Point3(3, 3, 3), 27 },
        };

        public override string Json => "{\"Matrix\":" +
#if NET8_0_OR_GREATER
            "[0.2971,0.4537,0.0191,0.8069,0.4482,0.1602,0.3329,0.4238,0.3899,0.9365,0.4164,0.2365,0.6978,0.9152,0.8771,0.086]" +
#else
            "[0.297100008,0.453700006,0.0190999992,0.806900024,0.448199987,0.1602,0.332899988,0.423799992,0.389899999,0.936500013,0.416399986,0.236499995,0.697799981,0.915199995,0.877099991,0.0860000029]" +
#endif
            ",\"Bounds\":[-3,-3,-3,7,7,7],\"Strengths\":[1,-2,2,-1,3,-15,4,-2,5,-15,6,-1,7,-2,8,-2,9,-49,10,-51,11,-2,12,-14,13,-2,14,-2,15,-14,16,-2,17,-100,18,19,-2,20,-2,21,-14,22,-2,23,-2,24,-14,25,-2,26,-2,27]}";

        public override void Validate(Heatmap3 expected, Heatmap3 actual)
        {
            Assert.Equal(expected.CellToPositionMatrix, actual.CellToPositionMatrix);
            Assert.Equal(expected.Count, actual.Count);
            foreach (var (cell, strength) in expected)
            {
                Assert.Equal(actual[cell], strength);
            }
        }
    }
}
