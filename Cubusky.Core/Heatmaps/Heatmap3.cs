using Cubusky.Heatmaps.Json.Serialization;
using Cubusky.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 3D heatmap implementation.</summary>
    [JsonConverter(typeof(Heatmap3JsonConverter))]
    public class Heatmap3 : IHeatmap<Point3, Vector3>
    {
        private readonly Dictionary<Point3, int> strengthByCell;

        /// <inheritdoc />
        public int this[Point3 cell] => strengthByCell.GetValueOrDefault(cell);

        /// <summary>Gets the transformation matrix from cell space to position space.</summary>
        public readonly Matrix4x4 CellToPositionMatrix = Matrix4x4.Identity;

        /// <summary>Gets the transformation matrix from position space to cell space.</summary>
        public readonly Matrix4x4 PositionToCellMatrix = Matrix4x4.Identity;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class.</summary>
        public Heatmap3() => strengthByCell = new Dictionary<Point3, int>();

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(Matrix4x4, Swizzle3to2)" />
        public Heatmap3(Matrix4x4 cellToPositionMatrix) : this()
        {
            if (!Matrix4x4.Invert(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(cellToPositionMatrix));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified initial capacity.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Swizzle3to2)" />
        public Heatmap3(int capacity)
            => strengthByCell = new Dictionary<Point3, int>(capacity);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified initial capacity and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(int capacity, Matrix4x4 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point3, int>(capacity);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap3(IEqualityComparer<Point3>? comparer)
            => strengthByCell = new Dictionary<Point3, int>(comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(IEqualityComparer<Point3>? comparer, Matrix4x4 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point3, int>(comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, Swizzle3to2)" />
        public Heatmap3(IEnumerable<KeyValuePair<Point3, int>> collection)
            => strengthByCell = new Dictionary<Point3, int>(collection);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection and with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(IEnumerable<KeyValuePair<Point3, int>> collection, Matrix4x4 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point3, int>(collection);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified capacity and <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap3(int capacity, IEqualityComparer<Point3>? comparer)
            => strengthByCell = new Dictionary<Point3, int>(capacity, comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified capacity, <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(int capacity, IEqualityComparer<Point3>? comparer, Matrix4x4 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point3, int>(capacity, comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap3(IEnumerable<KeyValuePair<Point3, int>> collection, IEqualityComparer<Point3>? comparer)
            => strengthByCell = new Dictionary<Point3, int>(collection, comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap3(IEnumerable<KeyValuePair<Point3, int>> collection, IEqualityComparer<Point3>? comparer, Matrix4x4 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point3, int>(collection, comparer);

        /// <inheritdoc />
        public Point3 GetCell(Vector3 position)
        {
            position = Vector3.Transform(position, PositionToCellMatrix);
            return new Point3((int)MathF.Round(position.X), (int)MathF.Round(position.Y), (int)MathF.Round(position.Z));
        }

        /// <inheritdoc />
        public Vector3 GetPosition(Point3 cell) => Vector3.Transform(cell, CellToPositionMatrix);

        /// <summary>Gets the number of cells contained in the <see cref="IHeatmap{TPoint, TVector}"/>.</summary>
        public int Count => strengthByCell.Count;

        /// <summary>Gets a value indicating whether the heatmap is read-only.</summary>
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(Point3 cell) => AddInternal(cell, 1);

        /// <inheritdoc />
        public void Add(Point3 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            AddInternal(cell, strength);
        }

        internal void AddInternal(Point3 cell, int strength)
        {
            if (!strengthByCell.TryAdd(cell, strength))
            {
                strengthByCell[cell] += strength;
            }
        }

        /// <inheritdoc />
        public bool Remove(Point3 cell) => RemoveInternal(cell, 1);

        /// <inheritdoc />
        public bool Remove(Point3 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            return RemoveInternal(cell, strength);
        }

        internal bool RemoveInternal(Point3 cell, int strength) => strengthByCell.ContainsKey(cell)
            && ((strengthByCell[cell] -= strength) > 0 || strengthByCell.Remove(cell));

        /// <summary>Removes all cells from the <see cref="IHeatmap{TPoint, TVector}"/>.</summary>
        public void Clear() => strengthByCell.Clear();

        /// <inheritdoc />
        public bool Contains(Point3 cell) => strengthByCell.ContainsKey(cell);

        /// <inheritdoc />
        public bool Contains(Point3 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            return strengthByCell.TryGetValue(cell, out var value) && value == strength;
        }

        /// <summary>Copies the <see cref="IHeatmap{TPoint, TVector}"/> cells to an existing one-dimensional array, starting at the specified array index.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The starting index of the array.</param>
        public void CopyTo(KeyValuePair<Point3, int>[] array, int arrayIndex) => ((ICollection<KeyValuePair<Point3, int>>)strengthByCell).CopyTo(array, arrayIndex);

        /// <summary>Returns an enumerator that iterates through the cells of the <see cref="IHeatmap{TPoint, TVector}"/>.</summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the cells of the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        public IEnumerator<KeyValuePair<Point3, int>> GetEnumerator() => strengthByCell.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
