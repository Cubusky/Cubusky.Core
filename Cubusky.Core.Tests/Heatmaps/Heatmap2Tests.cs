using Cubusky.Numerics;
using System;
using System.Numerics;
using Xunit;

namespace Cubusky.Heatmaps.Tests
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
        public static readonly Heatmap2 heatmapTransformed = new Heatmap2(position, radians, scales);

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
            foreach (var cell in heatmap)
            {
                copy.Add(cell, heatmap[cell]);
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

        #region Compact Array / Multidimensional Array
        public static readonly Heatmap2 heatmapCube = new Heatmap2()
        {
            { new Point2(1, 0), 2 },
            { new Point2(0, 1), 3 },
            { new Point2(1, 1), 4 },

            { new Point2(-1, 0), 5 },
            { new Point2(0, -1), 6 },
            { new Point2(-1, -1), 7 },

            { new Point2(1, -1), 8 },
            { new Point2(-1, 1), 9 },
        };

        public static readonly int[] compactCubeArray = new int[24]
        {
            1, 0, 2,
            0, 1, 3,
            1, 1, 4,

            -1, 0, 5,
            0, -1, 6,
            -1, -1, 7,

            1, -1, 8,
            -1, 1, 9,
        };

        public static readonly int[,] multidimensionalCubeArray = new int[3, 3]
        {
            { 7, 5, 9 },
            { 6, 0, 3 },
            { 8, 2, 4 },
        };

        [Fact]
        public void ToCompactArray()
        {
            var result = heatmapCube.ToCompactArray();
            Assert.Equal(compactCubeArray, result);
        }

        [Fact]
        public void AddCompactArray()
        {
            var result = new Heatmap2(heatmapCube.CellToPositionMatrix)
            {
                compactCubeArray
            };

            Assert.Equal(heatmapCube.Count, result.Count);
            foreach (var cell in heatmapCube)
            {
                Assert.Equal(heatmapCube[cell], result[cell]);
            }
        }

        [Fact]
        public void ToMultidimensionalArray()
        {
            var result = heatmapCube.ToMultidimensionalArray(out var offset);
            Assert.Equal(multidimensionalCubeArray, result);
            Assert.Equal(-Point2.One, offset);
        }

        [Fact]
        public void AddMultidimensionalArray()
        {
            var result = new Heatmap2(heatmapCube.CellToPositionMatrix)
            {
                { multidimensionalCubeArray, -Point2.One }
            };

            Assert.Equal(heatmapCube.Count, result.Count);
            foreach (var cell in heatmapCube)
            {
                Assert.Equal(heatmapCube[cell], result[cell]);
            }
        }

        [Fact]
        public void AddCompactAndMultidimensionalArray()
        {
            var result = new Heatmap2(heatmapCube.CellToPositionMatrix)
            {
                compactCubeArray,
                { multidimensionalCubeArray, -Point2.One }
            };

            Assert.Equal(heatmapCube.Count, result.Count);
            foreach (var cell in heatmapCube)
            {
                Assert.Equal(heatmapCube[cell] * 2, result[cell]);
            }
        }
        #endregion
    }
}
