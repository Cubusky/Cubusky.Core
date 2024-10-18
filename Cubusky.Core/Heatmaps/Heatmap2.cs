using Cubusky.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 2D heatmap implementation.</summary>
    public class Heatmap2 : IHeatmap<Point2, Vector2>
    {
        private readonly Dictionary<Point2, int> strengthByCell = new Dictionary<Point2, int>();

        /// <inheritdoc />
        public int this[Point2 cell] => strengthByCell.GetValueOrDefault(cell);

        /// <inheritdoc cref="Heatmap3.CellToPositionMatrix" />
        public readonly Matrix3x2 CellToPositionMatrix = Matrix3x2.Identity;

        /// <inheritdoc cref="Heatmap3.PositionToCellMatrix" />
        public readonly Matrix3x2 PositionToCellMatrix = Matrix3x2.Identity;

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class.</summary>
        public Heatmap2() { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified transformation matrix.</summary>
        /// <param name="matrix">The transformation matrix. This equals the <see cref="CellToPositionMatrix"/> property. Its inverse equals the <see cref="PositionToCellMatrix"/> property.</param>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        public Heatmap2(Matrix3x2 matrix)
        {
            if (!Matrix3x2.Invert(CellToPositionMatrix = matrix, out PositionToCellMatrix))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(matrix));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified position, rotation, and scale.</summary>
        /// <param name="position">The heatmap's position.</param>
        /// <param name="radians">The heatmap's rotation.</param>
        /// <param name="scale">The heatmap's scale.</param>
        public Heatmap2(Vector2 position, float radians, float scale) : this(Matrix.CreateTransformation3x2(position, radians, scale)) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap2"/> class with the specified position, rotation, and scale.</summary>
        /// <param name="position">The heatmap's position.</param>
        /// <param name="radians">The heatmap's rotation.</param>
        /// <param name="scales">The heatmap's scales.</param>
        public Heatmap2(Vector2 position, float radians, Vector2 scales) : this(Matrix.CreateTransformation3x2(position, radians, scales)) { }

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

        /// <inheritdoc cref="Heatmap3.Add(Point3)" />
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

        /// <inheritdoc cref="Heatmap3.Contains(Point3)" />
        public bool Contains(Point2 cell) => strengthByCell.ContainsKey(cell);

        /// <inheritdoc cref="Heatmap3.CopyTo(Point3[], int)" />
        public void CopyTo(Point2[] array, int arrayIndex) => strengthByCell.Keys.CopyTo(array, arrayIndex);

        /// <inheritdoc cref="Heatmap3.Remove(Point3)" />
        public bool Remove(Point2 cell) => RemoveInternal(cell, 1);

        /// <inheritdoc />
        public bool Remove(Point2 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            return RemoveInternal(cell, strength);
        }

        internal bool RemoveInternal(Point2 cell, int strength) => strengthByCell.ContainsKey(cell)
            && ((strengthByCell[cell] -= strength) > 0 || strengthByCell.Remove(cell));

        /// <inheritdoc cref="Heatmap3.GetEnumerator" />
        public IEnumerator<Point2> GetEnumerator() => strengthByCell.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
