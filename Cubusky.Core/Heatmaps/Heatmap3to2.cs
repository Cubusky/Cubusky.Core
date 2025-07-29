using Cubusky.Collections.Generic;
using Cubusky.Heatmaps.Json.Serialization;
using Cubusky.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 2D heatmap implementation in 3D space.</summary>
    [JsonConverter(typeof(Heatmap3to2JsonConverter))]
    public class Heatmap3to2 : Counter<Point2>, IHeatmap<Point2, Vector3>
    {
        /// <inheritdoc cref="Heatmap3.CellToPositionMatrix" />
        public readonly Matrix4x4 CellToPositionMatrix = Matrix4x4.Identity;

        /// <inheritdoc cref="Heatmap3.PositionToCellMatrix" />
        public readonly Matrix4x4 PositionToCellMatrix = Matrix4x4.Identity;

        private static Matrix4x4 InvertMatrixOrThrow(Matrix4x4 cellToPositionMatrix)
            => !Matrix4x4.Invert(cellToPositionMatrix, out Matrix4x4 inverse)
                ? throw new ArgumentException("The matrix cannot be inverted.", nameof(cellToPositionMatrix))
                : inverse;

        /// <summary>Gets the swizzle to use when transforming between positions and cells.</summary>
        public readonly Swizzle3to2 Swizzle;

        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(Swizzle3to2 swizzle = Swizzle3to2.XY)
            => this.Swizzle = swizzle;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified transformation matrix.</summary>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(Matrix4x4 cellToPositionMatrix, Swizzle3to2 swizzle = Swizzle3to2.XY)
            => (PositionToCellMatrix, Swizzle) = (InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix), swizzle);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified initial capacity.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(int capacity, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(capacity)
            => this.Swizzle = swizzle;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified initial capacity and transformation matrix.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(int capacity, Matrix4x4 cellToPositionMatrix, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(capacity)
            => (PositionToCellMatrix, Swizzle) = (InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix), swizzle);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(IEqualityComparer<Point2>? comparer, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(comparer)
            => this.Swizzle = swizzle;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(IEqualityComparer<Point2>? comparer, Matrix4x4 cellToPositionMatrix, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(comparer)
            => (PositionToCellMatrix, Swizzle) = (InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix), swizzle);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class that contains the elements copied from the specified collection.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(IEnumerable<ItemCount<Point2>> collection, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(collection)
            => this.Swizzle = swizzle;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class that contains the elements copied from the specified collection and with the specified transformation matrix.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(IEnumerable<ItemCount<Point2>> collection, Matrix4x4 cellToPositionMatrix, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(collection)
            => (PositionToCellMatrix, Swizzle) = (InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix), swizzle);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified capacity and <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(int capacity, IEqualityComparer<Point2>? comparer, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(capacity, comparer)
            => this.Swizzle = swizzle;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified capacity, <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(int capacity, IEqualityComparer<Point2>? comparer, Matrix4x4 cellToPositionMatrix, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(capacity, comparer)
            => (PositionToCellMatrix, Swizzle) = (InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix), swizzle);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(IEnumerable<ItemCount<Point2>> collection, IEqualityComparer<Point2>? comparer, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(collection, comparer)
            => this.Swizzle = swizzle;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        /// <inheritdoc cref="Heatmap_doc(int, IEnumerable{ItemCount{Point2}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3to2(IEnumerable<ItemCount<Point2>> collection, IEqualityComparer<Point2>? comparer, Matrix4x4 cellToPositionMatrix, Swizzle3to2 swizzle = Swizzle3to2.XY)
            : base(collection, comparer)
            => (PositionToCellMatrix, Swizzle) = (InvertMatrixOrThrow(CellToPositionMatrix = cellToPositionMatrix), swizzle);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class.</summary>
        /// <param name="capacity">The initial number of elements that the internal <see cref="Dictionary{TKey, TValue}"/> can contain.</param>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the internal <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing items, or null to use the default <see cref="EqualityComparer{T}"/> for the type of the item.</param>
        /// <param name="cellToPositionMatrix">The transformation matrix. This equals the <see cref="CellToPositionMatrix"/> property. Its inverse equals the <see cref="PositionToCellMatrix"/> property.</param>
        /// <param name="swizzle">The swizzle to use when transforming between positions and cells.</param>
        internal static void Heatmap_doc(int capacity, IEnumerable<ItemCount<Point2>> collection, IEqualityComparer<Point2>? comparer, Matrix4x4 cellToPositionMatrix, Swizzle3to2 swizzle = Swizzle3to2.XY) { }

        /// <inheritdoc />
        public Point2 GetCell(Vector3 position)
        {
            position = Vector3.Transform(position, PositionToCellMatrix);
            return Swizzle switch
            {
                Swizzle3to2.XY => new Point2((int)MathF.Round(position.X), (int)MathF.Round(position.Y)),
                Swizzle3to2.XZ => new Point2((int)MathF.Round(position.X), (int)MathF.Round(position.Z)),
                Swizzle3to2.YZ => new Point2((int)MathF.Round(position.Y), (int)MathF.Round(position.Z)),
                _ => throw new InvalidEnumArgumentException(nameof(Swizzle), (int)Swizzle, typeof(Swizzle3to2)),
            };
        }

        /// <inheritdoc />
        public Vector3 GetPosition(Point2 cell) => Swizzle switch
        {
            Swizzle3to2.XY => Vector3.Transform(new Vector3(cell.X, cell.Y, 0f), CellToPositionMatrix),
            Swizzle3to2.XZ => Vector3.Transform(new Vector3(cell.X, 0f, cell.Y), CellToPositionMatrix),
            Swizzle3to2.YZ => Vector3.Transform(new Vector3(0f, cell.X, cell.Y), CellToPositionMatrix),
            _ => throw new InvalidEnumArgumentException(nameof(Swizzle), (int)Swizzle, typeof(Swizzle3to2)),
        };
    }
}
