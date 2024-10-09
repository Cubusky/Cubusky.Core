using Cubusky.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Cubusky.Heatmaps
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Heatmap2 : IHeatmap<Point2, Vector2>
    {
        private readonly Dictionary<Point2, int> strengthByCell = new Dictionary<Point2, int>();

        public int this[Point2 cell] => strengthByCell.GetValueOrDefault(cell);

        public readonly Matrix4x4 CellToPosition = Matrix4x4.Identity;
        public readonly Matrix4x4 PositionToCell = Matrix4x4.Identity;

        public Heatmap2() { }

        public Heatmap2(Matrix3x2 matrix) : this(new Matrix4x4(matrix)) { }
        public Heatmap2(Vector2 position, float radians, float scale) : this(Matrix3x2Extensions.CreateTransformation(position, radians, scale)) { }
        public Heatmap2(Vector2 position, float radians, Vector2 scales) : this(Matrix3x2Extensions.CreateTransformation(position, radians, scales)) { }

        public Heatmap2(Matrix4x4 matrix)
        {
            if (!Matrix4x4.Invert(this.CellToPosition = matrix, out PositionToCell))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(matrix));
            }
        }
        public Heatmap2(Vector3 position, Quaternion quaternion, float scale) : this(Matrix4x4Extensions.CreateTransformation(position, quaternion, scale)) { }
        public Heatmap2(Vector3 position, Quaternion quaternion, Vector3 scales) : this(Matrix4x4Extensions.CreateTransformation(position, quaternion, scales)) { }

        public Point2 GetCell(Vector2 position)
        {
            position = Vector2.Transform(position, PositionToCell);
            return new Point2((int)MathF.Round(position.X), (int)MathF.Round(position.Y));
        }

        public Vector2 GetPosition(Point2 cell) => Vector2.Transform(cell, CellToPosition);

        public int Count => strengthByCell.Count;
        public bool IsReadOnly => false;

        public void Add(Point2 cell) => AddInternal(cell, 1);
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

        public void Clear() => strengthByCell.Clear();
        public bool Contains(Point2 cell) => strengthByCell.ContainsKey(cell);
        public void CopyTo(Point2[] array, int arrayIndex) => strengthByCell.Keys.CopyTo(array, arrayIndex);

        public bool Remove(Point2 cell) => RemoveInternal(cell, 1);
        public bool Remove(Point2 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            return RemoveInternal(cell, strength);
        }

        internal bool RemoveInternal(Point2 cell, int strength) => strengthByCell.ContainsKey(cell)
            && ((strengthByCell[cell] -= strength) > 0 || strengthByCell.Remove(cell));

        public IEnumerator<Point2> GetEnumerator() => strengthByCell.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
