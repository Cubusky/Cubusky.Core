using Cubusky.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 3D heatmap implementation.</summary>
    public class Heatmap3 : IHeatmap<Point3, Vector3>
    {
        private readonly Dictionary<Point3, int> strengthByCell = new Dictionary<Point3, int>();

        /// <inheritdoc />
        public int this[Point3 cell] => strengthByCell.GetValueOrDefault(cell);

        /// <summary>Gets the transformation matrix from cell space to position space.</summary>
        public readonly Matrix4x4 CellToPositionMatrix = Matrix4x4.Identity;

        /// <summary>Gets the transformation matrix from position space to cell space.</summary>
        public readonly Matrix4x4 PositionToCellMatrix = Matrix4x4.Identity;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class.</summary>
        public Heatmap3() { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified transformation matrix.</summary>
        /// <param name="matrix">The transformation matrix. This equals the <see cref="CellToPositionMatrix"/> property. Its inverse equals the <see cref="PositionToCellMatrix"/> property.</param>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        public Heatmap3(Matrix4x4 matrix)
        {
            if (!Matrix4x4.Invert(CellToPositionMatrix = matrix, out PositionToCellMatrix))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(matrix));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified position, rotation, and scale.</summary>
        /// <param name="position">The heatmap's position.</param>
        /// <param name="rotation">The heatmap's rotation.</param>
        /// <param name="scale">The heatmap's scale.</param>
        /// <exception cref="ArgumentException">Thrown when the resulting matrix cannot be inverted.</exception>
        public Heatmap3(Vector3 position, Quaternion rotation, float scale) : this(Matrix.CreateTransformation4x4(position, rotation, scale)) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3"/> class with the specified position, rotation, and scales.</summary>
        /// <param name="position">The heatmap's position.</param>
        /// <param name="rotation">The heatmap's rotation.</param>
        /// <param name="scales">The heatmap's scales.</param>
        /// <exception cref="ArgumentException">Thrown when the resulting matrix cannot be inverted.</exception>
        public Heatmap3(Vector3 position, Quaternion rotation, Vector3 scales) : this(Matrix.CreateTransformation4x4(position, rotation, scales)) { }

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

        /// <summary>Adds a strength at the specified cell.</summary>
        /// <param name="cell">The point representing the cell in the heatmap.</param>
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

        /// <summary>Removes a strength at the specified cell. If the remaining strength is 0 or less, the cell is removed from the heatmap.</summary>
        /// <param name="cell">The point representing the cell in the heatmap.</param>
        /// <returns><see langword="true" /> if the strength was successfully removed; otherwise, <see langword="false" />. This method returns <see langword="false" /> if <paramref name="cell"/> is not found in the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
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

        /// <summary>Determines whether the <see cref="IHeatmap{TPoint, TVector}"/> contains the specified cell.</summary>
        /// <param name="cell">The cell to check.</param>
        /// <returns><see langword="true" /> if the <see cref="IHeatmap{TPoint, TVector}"/> contains the specified cell; otherwise, <see langword="false" />.</returns>
        public bool Contains(Point3 cell) => strengthByCell.ContainsKey(cell);

        /// <summary>Copies the <see cref="IHeatmap{TPoint, TVector}"/> cells to an existing one-dimensional array, starting at the specified array index.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The starting index of the array.</param>
        public void CopyTo(Point3[] array, int arrayIndex) => strengthByCell.Keys.CopyTo(array, arrayIndex);

        /// <summary>Returns an enumerator that iterates through the cells of the <see cref="IHeatmap{TPoint, TVector}"/>.</summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the cells of the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        public IEnumerator<Point3> GetEnumerator() => strengthByCell.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
