using Cubusky.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Cubusky.Heatmaps
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Heatmap3 : IHeatmap<Point3, Vector3>
    {
        private readonly Dictionary<Point3, int> strengthByCell = new Dictionary<Point3, int>();

        public int this[Point3 cell] => strengthByCell.GetValueOrDefault(cell);

        public readonly Matrix4x4 CellToPosition = Matrix4x4.Identity;
        public readonly Matrix4x4 PositionToCell = Matrix4x4.Identity;

        public Heatmap3() { }
        public Heatmap3(Matrix4x4 matrix)
        {
            if (!Matrix4x4.Invert(this.CellToPosition = matrix, out PositionToCell))
            {
                throw new ArgumentException("The matrix cannot be inverted.", nameof(matrix));
            }
        }
        public Heatmap3(Vector3 position, Quaternion quaternion, float scale) : this(Matrix4x4Extensions.CreateTransformation(position, quaternion, scale)) { }
        public Heatmap3(Vector3 position, Quaternion quaternion, Vector3 scales) : this(Matrix4x4Extensions.CreateTransformation(position, quaternion, scales)) { }

        public Point3 GetCell(Vector3 position)
        {
            position = Vector3.Transform(position, PositionToCell);
            return new Point3((int)MathF.Round(position.X), (int)MathF.Round(position.Y), (int)MathF.Round(position.Z));
        }

        public Vector3 GetPosition(Point3 cell) => Vector3.Transform(cell, CellToPosition);

        public int Count => strengthByCell.Count;
        public bool IsReadOnly => false;

        public void Add(Point3 cell) => AddInternal(cell, 1);
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

        public void Clear() => strengthByCell.Clear();
        public bool Contains(Point3 cell) => strengthByCell.ContainsKey(cell);
        public void CopyTo(Point3[] array, int arrayIndex) => strengthByCell.Keys.CopyTo(array, arrayIndex);

        public bool Remove(Point3 cell) => RemoveInternal(cell, 1);
        public bool Remove(Point3 cell, int strength)
        {
            Throw.IfArgumentNegativeOrZero(strength, nameof(strength));
            return RemoveInternal(cell, strength);
        }

        internal bool RemoveInternal(Point3 cell, int strength) => strengthByCell.ContainsKey(cell)
            && ((strengthByCell[cell] -= strength) > 0 || strengthByCell.Remove(cell));

        public IEnumerator<Point3> GetEnumerator() => strengthByCell.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
