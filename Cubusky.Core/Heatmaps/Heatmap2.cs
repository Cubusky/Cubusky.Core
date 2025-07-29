using Cubusky.Collections.Generic;
using Cubusky.Heatmaps.Json.Serialization;
using Cubusky.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 2D heatmap implementation.</summary>
    [JsonConverter(typeof(Heatmap2JsonConverter))]
    public class Heatmap2 : Counter<Point2>, IHeatmap<Point2, Vector2>
    {
        /// <inheritdoc cref="Heatmap3.CellToPositionMatrix" />
        public readonly Matrix3x2 CellToPositionMatrix = Matrix3x2.Identity;

        /// <inheritdoc cref="Heatmap3.PositionToCellMatrix" />
        public readonly Matrix3x2 PositionToCellMatrix = Matrix3x2.Identity;

        private static void InvertMatrixOrThrow(Matrix3x2 cellToPositionMatrix, out Matrix3x2 inverse)
        {
            if (!Matrix3x2.Invert(cellToPositionMatrix, out inverse))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(cellToPositionMatrix));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class.</summary>
        public Heatmap2() { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(Matrix4x4, Swizzle3to2)" />
        public Heatmap2(Matrix3x2 cellToPositionMatrix)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified initial capacity.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Swizzle3to2)" />
        public Heatmap2(int capacity)
            : base(capacity) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified initial capacity and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(int capacity, Matrix3x2 cellToPositionMatrix)
            : base(capacity)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap2(IEqualityComparer<Point2>? comparer)
            : base(comparer) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(IEqualityComparer<Point2>? comparer, Matrix3x2 cellToPositionMatrix)
            : base(comparer)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, Swizzle3to2)" />
        public Heatmap2(IEnumerable<ItemCount<Point2>> collection)
            : base(collection) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection and with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(IEnumerable<ItemCount<Point2>> collection, Matrix3x2 cellToPositionMatrix)
            : base(collection)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified capacity and <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap2(int capacity, IEqualityComparer<Point2>? comparer)
            : base(capacity, comparer) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified capacity, <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(int capacity, IEqualityComparer<Point2>? comparer, Matrix3x2 cellToPositionMatrix)
            : base(capacity, comparer)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap2(IEnumerable<ItemCount<Point2>> collection, IEqualityComparer<Point2>? comparer)
            : base(collection, comparer) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(IEnumerable<ItemCount<Point2>> collection, IEqualityComparer<Point2>? comparer, Matrix3x2 cellToPositionMatrix)
            : base(collection, comparer)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <inheritdoc />
        public Point2 GetCell(Vector2 position)
        {
            position = Vector2.Transform(position, PositionToCellMatrix);
            return new Point2((int)MathF.Round(position.X), (int)MathF.Round(position.Y));
        }

        /// <inheritdoc />
        public Vector2 GetPosition(Point2 cell) => Vector2.Transform(cell, CellToPositionMatrix);
    }
}
