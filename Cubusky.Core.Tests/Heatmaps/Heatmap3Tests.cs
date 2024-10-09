using Cubusky.Numerics;
using System;
using System.Numerics;
using Xunit;

namespace Cubusky.Heatmaps.Tests
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
        public static readonly Heatmap3 heatmapTransformed = new Heatmap3(position, quaternion, scales);

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

        //public static Heatmap3 Copy(Heatmap3 heatmap)
        //{
        //    var copy = new Heatmap3(heatmap.CellToPosition);
        //    foreach (var cell in heatmap)
        //    {
        //        copy.Add(cell, heatmap[cell]);
        //    }
        //    return copy;
        //}

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
            var heatmapPopulatedCopy = new Heatmap3(heatmapPopulated.CellToPosition)
            {
                heatmapPopulated,
                { cell, add },
            };
            var result = heatmapPopulatedCopy[cell];
            Assert.Equal(strength + add, result);
        }

        [Fact]
        public void Add0_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("strength", () => heatmapPopulated.Add(Point3.Zero, 0));
        }

        [Theory]
        [MemberData(nameof(CellStrength))]
        public void Remove(Point3 cell, int strength, int remove)
        {
            var heatmapPopulatedCopy = new Heatmap3(heatmapPopulated.CellToPosition)
            {
                heatmapPopulated,
            };
            heatmapPopulatedCopy.Remove(cell, remove);
            var result = heatmapPopulatedCopy[cell];
            Assert.Equal(Math.Max(strength - remove, 0), result);
        }

        [Fact]
        public void Remove0_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("strength", () => heatmapPopulated.Remove(Point3.Zero, 0));
        }
        #endregion

        #region Compact Array / Multidimensional Array
        public static readonly Heatmap3 heatmapCube = new Heatmap3()
        {
            { new Point3(0, 0, 0), 1 },

            { new Point3(1, 0, 0), 2 },
            { new Point3(0, 1, 0), 3 },
            { new Point3(0, 0, 1), 4 },
            { new Point3(1, 1, 0), 5 },
            { new Point3(1, 0, 1), 6 },
            { new Point3(0, 1, 1), 7 },
            { new Point3(1, 1, 1), 8 },

            { new Point3(-1, 0, 0), 9 },
            { new Point3(0, -1, 0), 10 },
            { new Point3(0, 0, -1), 11 },
            { new Point3(-1, -1, 0), 12 },
            { new Point3(-1, 0, -1), 13 },
            { new Point3(0, -1, -1), 14 },
            { new Point3(-1, -1, -1), 15 },

            { new Point3(1, -1, 0), 16 },
            { new Point3(-1, 1, 0), 17 },
            { new Point3(1, 0, -1), 18 },
            { new Point3(-1, 0, 1), 19 },
            { new Point3(0, 1, -1), 20 },
            { new Point3(0, -1, 1), 21 },
            { new Point3(1, -1, -1), 22 },
            { new Point3(-1, 1, -1), 23 },
            { new Point3(-1, -1, 1), 24 },

            { new Point3(1, 1, -1), 25 },
            { new Point3(1, -1, 1), 26 },
            { new Point3(-1, 1, 1), 27 },
        };

        public static readonly int[] compactCubeArray = new int[108]
        {
            0, 0, 0, 1,

            1, 0, 0, 2,
            0, 1, 0, 3,
            0, 0, 1, 4,
            1, 1, 0, 5,
            1, 0, 1, 6,
            0, 1, 1, 7,
            1, 1, 1, 8,

            -1, 0, 0, 9,
            0, -1, 0, 10,
            0, 0, -1, 11,
            -1, -1, 0, 12,
            -1, 0, -1, 13,
            0, -1, -1, 14,
            -1, -1, -1, 15,

            1, -1, 0, 16,
            -1, 1, 0, 17,
            1, 0, -1, 18,
            -1, 0, 1, 19,
            0, 1, -1, 20,
            0, -1, 1, 21,
            1, -1, -1, 22,
            -1, 1, -1, 23,
            -1, -1, 1, 24,

            1, 1, -1, 25,
            1, -1, 1, 26,
            -1, 1, 1, 27,
        };

        public static readonly int[,,] multidimensionalCubeArray = new int[3, 3, 3]
        {
            {
                { 15, 12, 24 },
                { 13, 9, 19 },
                { 23, 17, 27 },
            },
            {
                { 14, 10, 21 },
                { 11, 1, 4 },
                { 20, 3, 7 },
            },
            {
                { 22, 16, 26 },
                { 18, 2, 6 },
                { 25, 5, 8 },
            }
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
            var result = new Heatmap3(heatmapCube.CellToPosition)
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
            Assert.Equal(-Point3.One, offset);
        }

        [Fact]
        public void AddMultidimensionalArray()
        {
            var result = new Heatmap3(heatmapCube.CellToPosition)
            {
                { multidimensionalCubeArray, -Point3.One }
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
            var result = new Heatmap3(heatmapCube.CellToPosition)
            {
                compactCubeArray,
                { multidimensionalCubeArray, -Point3.One }
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
