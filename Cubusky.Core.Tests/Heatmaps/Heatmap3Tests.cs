using Cubusky.Heatmaps;
using Cubusky.Numerics;
using System;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Heatmaps
{
    public class Heatmap3Tests
    {
        #region Get Cell / Get Position
        public static readonly Vector3 position = Vector3.One * 10f;
        public static readonly Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathF.PI * -0.5f);
        public static readonly Vector3 scales = new Vector3(0.25f, 0.5f, 2f);

        public static readonly Heatmap3 heatmapTranslated = new Heatmap3(Matrix4x4.CreateTranslation(position));
        public static readonly Heatmap3 heatmapRotated = new Heatmap3(Matrix4x4.CreateFromQuaternion(quaternion));
        public static readonly Heatmap3 heatmapScaled = new Heatmap3(Matrix4x4.CreateScale(scales));
        public static readonly Heatmap3 heatmapTransformed = new Heatmap3(Matrix.CreateTransformation4x4(position, quaternion, scales));

        public static readonly TheoryData<Heatmap3, Vector3, Point3> HeatmapPositionCell = new TheoryData<Heatmap3, Vector3, Point3>()
        {
            { heatmapTranslated, position, Point3.Zero },
            { heatmapRotated, position, new Point3(10, 10, -10) },
            { heatmapScaled, position, new Point3(40, 20, 5) },
            { heatmapTransformed, new Vector3(50, 20, -45), new Point3(-220, 20, -20) },
        };

        [Theory]
        [MemberData(nameof(HeatmapPositionCell))]
        public void GetCell(Heatmap3 heatmap, Vector3 position, Point3 cell)
        {
            var result = heatmap.GetCell(position);
            Assert.Equal(cell, result);
        }

        [Theory]
        [MemberData(nameof(HeatmapPositionCell))]
        public void GetPosition(Heatmap3 heatmap, Vector3 position, Point3 cell)
        {
            var result = heatmap.GetPosition(cell);
            Assert.Equal(position.X, result.X, 1E-5f);
            Assert.Equal(position.Y, result.Y, 1E-5f);
            Assert.Equal(position.Z, result.Z, 1E-5f);
        }
        #endregion

        #region Strength / Add / Remove
        public static readonly Heatmap3 heatmapPopulated = new Heatmap3()
        {
            { Point3.Zero, 5 },
            Vector3.Zero,
            { Point3.Zero, 12 },
            { Vector3.UnitX, 3 },
            { Point3.UnitY, 7 },
            { Point3.UnitZ, 11 },
        };

        public static Heatmap3 Copy(Heatmap3 heatmap)
        {
            var copy = new Heatmap3(heatmap.CellToPositionMatrix);
            foreach (var (cell, strength) in heatmap)
            {
                copy.Add(cell, strength);
            }
            return copy;
        }

        public static readonly TheoryData<Point3, int, int> CellStrength = new TheoryData<Point3, int, int>()
        {
            { Point3.Zero, 18, 4 },
            { Point3.UnitX, 3, 5 },
            { Point3.UnitY, 7, 6 },
            { Point3.UnitZ, 11, 7 },
        };

        [Theory]
        [MemberData(nameof(CellStrength))]
        public void Strength(Point3 cell, int strength, int _0)
        {
            var result = heatmapPopulated[cell];
            Assert.Equal(strength, result);
        }

        [Theory]
        [MemberData(nameof(CellStrength))]
        public void Add(Point3 cell, int strength, int add)
        {
            var heatmapPopulatedCopy = Copy(heatmapPopulated);
            heatmapPopulatedCopy.Add(cell, add);
            var result = heatmapPopulatedCopy[cell];
            Assert.Equal(strength + add, result);
        }

        [Fact]
        public void Add0_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () => heatmapPopulated.Add(Point3.Zero, 0));
        }

        [Theory]
        [MemberData(nameof(CellStrength))]
        public void Remove(Point3 cell, int strength, int remove)
        {
            var heatmapPopulatedCopy = Copy(heatmapPopulated);
            heatmapPopulatedCopy.Remove(cell, remove);
            var result = heatmapPopulatedCopy[cell];
            Assert.Equal(Math.Max(strength - remove, 0), result);
        }

        [Fact]
        public void Remove0_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("count", () => heatmapPopulated.Remove(Point3.Zero, 0));
        }
        #endregion
    }
}
