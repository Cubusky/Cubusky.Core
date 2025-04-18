using Cubusky.Heatmaps;
using Cubusky.Numerics;
using System;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Heatmaps
{
    public class Heatmap3to2Tests
    {
        #region Get Cell / Get Position
        public static readonly Vector3 position = Vector3.One * 10f;
        public static readonly Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI * -0.5f);
        public static readonly Vector3 scales = new Vector3(0.25f, 0.5f, 2f);

        public static readonly Heatmap3to2 heatmapTranslated = new Heatmap3to2(Matrix4x4.CreateTranslation(position));
        public static readonly Heatmap3to2 heatmapRotatedXY = new Heatmap3to2(Matrix4x4.CreateFromQuaternion(quaternion), Swizzle3to2.XY);
        public static readonly Heatmap3to2 heatmapRotatedXZ = new Heatmap3to2(Matrix4x4.CreateFromQuaternion(quaternion), Swizzle3to2.XZ);
        public static readonly Heatmap3to2 heatmapRotatedYZ = new Heatmap3to2(Matrix4x4.CreateFromQuaternion(quaternion), Swizzle3to2.YZ);
        public static readonly Heatmap3to2 heatmapScaledXY = new Heatmap3to2(Matrix4x4.CreateScale(scales), Swizzle3to2.XY);
        public static readonly Heatmap3to2 heatmapScaledXZ = new Heatmap3to2(Matrix4x4.CreateScale(scales), Swizzle3to2.XZ);
        public static readonly Heatmap3to2 heatmapScaledYZ = new Heatmap3to2(Matrix4x4.CreateScale(scales), Swizzle3to2.YZ);
        public static readonly Heatmap3to2 heatmapTransformedXY = new Heatmap3to2(Matrix.CreateTransformation4x4(position, quaternion, scales), Swizzle3to2.XY);
        public static readonly Heatmap3to2 heatmapTransformedXZ = new Heatmap3to2(Matrix.CreateTransformation4x4(position, quaternion, scales), Swizzle3to2.XZ);
        public static readonly Heatmap3to2 heatmapTransformedYZ = new Heatmap3to2(Matrix.CreateTransformation4x4(position, quaternion, scales), Swizzle3to2.YZ);

        public static readonly TheoryData<Heatmap3to2, Vector3, Point2, Vector3> HeatmapPositionCell = new TheoryData<Heatmap3to2, Vector3, Point2, Vector3>()
        {
            { heatmapTranslated, position, Point2.Zero, position },
            { heatmapRotatedXY, position, new Point2(10, 10), new Vector3(0, 10, 10) },
            { heatmapRotatedXZ, position, new Point2(10, -10), new Vector3(10, 0, 10) },
            { heatmapRotatedYZ, position, new Point2(10, -10), new Vector3(10, 10, 0) },
            { heatmapScaledXY, position, new Point2(40, 20), new Vector3(10, 10, 0) },
            { heatmapScaledXZ, position, new Point2(40, 5), new Vector3(10, 0, 10) },
            { heatmapScaledYZ, position, new Point2(20, 5), new Vector3(0, 10, 10) },
            { heatmapTransformedXY, new Vector3(50, 20, -45), new Point2(-220, 20), new Vector3(10, 20, -45) },
            { heatmapTransformedXZ, new Vector3(50, 20, -45), new Point2(-220, -20), new Vector3(50, 10, -45) },
            { heatmapTransformedYZ, new Vector3(50, 20, -45), new Point2(20, -20), new Vector3(50, 20, 10) },
        };

        [Theory]
        [MemberData(nameof(HeatmapPositionCell))]
        public void GetCell(Heatmap3to2 heatmap, Vector3 position, Point2 cell, Vector3 _0)
        {
            var result = heatmap.GetCell(position);
            Assert.Equal(cell, result);
        }

        [Theory]
        [MemberData(nameof(HeatmapPositionCell))]
        public void GetPosition(Heatmap3to2 heatmap, Vector3 _0, Point2 cell, Vector3 position)
        {
            var result = heatmap.GetPosition(cell);

            Assert.Equal(position.X, result.X, 1E-5f);
            Assert.Equal(position.Y, result.Y, 1E-5f);
            Assert.Equal(position.Z, result.Z, 1E-5f);
        }
        #endregion

        #region Strength / Add / Remove
        public static readonly Heatmap3to2 heatmapPopulated = new Heatmap3to2()
        {
            { Point2.Zero, 5 },
            Vector3.Zero,
            { Point2.Zero, 12 },
            { Vector3.UnitX, 3 },
            { Point2.UnitY, 7 },
            { Vector3.UnitZ, 11 },
        };

        public static Heatmap3to2 Copy(Heatmap3to2 heatmap)
        {
            var copy = new Heatmap3to2(heatmap.CellToPositionMatrix);
            foreach (var (cell, strength) in heatmap)
            {
                copy.Add(cell, strength);
            }
            return copy;
        }

        public static readonly TheoryData<Point2, int, int> CellStrength = new TheoryData<Point2, int, int>()
        {
            { Point2.Zero, 29, 4 },
            { Point2.UnitX, 3, 5 },
            { Point2.UnitY, 7, 6 },
        };

        [Theory]
        [MemberData(nameof(CellStrength))]
        public void Strength(Point2 cell, int strength, int _0)
        {
            var result = heatmapPopulated[cell];
            Assert.Equal(strength, result);
        }

        [Theory]
        [MemberData(nameof(CellStrength))]
        public void Add(Point2 cell, int strength, int add)
        {
            var heatmapPopulatedCopy = Copy(heatmapPopulated);
            heatmapPopulatedCopy.Add(cell, add);
            var result = heatmapPopulatedCopy[cell];
            Assert.Equal(strength + add, result);
        }

        [Fact]
        public void Add0_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("strength", () => heatmapPopulated.Add(Point2.Zero, 0));
        }

        [Theory]
        [MemberData(nameof(CellStrength))]
        public void Remove(Point2 cell, int strength, int remove)
        {
            var heatmapPopulatedCopy = Copy(heatmapPopulated);
            heatmapPopulatedCopy.Remove(cell, remove);
            var result = heatmapPopulatedCopy[cell];
            Assert.Equal(Math.Max(strength - remove, 0), result);
        }

        [Fact]
        public void Remove0_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("strength", () => heatmapPopulated.Remove(Point2.Zero, 0));
        }
        #endregion
    }
}