using Cubusky.Collections.Generic;
using Cubusky.Heatmaps.Json.Serialization;
using Cubusky.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 3D heatmap implementation.</summary>
    [JsonConverter(typeof(Heatmap3JsonConverter))]
    public class Heatmap3 : Counter<Point3>, IHeatmap<Point3, Vector3>
    {
        /// <summary>Gets the transformation matrix from cell space to position space.</summary>
        public readonly Matrix4x4 CellToPositionMatrix = Matrix4x4.Identity;

        /// <summary>Gets the transformation matrix from position space to cell space.</summary>
        public readonly Matrix4x4 PositionToCellMatrix = Matrix4x4.Identity;

        private static void InvertMatrixOrThrow(Matrix4x4 cellToPositionMatrix, out Matrix4x4 inverse)
        {
            if (!Matrix4x4.Invert(cellToPositionMatrix, out inverse))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(cellToPositionMatrix));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class.</summary>
        public Heatmap3() { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(Matrix4x4, Swizzle3to2)" />
        public Heatmap3(Matrix4x4 cellToPositionMatrix)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified initial capacity.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Swizzle3to2)" />
        public Heatmap3(int capacity)
            : base(capacity) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified initial capacity and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(int capacity, Matrix4x4 cellToPositionMatrix)
            : base(capacity)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap3(IEqualityComparer<Point3>? comparer)
            : base(comparer) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(IEqualityComparer<Point3>? comparer, Matrix4x4 cellToPositionMatrix)
            : base(comparer)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, Swizzle3to2)" />
        public Heatmap3(IEnumerable<ItemCount<Point3>> collection)
            : base(collection) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection and with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(IEnumerable<ItemCount<Point3>> collection, Matrix4x4 cellToPositionMatrix)
            : base(collection)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified capacity and <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap3(int capacity, IEqualityComparer<Point3>? comparer)
            : base(capacity, comparer) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified capacity, <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(int capacity, IEqualityComparer<Point3>? comparer, Matrix4x4 cellToPositionMatrix)
            : base(capacity, comparer)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap3(IEnumerable<ItemCount<Point3>> collection, IEqualityComparer<Point3>? comparer)
            : base(collection, comparer) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(IEnumerable<ItemCount<Point3>> collection, IEqualityComparer<Point3>? comparer, Matrix4x4 cellToPositionMatrix)
            : base(collection, comparer)
            => InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix);

        /// <inheritdoc />
        public Point3 GetCell(Vector3 position)
        {
            position = Vector3.Transform(position, PositionToCellMatrix);
            return new Point3((int)MathF.Round(position.X), (int)MathF.Round(position.Y), (int)MathF.Round(position.Z));
        }

        /// <inheritdoc />
        public Vector3 GetPosition(Point3 cell) => Vector3.Transform(cell, CellToPositionMatrix);
    }
}
