using Cubusky.Heatmaps;
using Cubusky.Numerics;
using System;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Heatmaps
{
    public class Heatmap2Tests
    {
        #region Get Cell / Get Position
        public static readonly Vector2 position = Vector2.One * 10f;
        public static readonly float radians = MathF.PI * 0.5f;
        public static readonly Vector2 scales = new Vector2(0.25f, 2f);

        public static readonly Heatmap2 heatmapTranslated = new Heatmap2(Matrix3x2.CreateTranslation(position));
        public static readonly Heatmap2 heatmapRotated = new Heatmap2(Matrix3x2.CreateRotation(radians));
        public static readonly Heatmap2 heatmapScaled = new Heatmap2(Matrix3x2.CreateScale(scales));
        public static readonly Heatmap2 heatmapTransformed = new Heatmap2(Matrix.CreateTransformation3x2(position, radians, scales));

        public static readonly TheoryData<Heatmap2, Vector2, Point2> HeatmapPositionCell = new TheoryData<Heatmap2, Vector2, Point2>()
        {
            { heatmapTranslated, position, Point2.Zero },
            { heatmapRotated, position, new Point2(10, -10) },
            { heatmapScaled, position, new Point2(40, 5) },
            { heatmapTransformed, new Vector2(50, -45), new Point2(-220, -20) },
        };

        [Theory]
        [MemberData(nameof(HeatmapPositionCell))]
        public void GetCell(Heatmap2 heatmap, Vector2 position, Point2 cell)
        {
            var result = heatmap.GetCell(position);
            Assert.Equal(cell, result);
        }

        [Theory]
        [MemberData(nameof(HeatmapPositionCell))]
        public void GetPosition(Heatmap2 heatmap, Vector2 position, Point2 cell)
        {
            var result = heatmap.GetPosition(cell);
            Assert.Equal(position.X, result.X, 1E-5f);
            Assert.Equal(position.Y, result.Y, 1E-5f);
        }
        #endregion

        #region Strength / Add / Remove
        public static readonly Heatmap2 heatmapPopulated = new Heatmap2()
        {
            { Point2.Zero, 5 },
            Vector2.Zero,
            { Point2.Zero, 12 },
            { Vector2.UnitX, 3 },
            { Point2.UnitY, 7 },
        };

        public static Heatmap2 Copy(Heatmap2 heatmap)
        {
            var copy = new Heatmap2(heatmap.CellToPositionMatrix);
            foreach (var (cell, strength) in heatmap)
            {
                copy.Add(cell, strength);
            }
            return copy;
        }

        public static readonly TheoryData<Point2, int, int> CellStrength = new TheoryData<Point2, int, int>()
        {
            { Point2.Zero, 18, 4 },
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
            Assert.Throws<ArgumentOutOfRangeException>("count", () => heatmapPopulated.Add(Point2.Zero, 0));
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
            Assert.Throws<ArgumentOutOfRangeException>("count", () => heatmapPopulated.Remove(Point2.Zero, 0));
        }
        #endregion
    }
}
