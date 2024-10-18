using Cubusky.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a 2D heatmap implementation in 3D space.</summary>
    public class Heatmap3to2 : IHeatmap<Point2, Vector3>
    {
        private readonly Dictionary<Point2, int> strengthByCell = new Dictionary<Point2, int>();

        /// <inheritdoc />
        public int this[Point2 cell] => strengthByCell.GetValueOrDefault(cell);

        /// <inheritdoc cref="Heatmap3.CellToPositionMatrix" />
        public readonly Matrix4x4 CellToPositionMatrix = Matrix4x4.Identity;

        /// <inheritdoc cref="Heatmap3.PositionToCellMatrix" />
        public readonly Matrix4x4 PositionToCellMatrix = Matrix4x4.Identity;

        /// <summary>Gets the swizzle to use when transforming between positions and cells.</summary>
        public readonly Swizzle3to2 swizzle;

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class.</summary>
        /// <param name="swizzle">The swizzle to use when transforming between positions and cells.</param>
        public Heatmap3to2(Swizzle3to2 swizzle = Swizzle3to2.XY)
        {
            this.swizzle = swizzle;
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified transformation matrix.</summary>
        /// <param name="matrix">The transformation matrix. This equals the <see cref="CellToPositionMatrix"/> property. Its inverse equals the <see cref="PositionToCellMatrix"/> property.</param>
        /// <param name="swizzle">The swizzle to use when transforming between positions and cells.</param>
        /// <exception cref="ArgumentException">Thrown when the matrix cannot be inverted.</exception>
        public Heatmap3to2(Matrix4x4 matrix, Swizzle3to2 swizzle = Swizzle3to2.XY) : this(swizzle)
        {
            if (!Matrix4x4.Invert(CellToPositionMatrix = matrix, out PositionToCellMatrix))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(matrix));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified position, rotation, and scale.</summary>
        /// <param name="position">The heatmap's position.</param>
        /// <param name="rotation">The heatmap's rotation.</param>
        /// <param name="scale">The heatmap's scale.</param>
        /// <param name="swizzle">The swizzle to use when transforming between positions and cells.</param>
        /// <exception cref="ArgumentException">Thrown when the resulting matrix cannot be inverted.</exception>
        public Heatmap3to2(Vector3 position, Quaternion rotation, float scale, Swizzle3to2 swizzle = Swizzle3to2.XY) : this(Matrix.CreateTransformation4x4(position, rotation, scale), swizzle) { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3to2"/> class with the specified position, rotation, and scales.</summary>
        /// <param name="position">The heatmap's position.</param>
        /// <param name="rotation">The heatmap's rotation.</param>
        /// <param name="scales">The heatmap's scales.</param>
        /// <param name="swizzle">The swizzle to use when transforming between positions and cells.</param>
        /// <exception cref="ArgumentException">Thrown when the resulting matrix cannot be inverted.</exception>
        public Heatmap3to2(Vector3 position, Quaternion rotation, Vector3 scales, Swizzle3to2 swizzle = Swizzle3to2.XY) : this(Matrix.CreateTransformation4x4(position, rotation, scales), swizzle) { }

        /// <inheritdoc />
        public Point2 GetCell(Vector3 position)
        {
            position = Vector3.Transform(position, PositionToCellMatrix);
            return swizzle switch
            {
                Swizzle3to2.XY => new Point2((int)MathF.Round(position.X), (int)MathF.Round(position.Y)),
                Swizzle3to2.XZ => new Point2((int)MathF.Round(position.X), (int)MathF.Round(position.Z)),
                Swizzle3to2.YZ => new Point2((int)MathF.Round(position.Y), (int)MathF.Round(position.Z)),
                _ => throw new InvalidEnumArgumentException(nameof(swizzle), (int)swizzle, typeof(Swizzle3to2)),
            };
        }

        /// <inheritdoc />
        public Vector3 GetPosition(Point2 cell) => swizzle switch
        {
            Swizzle3to2.XY => Vector3.Transform(new Vector3(cell.X, cell.Y, 0f), CellToPositionMatrix),
            Swizzle3to2.XZ => Vector3.Transform(new Vector3(cell.X, 0f, cell.Y), CellToPositionMatrix),
            Swizzle3to2.YZ => Vector3.Transform(new Vector3(0f, cell.X, cell.Y), CellToPositionMatrix),
            _ => throw new InvalidEnumArgumentException(nameof(swizzle), (int)swizzle, typeof(Swizzle3to2)),
        };

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
