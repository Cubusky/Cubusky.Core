using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Cubusky.Numerics
{
    /// <summary>Provides a collection of static methods for creating, manipulating, and otherwise operating on generic points.</summary>
    public static partial class Point
    {
        /// <summary>Gets the element at the specified index.</summary>
        /// <param name="point">The point to get the element from.</param>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>The value of the element at <paramref name="index" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetElement(this Point2 point, int index)
        {
            Throw.IfArgumentGreaterThanOrEqual((uint)index, (uint)Point2.Count, nameof(index));
            return point.GetElementUnsafe(index);
        }

        /// <summary>Creates a new <see cref="Point2" /> with the element at the specified index set to the specified value and the remaining elements set to the same value as that in the given point.</summary>
        /// <param name="point">The point to get the remaining elements from.</param>
        /// <param name="index">The index of the element to set.</param>
        /// <param name="value">The value to set the element to.</param>
        /// <returns>A <see cref="Point2" /> with the value of the element at <paramref name="index" /> set to <paramref name="value" /> and the remaining elements set to the same value as that in <paramref name="point" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
        internal static Point2 WithElement(this Point2 point, int index, int value)
        {
            Throw.IfArgumentGreaterThanOrEqual((uint)index, (uint)Point2.Count, nameof(index));

            Point2 result = point;
            result.SetElementUnsafe(index, value);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetElementUnsafe(in this Point2 point, int index)
        {
            Debug.Assert((index >= 0) && (index < Point2.Count));
            ref int address = ref Unsafe.AsRef(in point.X);
            return Unsafe.Add(ref address, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetElementUnsafe(ref this Point2 point, int index, int value)
        {
            Debug.Assert((index >= 0) && (index < Point2.Count));
            Unsafe.Add(ref point.X, index) = value;
        }
    }
}
