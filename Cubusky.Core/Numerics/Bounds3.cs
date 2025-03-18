using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cubusky.Numerics
{
    /// <summary>Represents a bounds with six floating point values.</summary>
    public struct Bounds3 : IEquatable<Bounds3>, IFormattable
    {
        /// <summary>The X component of the bounds.</summary>
        public float X;

        /// <summary>The Y component of the bounds.</summary>
        public float Y;

        /// <summary>The Z component of the bounds.</summary>
        public float Z;

        private float width;
        private float height;
        private float depth;

        /// <summary>The width of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.</exception>
        public float Width
        {
            readonly get => width;
            set
            {
                Throw.IfArgumentNegative(value, nameof(Width));
                width = value;
            }
        }

        /// <summary>The height of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.</exception>
        public float Height
        {
            readonly get => height;
            set
            {
                Throw.IfArgumentNegative(value, nameof(Height));
                height = value;
            }
        }

        /// <summary>The depth of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.</exception>
        public float Depth
        {
            readonly get => depth;
            set
            {
                Throw.IfArgumentNegative(value, nameof(Depth));
                depth = value;
            }
        }

        /// <summary>Creates a new <see cref="Bounds3" /> object whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
        /// <param name="width">The value to assign to the <see cref="Width" /> field.</param>
        /// <param name="height">The value to assign to the <see cref="Height" /> field.</param>
        /// <param name="depth">The value to assign to the <see cref="Depth" /> field.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/>, <paramref name="height"/> or <paramref name="depth"/> is negative.</exception>
        public Bounds3(float x, float y, float z, float width, float height, float depth)
        {
            Throw.IfArgumentNegative(width, nameof(width));
            Throw.IfArgumentNegative(height, nameof(height));
            Throw.IfArgumentNegative(depth, nameof(depth));

            X = x;
            Y = y;
            Z = z;
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        /// <summary>Creates a new <see cref="Bounds3"/> object from the specified <see cref="Bounds2"/> object and the specified values.</summary>
        /// <param name="value">The bounds with four elements.</param>
        /// <param name="z">The additional value to assign to the <see cref="Z"/> field.</param>
        /// <param name="depth">The additional value to assign to the <see cref="Depth"/> field.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depth"/> is negative.</exception>
        public Bounds3(Bounds2 value, float z, float depth) : this(value.X, value.Y, z, value.Width, value.Height, depth) { }

        /// <summary>Creates a new <see cref="Bounds3" /> object from the specified <see cref="Vector3"/> objects.</summary>
        /// <param name="position">The value to assign to the <see cref="X"/>, <see cref="Y"/> and <see cref="Z"/> fields.</param>
        /// <param name="size">The value to assign to the <see cref="Width"/>, <see cref="Height"/> and <see cref="Depth"/> fields.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="size"/> has an element that is negative.</exception>
        public Bounds3(Vector3 position, Vector3 size) : this(position.X, position.Y, position.Z, size.X, size.Y, size.Z) { }

        /// <summary>Constructs a bounds from the given <see cref="ReadOnlySpan{Int32}" />. The span must contain at least 6 elements.</summary>
        /// <param name="values">The span of elements to assign to the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the fourth, fifth or the sixth element of the span is negative.</exception>
        public Bounds3(ReadOnlySpan<float> values)
        {
            Throw.IfArgumentLessThan(values.Length, 6, nameof(values));
            Throw.IfArgumentNegative(values[3], "values[3]");
            Throw.IfArgumentNegative(values[4], "values[4]");
            Throw.IfArgumentNegative(values[5], "values[5]");
            this = Unsafe.ReadUnaligned<Bounds3>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
        }

        /// <summary>Gets a bounds whose 6 elements are equal to zero.</summary>
        /// <value>A bounds whose six elements are equal to zero (that is, it returns the bounds <c>(0,0,0,0,0,0)</c>.</value>
        public static readonly Bounds3 Zero;

        /// <summary>Returns a value that indicates whether each pair of elements in two specified bounds is equal.</summary>
        /// <param name="left">The first bounds to compare.</param>
        /// <param name="right">The second bounds to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Bounds3" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Bounds3 left, Bounds3 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z)
                && (left.Width == right.Width)
                && (left.Height == right.Height)
                && (left.Depth == right.Depth);
        }

        /// <summary>Returns a value that indicates whether two specified bounds are not equal.</summary>
        /// <param name="left">The first bounds to compare.</param>
        /// <param name="right">The second bounds to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Bounds3 left, Bounds3 right) => !(left == right);

        /// <summary>Creates a new <see cref='Bounds3'/> object from the specified values.</summary>
        /// <param name="left">The x-coordinate of the front-top-left corner of the bounds.</param>
        /// <param name="top">The y-coordinate of the front-top-left corner of the bounds.</param>
        /// <param name="front">The z-coordinate of the front-top-left corner of the bounds.</param>
        /// <param name="right">The x-coordinate of the back-bottom-right corner of the bounds.</param>
        /// <param name="bottom">The y-coordinate of the back-bottom-right corner of the bounds.</param>
        /// <param name="back">The z-coordinate of the back-bottom-right corner of the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="right"/>, <paramref name="bottom"/> or <paramref name="back"/> is less than <paramref name="left"/>, <paramref name="top"/> or <paramref name="front"/> respectively.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds3 FromEdges(float left, float top, float front, float right, float bottom, float back) => new Bounds3(left, top, front, checked(right - left), checked(bottom - top), checked(back - front));

        /// <summary>Creates a new <see cref="Bounds3" /> object from the specified <see cref="Vector3"/> objects.</summary>
        /// <param name="frontTopLeft">The coordinates of the front-top-left corner of the box.</param>
        /// <param name="backBottomRight">The coordinates of the back-bottom-right corner of the box.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="backBottomRight"/> is less than <paramref name="frontTopLeft"/> in any dimension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds3 FromEdges(Vector3 frontTopLeft, Vector3 backBottomRight) => FromEdges(frontTopLeft.X, frontTopLeft.Y, frontTopLeft.Z, backBottomRight.X, backBottomRight.Y, backBottomRight.Z);

        /// <summary>Creates a new <see cref="Bounds3" /> object from the specified values.</summary>
        /// <param name="centerX">The x-coordinate of the center of the bounds.</param>
        /// <param name="centerY">The y-coordinate of the center of the bounds.</param>
        /// <param name="centerZ">The z-coordinate of the center of the bounds.</param>
        /// <param name="extentX">The extent from the center of the bounds on the x-axis.</param>
        /// <param name="extentY">The extent from the center of the bounds on the y-axis.</param>
        /// <param name="extentZ">The extent from the center of the bounds on the z-axis.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="extentX"/>, <paramref name="extentY"/> or <paramref name="extentZ"/> is negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds3 FromCenter(float centerX, float centerY, float centerZ, float extentX, float extentY, float extentZ) => new Bounds3(checked(centerX - extentX), checked(centerY - extentY), checked(centerZ - extentZ), checked(extentX * 2f), checked(extentY * 2f), checked(extentZ * 2f));

        /// <summary>Creates a new <see cref="Bounds3" /> object from the specified <see cref="Vector3"/> objects.</summary>
        /// <param name="center">The coordinates of the center of the bounds.</param>
        /// <param name="extents">The extents from the center of the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="extents"/> has an element that is negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds3 FromCenter(Vector3 center, Vector3 extents) => FromCenter(center.X, center.Y, center.Z, extents.X, extents.Y, extents.Z);

        /// <summary>Gets the coordinate of the front-top-left corner of the bounds.</summary>
        public Vector3 Position
        {
            readonly get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>Gets the size of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> has an element that is negative.</exception>
        public Vector3 Size
        {
            readonly get => new Vector3(Width, Height, Depth);
            set
            {
                Width = value.X;
                Height = value.Y;
                Depth = value.Z;
            }
        }

        /// <summary>Gets the coordinate of the center of the bounds.</summary>
        public Vector3 Center
        {
            readonly get => new Vector3(checked(X + Width * 0.5f), checked(Y + Height * 0.5f), checked(Z + Depth * 0.5f));
            set
            {
                X = checked(value.X - Width * 0.5f);
                Y = checked(value.Y - Height * 0.5f);
                Z = checked(value.Z - Depth * 0.5f);
            }
        }

        /// <summary>Gets the extents of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> has an element that is negative.</exception>
        public Vector3 Extents
        {
            readonly get => new Vector3(Width * 0.5f, Height * 0.5f, Depth * 0.5f);
            set
            {
                X = checked(X + Width * 0.5f - value.X);
                Y = checked(Y + Height * 0.5f - value.Y);
                Z = checked(Z + Depth * 0.5f - value.Z);
                Width = checked(value.X * 2f);
                Height = checked(value.Y * 2f);
                Depth = checked(value.Z * 2f);
            }
        }

        /// <summary>Gets the x-coordinate of the left edge of the bounds.</summary>
        public float Left
        {
            readonly get => X;
            set => X = value;
        }

        /// <summary>Gets the y-coordinate of the top edge of the bounds.</summary>
        public float Top
        {
            readonly get => Y;
            set => Y = value;
        }

        /// <summary>Gets the z-coordinate of the front edge of the bounds.</summary>
        public float Front
        {
            readonly get => Z;
            set => Z = value;
        }

        /// <summary>Gets the x-coordinate of the right edge of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Left"/>.</exception>
        public float Right
        {
            readonly get => checked(X + Width);
            set => Width = checked(value - X);
        }

        /// <summary>Gets the y-coordinate of the bottom edge of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Top"/>.</exception>
        public float Bottom
        {
            readonly get => checked(Y + Height);
            set => Height = checked(value - Y);
        }

        /// <summary>Gets the z-coordinate of the back edge of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Front"/>.</exception>
        public float Back
        {
            readonly get => checked(Z + Depth);
            set => Depth = checked(value - Z);
        }

        /// <summary><see langword="true"/> if the <see cref="Width"/>, <see cref="Height"/> and <see cref="Depth"/> of the bounds are 0; otherwise, <see langword="false"/>.</summary>
        public readonly bool IsEmpty => Width == 0 && Height == 0 && Depth == 0;

        /// <summary>Creates a new <see cref="Bounds3"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="x">The value to adjust the <see cref="X"/> component by.</param>
        /// <param name="y">The value to adjust the <see cref="Y"/> component by.</param>
        /// <param name="z">The value to adjust the <see cref="Z"/> component by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds3 Offset(float x, float y, float z) => new Bounds3(checked(X + x), checked(Y + y), checked(Z + z), Width, Height, Depth);

        /// <summary>Creates a new <see cref="Bounds3"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Position"/> by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds3 Offset(Vector3 value) => Offset(value.X, value.Y, value.Z);

        /// <summary>Creates a new <see cref="Bounds3"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="width">The value to adjust the <see cref="Width"/> component by.</param>
        /// <param name="height">The value to adjust the <see cref="Height"/> component by.</param>
        /// <param name="depth">The value to adjust the <see cref="Depth"/> component by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/>, <paramref name="height"/> or <paramref name="depth"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds3 Expand(float width, float height, float depth) => new Bounds3(X, Y, Z, checked(Width + width), checked(Height + height), checked(Depth - depth));

        /// <summary>Creates a new <see cref="Bounds3"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Size"/> by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds3 Expand(Vector3 value) => Expand(value.X, value.Y, value.Z);

        /// <summary>Creates a new <see cref="Bounds3"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="width">The value to adjust the <see cref="Width"/> component by.</param>
        /// <param name="height">The value to adjust the <see cref="Height"/> component by.</param>
        /// <param name="depth">The value to adjust the <see cref="Depth"/> component by.</param>
        /// <param name="centerPointX">The x-coordinate of the point to adjust the <see cref="Size"/> from.</param>
        /// <param name="centerPointY">The y-coordinate of the point to adjust the <see cref="Size"/> from.</param>
        /// <param name="centerPointZ">The z-coordinate of the point to adjust the <see cref="Size"/> from.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/>, <paramref name="height"/> or <paramref name="depth"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds3 Expand(float width, float height, float depth, float centerPointX, float centerPointY, float centerPointZ) => new Bounds3(checked(X - (centerPointX - X) / Width * width), checked(Y - (centerPointY - Y) / Height * height), checked(Z - (centerPointZ - Z) / Depth * depth), checked(Width + width), checked(Height + height), checked(Depth + depth));

        /// <summary>Creates a new <see cref="Bounds3"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Size"/> by.</param>
        /// <param name="centerPoint">The point to adjust the <see cref="Size"/> from.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds3 Expand(Vector3 value, Vector3 centerPoint) => Expand(value.X, value.Y, value.Z, centerPoint.X, centerPoint.Y, centerPoint.Z);

        /// <summary>Determines whether the specified point is contained within the bounds.</summary>
        /// <param name="point">The point to locate within the bounds.</param>
        /// <returns><see langword="true"/> if the point is contained within the bounds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Vector3 point)
            => point.X >= Left
            && point.X <= Right
            && point.Y >= Top
            && point.Y <= Bottom
            && point.Z >= Front
            && point.Z <= Back;

        /// <summary>Determines whether the specified bounds is entirely contained within the bounds.</summary>
        /// <param name="other">The bounds to entirely locate within the bounds.</param>
        /// <returns><see langword="true"/> if the bounds is entirely contained within the bounds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Bounds3 other)
            => other.Left >= Left
            && other.Right <= Right
            && other.Top >= Top
            && other.Bottom <= Bottom
            && other.Front >= Front
            && other.Back <= Back;

        /// <summary>Determines whether the specified bounds intersects with the bounds.</summary>
        /// <param name="other">The bounds to locate intersecting with the bounds.</param>
        /// <returns><see langword="true"/> if the bounds intersects with the bounds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Intersects(Bounds3 other)
            => other.Left < Right
            && other.Right > Left
            && other.Top < Bottom
            && other.Bottom > Top
            && other.Front < Back
            && other.Back > Front;

        /// <summary>Returns the closest point between a specified bounds and point.</summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="point">The point.</param>
        /// <returns>The closest point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ClosestPoint(Bounds3 bounds, Vector3 point)
        {
            var x = Math.Clamp(point.X, bounds.Left, bounds.Right);
            var y = Math.Clamp(point.Y, bounds.Top, bounds.Bottom);
            var z = Math.Clamp(point.Z, bounds.Front, bounds.Back);
            return new Vector3(x, y, z);
        }

        /// <summary>Returns the Euclidean distance squared between a specified bounds and point.</summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Bounds3 bounds, Vector3 point) => Vector3.DistanceSquared(point, ClosestPoint(bounds, point));

        /// <summary>Computes the Euclidean distance squared between a specified bounds and point.</summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Bounds3 bounds, Vector3 point) => Vector3.Distance(point, ClosestPoint(bounds, point));

        /// <summary>Returns the encapsulation of the specified point within the bounds.</summary>
        /// <param name="bounds">The bounds to encapsulate the point within.</param>
        /// <param name="point">The point to encapsulate within the bounds.</param>
        /// <returns>The encapsulation of the specified point within the bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds3 Encapsulate(Bounds3 bounds, Vector3 point)
        {
            var left = Math.Min(bounds.Left, point.X);
            var top = Math.Min(bounds.Top, point.Y);
            var front = Math.Min(bounds.Front, point.Z);
            var right = Math.Max(bounds.Right, point.X);
            var bottom = Math.Max(bounds.Bottom, point.Y);
            var back = Math.Max(bounds.Back, point.Z);

            return FromEdges(left, top, front, right, bottom, back);
        }

        /// <summary>Returns the intersection between the two specified boundses, or an empty bounds if they do not intersect.</summary>
        /// <param name="value1">The first bounds.</param>
        /// <param name="value2">The second bounds.</param>
        /// <returns>The intersection or an empty bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds3 Intersect(Bounds3 value1, Bounds3 value2)
        {
            var left = Math.Max(value1.Left, value2.Left);
            var top = Math.Max(value1.Top, value2.Top);
            var front = Math.Max(value1.Front, value2.Front);
            var right = Math.Min(value1.Right, value2.Right);
            var bottom = Math.Min(value1.Bottom, value2.Bottom);
            var back = Math.Min(value1.Back, value2.Back);

            return right >= left && bottom >= top && back >= front
                ? FromEdges(left, top, front, right, bottom, back)
                : Zero;
        }

        /// <summary>Returns the union of the two specified bounds.</summary>
        /// <param name="value1">The first bounds.</param>
        /// <param name="value2">The second bounds.</param>
        /// <returns>The union.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds3 Union(Bounds3 value1, Bounds3 value2)
        {
            var left = Math.Min(value1.Left, value2.Left);
            var top = Math.Min(value1.Top, value2.Top);
            var front = Math.Min(value1.Front, value2.Front);
            var right = Math.Max(value1.Right, value2.Right);
            var bottom = Math.Max(value1.Bottom, value2.Bottom);
            var back = Math.Max(value1.Back, value2.Back);

            return FromEdges(left, top, front, right, bottom, back);
        }

        /// <summary>Copies the elements of the bounds to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least six elements. The method copies the bounds' elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(float[] array)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
            Throw.IfArgumentLessThan(array.Length, 6, nameof(array));
            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref array[0]), this);
        }

        /// <summary>Copies the elements of the bounds to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the bounds.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the six bounds elements. In other words, elements <paramref name="index" /> to <paramref name="index" /> + 6 must already exist in <paramref name="array" />.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
        /// -or-
        /// <paramref name="index" /> is greater than or equal to the array length.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(float[] array, int index)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
            Throw.IfArgumentGreaterThanOrEqual((uint)index, (uint)array.Length, nameof(index));
            Throw.IfArgumentLessThan(array.Length - index, 6);
            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref array[index]), this);
        }

        /// <summary>Copies the bounds to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 6.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source bounds is greater than those available in destination span.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<float> destination)
        {
            Throw.IfArgumentLessThan(destination.Length, 6, nameof(destination));
            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the bounds to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 6.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source bounds was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<float> destination)
        {
            if (destination.Length < 6)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);
            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Bounds3" /> object and their corresponding elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Bounds3 other && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another bounds are equal.</summary>
        /// <param name="other">The other bounds.</param>
        /// <returns><see langword="true" /> if the two bounds are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two bounds are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Z"/>, <see cref="Width"/>, <see cref="Height"/> and <see cref="Depth"/> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Bounds3 other)
            => X.Equals(other.X)
            && Y.Equals(other.Y)
            && Z.Equals(other.Z)
            && Width.Equals(other.Width)
            && Height.Equals(other.Height)
            && Depth.Equals(other.Depth);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public readonly override int GetHashCode() => HashCode.Combine(X, Y, Z, Width, Height, Depth);

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the position and size of the bounds is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "{" and "}" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public readonly override string ToString() => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the position and size of the bounds is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "{" and "}" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
#if NET7_0_OR_GREATER
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => ToString(format, CultureInfo.CurrentCulture);
#else
        public readonly string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);
#endif

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the position and size of the bounds is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "{" and "}" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
#if NET7_0_OR_GREATER
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
#else
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
#endif
        {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            return $"{{{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}{separator} {Z.ToString(format, formatProvider)}{separator} {Width.ToString(format, formatProvider)}{separator} {Height.ToString(format, formatProvider)}{separator} {Depth.ToString(format, formatProvider)}}}";
        }
    }
}
