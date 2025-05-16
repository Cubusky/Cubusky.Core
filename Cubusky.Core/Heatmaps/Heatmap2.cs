using Cubusky.Heatmaps.Json.Serialization;
using Cubusky.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 2D heatmap implementation.</summary>
    [JsonConverter(typeof(Heatmap2JsonConverter))]
    public class Heatmap2 : IHeatmap<Point2, Vector2>
    {
        private readonly Dictionary<Point2, int> strengthByCell;

        /// <inheritdoc />
        public int this[Point2 cell] => strengthByCell.GetValueOrDefault(cell);

        /// <inheritdoc cref="Heatmap3.CellToPositionMatrix" />
        public readonly Matrix3x2 CellToPositionMatrix = Matrix3x2.Identity;

        /// <inheritdoc cref="Heatmap3.PositionToCellMatrix" />
        public readonly Matrix3x2 PositionToCellMatrix = Matrix3x2.Identity;

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class.</summary>
        public Heatmap2() => strengthByCell = new Dictionary<Point2, int>();

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(Matrix4x4, Swizzle3to2)" />
        public Heatmap2(Matrix3x2 cellToPositionMatrix) : this()
        {
            if (!Matrix3x2.Invert(CellToPositionMatrix = cellToPositionMatrix, out PositionToCellMatrix))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(cellToPositionMatrix));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified initial capacity.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Swizzle3to2)" />
        public Heatmap2(int capacity)
            => strengthByCell = new Dictionary<Point2, int>(capacity);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified initial capacity and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(int capacity, Matrix3x2 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point2, int>(capacity);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap2(IEqualityComparer<Point2>? comparer)
            => strengthByCell = new Dictionary<Point2, int>(comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(IEqualityComparer<Point2>? comparer, Matrix3x2 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point2, int>(comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, Swizzle3to2)" />
        public Heatmap2(IEnumerable<KeyValuePair<Point2, int>> collection)
            => strengthByCell = new Dictionary<Point2, int>(collection);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection and with the specified transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(IEnumerable<KeyValuePair<Point2, int>> collection, Matrix3x2 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point2, int>(collection);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified capacity and <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap2(int capacity, IEqualityComparer<Point2>? comparer)
            => strengthByCell = new Dictionary<Point2, int>(capacity, comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified capacity, <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(int, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(int capacity, IEqualityComparer<Point2>? comparer, Matrix3x2 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point2, int>(capacity, comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, IEqualityComparer{Point2}?, Swizzle3to2)" />
        public Heatmap2(IEnumerable<KeyValuePair<Point2, int>> collection, IEqualityComparer<Point2>? comparer)
            => strengthByCell = new Dictionary<Point2, int>(collection, comparer);

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class that contains the elements copied from the specified collection and with the specified <see cref="IEqualityComparer{T}"/> and transformation matrix.</summary>
        /// <inheritdoc cref="Heatmap3to2(IEnumerable{KeyValuePair{Point2, int}}, IEqualityComparer{Point2}?, Matrix4x4, Swizzle3to2)" />
        public Heatmap2(IEnumerable<KeyValuePair<Point2, int>> collection, IEqualityComparer<Point2>? comparer, Matrix3x2 cellToPositionMatrix)
            : this(cellToPositionMatrix)
            => strengthByCell = new Dictionary<Point2, int>(collection, comparer);

        /// <inheritdoc />
        public Point2 GetCell(Vector2 position)
        {
            position = Vector2.Transform(position, PositionToCellMatrix);
            return new Point2((int)MathF.Round(position.X), (int)MathF.Round(position.Y));
        }

        /// <inheritdoc />
        public Vector2 GetPosition(Point2 cell) => Vector2.Transform(cell, CellToPositionMatrix);

        /// <inheritdoc cref="Heatmap3.Count" />
        public int Count => strengthByCell.Count;

        /// <inheritdoc cref="Heatmap3.IsReadOnly" />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(Point2 cell) => AddInternal(cell, 1);

        /// <inheritdoc />
        public void Add(Point2 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            AddInternal(cell, strength);
        }

        internal void AddInternal(Point2 cell, int strength)
        {
            if (!strengthByCell.TryAdd(cell, strength))
            {
                strengthByCell[cell] += strength;
            }
        }

        /// <inheritdoc cref="Heatmap3.Clear" />
        public void Clear() => strengthByCell.Clear();

        /// <inheritdoc />
        public bool Contains(Point2 cell) => strengthByCell.ContainsKey(cell);

        /// <inheritdoc />
        public bool Contains(Point2 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            return strengthByCell.TryGetValue(cell, out var value) && value == strength;
        }

        /// <inheritdoc cref="Heatmap3.CopyTo(KeyValuePair{Point3, int}[], int)" />
        public void CopyTo(KeyValuePair<Point2, int>[] array, int arrayIndex) => ((ICollection<KeyValuePair<Point2, int>>)strengthByCell).CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(Point2 cell) => RemoveInternal(cell, 1);

        /// <inheritdoc />
        public bool Remove(Point2 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            return RemoveInternal(cell, strength);
        }

        internal bool RemoveInternal(Point2 cell, int strength) => strengthByCell.ContainsKey(cell)
            && ((strengthByCell[cell] -= strength) > 0 || strengthByCell.Remove(cell));

        /// <summary>Sets the capacity of this heatmap to what it would be if it had been originally initialized with all its entries.</summary>
        public void TrimExcess() => strengthByCell.TrimExcess();

        /// <summary>Sets the capacity of this heatmap to hold up a specified number of entries without any further expansion of its backing storage.</summary>
        /// <exception cref="ArgumentOutOfRangeException" />
        public void TrimExcess(int capacity) => strengthByCell.TrimExcess(capacity);

        /// <inheritdoc cref="Heatmap3.GetEnumerator" />
        public IEnumerator<KeyValuePair<Point2, int>> GetEnumerator() => strengthByCell.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
