using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cubusky.Numerics
{
    /// <summary>Represents a bounds with four floating point values.</summary>
    public struct Bounds2 : IEquatable<Bounds2>, IFormattable
    {
        /// <summary>The X component of the bounds.</summary>
        public float X;

        /// <summary>The Y component of the bounds.</summary>
        public float Y;

        private float width;
        private float height;

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

        /// <summary>Creates a new <see cref="Bounds2" /> object whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="width">The value to assign to the <see cref="Width" /> field.</param>
        /// <param name="height">The value to assign to the <see cref="Height" /> field.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> or <paramref name="height"/> is negative.</exception>
        public Bounds2(float x, float y, float width, float height)
        {
            Throw.IfArgumentNegative(width, nameof(width));
            Throw.IfArgumentNegative(height, nameof(height));

            X = x;
            Y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>Creates a new <see cref="Bounds2" /> object from the specified <see cref="Vector2"/> objects.</summary>
        /// <param name="position">The value to assign to the <see cref="X"/> and <see cref="Y"/> fields.</param>
        /// <param name="size">The value to assign to the <see cref="Width"/> and <see cref="Height"/> fields.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="size"/> has an element that is negative.</exception>
        public Bounds2(Vector2 position, Vector2 size) : this(position.X, position.Y, size.X, size.Y) { }

        /// <summary>Constructs a bounds from the given <see cref="ReadOnlySpan{Int32}" />. The span must contain at least 4 elements.</summary>
        /// <param name="values">The span of elements to assign to the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the third or fourth element of the span is negative.</exception>
        public Bounds2(ReadOnlySpan<float> values)
        {
            Throw.IfArgumentLessThan(values.Length, 4, nameof(values));
            Throw.IfArgumentNegative(values[2], "values[2]");
            Throw.IfArgumentNegative(values[3], "values[3]");
            this = Unsafe.ReadUnaligned<Bounds2>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
        }

        /// <summary>Gets a bounds whose 4 elements are equal to zero.</summary>
        /// <value>A bounds whose four elements are equal to zero (that is, it returns the bounds <c>(0,0,0,0)</c>.</value>
        public static readonly Bounds2 Zero;

        /// <summary>Returns a value that indicates whether each pair of elements in two specified bounds is equal.</summary>
        /// <param name="left">The first bounds to compare.</param>
        /// <param name="right">The second bounds to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Bounds2" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Bounds2 left, Bounds2 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Width == right.Width)
                && (left.Height == right.Height);
        }

        /// <summary>Returns a value that indicates whether two specified bounds are not equal.</summary>
        /// <param name="left">The first bounds to compare.</param>
        /// <param name="right">The second bounds to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Bounds2 left, Bounds2 right) => !(left == right);

        /// <summary>Creates a new <see cref='Bounds2'/> object from the specified values.</summary>
        /// <param name="left">The x-coordinate of the top-left corner of the bounds.</param>
        /// <param name="top">The y-coordinate of the top-left corner of the bounds.</param>
        /// <param name="right">The x-coordinate of the bottom-right corner of the bounds.</param>
        /// <param name="bottom">The y-coordinate of the bottom-right corner of the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="right"/> or <paramref name="bottom"/> is less than <paramref name="left"/> or <paramref name="top"/> respectively.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2 FromEdges(float left, float top, float right, float bottom) => new Bounds2(left, top, checked(right - left), checked(bottom - top));

        /// <summary>Creates a new <see cref="Bounds2" /> object from the specified <see cref="Vector2"/> objects.</summary>
        /// <param name="topLeft">The coordinates of the top-left corner of the bounds.</param>
        /// <param name="bottomRight">The coordinates of the bottom-right corner of the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="bottomRight"/> is less than <paramref name="topLeft"/> in any dimension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2 FromEdges(Vector2 topLeft, Vector2 bottomRight) => FromEdges(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);

        /// <summary>Creates a new <see cref="Bounds2" /> object from the specified values.</summary>
        /// <param name="centerX">The x-coordinate of the center of the bounds.</param>
        /// <param name="centerY">The y-coordinate of the center of the bounds.</param>
        /// <param name="extentX">The extent from the center of the bounds on the x-axis.</param>
        /// <param name="extentY">The extent from the center of the bounds on the y-axis.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="extentX"/> or <paramref name="extentY"/> is negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2 FromCenter(float centerX, float centerY, float extentX, float extentY) => new Bounds2(checked(centerX - extentX), checked(centerY - extentY), checked(extentX * 2f), checked(extentY * 2f));

        /// <summary>Creates a new <see cref="Bounds2" /> object from the specified <see cref="Vector2"/> objects.</summary>
        /// <param name="center">The coordinates of the center of the bounds.</param>
        /// <param name="extents">The extents from the center of the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="extents"/> has an element that is negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2 FromCenter(Vector2 center, Vector2 extents) => FromCenter(center.X, center.Y, extents.X, extents.Y);

        /// <summary>Gets the coordinate of the top-left corner of the bounds.</summary>
        public Vector2 Position
        {
            readonly get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>Gets the size of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> has an element that is negative.</exception>
        public Vector2 Size
        {
            readonly get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        /// <summary>Gets the coordinate of the center of the bounds.</summary>
        public Vector2 Center
        {
            readonly get => new Vector2(checked(X + Width * 0.5f), checked(Y + Height * 0.5f));
            set
            {
                X = checked(value.X - Width * 0.5f);
                Y = checked(value.Y - Height * 0.5f);
            }
        }

        /// <summary>Gets the extents of the bounds.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> has an element that is negative.</exception>
        public Vector2 Extents
        {
            readonly get => new Vector2(Width * 0.5f, Height * 0.5f);
            set
            {
                X = checked(X + Width * 0.5f - value.X);
                Y = checked(Y + Height * 0.5f - value.Y);
                Width = checked(value.X * 2f);
                Height = checked(value.Y * 2f);
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

        /// <summary><see langword="true"/> if the <see cref="Width"/> and <see cref="Height"/> of the bounds are 0; otherwise, <see langword="false"/>.</summary>
        public readonly bool IsEmpty => Width == 0 && Height == 0;

        /// <summary>Creates a new <see cref="Bounds2"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="x">The value to adjust the <see cref="X"/> component by.</param>
        /// <param name="y">The value to adjust the <see cref="Y"/> component by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds2 Offset(float x, float y) => new Bounds2(checked(X + x), checked(Y + y), Width, Height);

        /// <summary>Creates a new <see cref="Bounds2"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Position"/> by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds2 Offset(Vector2 value) => Offset(value.X, value.Y);

        /// <summary>Creates a new <see cref="Bounds2"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="width">The value to adjust the <see cref="Width"/> component by.</param>
        /// <param name="height">The value to adjust the <see cref="Height"/> component by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> or <paramref name="height"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds2 Expand(float width, float height) => new Bounds2(X, Y, checked(Width + width), checked(Height + height));

        /// <summary>Creates a new <see cref="Bounds2"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Size"/> by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds2 Expand(Vector2 value) => Expand(value.X, value.Y);

        /// <summary>Creates a new <see cref="Bounds2"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="width">The value to adjust the <see cref="Width"/> component by.</param>
        /// <param name="height">The value to adjust the <see cref="Height"/> component by.</param>
        /// <param name="centerPointX">The x-coordinate of the point to adjust the <see cref="Size"/> from.</param>
        /// <param name="centerPointY">The y-coordinate of the point to adjust the <see cref="Size"/> from.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> or <paramref name="height"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds2 Expand(float width, float height, float centerPointX, float centerPointY) => new Bounds2(checked(X - (centerPointX - X) / Width * width), checked(Y - (centerPointY - Y) / Height * height), checked(Width + width), checked(Height + height));

        /// <summary>Creates a new <see cref="Bounds2"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Size"/> by.</param>
        /// <param name="centerPoint">The point to adjust the <see cref="Size"/> from.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Bounds2 Expand(Vector2 value, Vector2 centerPoint) => Expand(value.X, value.Y, centerPoint.X, centerPoint.Y);

        /// <summary>Determines whether the specified point is contained within the bounds.</summary>
        /// <param name="point">The point to locate within the bounds.</param>
        /// <returns><see langword="true"/> if the point is contained within the bounds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Vector2 point)
            => point.X >= Left
            && point.X <= Right
            && point.Y >= Top
            && point.Y <= Bottom;

        /// <summary>Determines whether the specified bounds is entirely contained within the bounds.</summary>
        /// <param name="other">The bounds to entirely locate within the bounds.</param>
        /// <returns><see langword="true"/> if the bounds is entirely contained within the bounds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Bounds2 other)
            => other.Left >= Left
            && other.Right <= Right
            && other.Top >= Top
            && other.Bottom <= Bottom;

        /// <summary>Determines whether the specified bounds intersects with the bounds.</summary>
        /// <param name="other">The bounds to locate intersecting with the bounds.</param>
        /// <returns><see langword="true"/> if the bounds intersects with the bounds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Intersects(Bounds2 other)
            => other.Left < Right
            && other.Right > Left
            && other.Top < Bottom
            && other.Bottom > Top;

        /// <summary>Returns the closest point between a specified bounds and point.</summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="point">The point.</param>
        /// <returns>The closest point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ClosestPoint(Bounds2 bounds, Vector2 point)
        {
            var x = Math.Clamp(point.X, bounds.Left, bounds.Right);
            var y = Math.Clamp(point.Y, bounds.Top, bounds.Bottom);
            return new Vector2(x, y);
        }

        /// <summary>Returns the Euclidean distance squared between a specified bounds and point.</summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Bounds2 bounds, Vector2 point) => Vector2.DistanceSquared(point, ClosestPoint(bounds, point));

        /// <summary>Computes the Euclidean distance squared between a specified bounds and point.</summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Bounds2 bounds, Vector2 point) => Vector2.Distance(point, ClosestPoint(bounds, point));

        /// <summary>Returns the encapsulation of the specified point within the bounds.</summary>
        /// <param name="bounds">The bounds to encapsulate the point within.</param>
        /// <param name="point">The point to encapsulate within the bounds.</param>
        /// <returns>The encapsulation of the specified point within the bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2 Encapsulate(Bounds2 bounds, Vector2 point)
        {
            var left = Math.Min(bounds.Left, point.X);
            var top = Math.Min(bounds.Top, point.Y);
            var right = Math.Max(bounds.Right, point.X);
            var bottom = Math.Max(bounds.Bottom, point.Y);

            return FromEdges(left, top, right, bottom);
        }

        /// <summary>Returns the intersection between the two specified boundses, or an empty bounds if they do not intersect.</summary>
        /// <param name="value1">The first bounds.</param>
        /// <param name="value2">The second bounds.</param>
        /// <returns>The intersection or an empty bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2 Intersect(Bounds2 value1, Bounds2 value2)
        {
            var left = Math.Max(value1.Left, value2.Left);
            var top = Math.Max(value1.Top, value2.Top);
            var right = Math.Min(value1.Right, value2.Right);
            var bottom = Math.Min(value1.Bottom, value2.Bottom);

            return right >= left && bottom >= top
                ? FromEdges(left, top, right, bottom)
                : Zero;
        }

        /// <summary>Returns the union of the two specified bounds.</summary>
        /// <param name="value1">The first bounds.</param>
        /// <param name="value2">The second bounds.</param>
        /// <returns>The union.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Bounds2 Union(Bounds2 value1, Bounds2 value2)
        {
            var left = Math.Min(value1.Left, value2.Left);
            var top = Math.Min(value1.Top, value2.Top);
            var right = Math.Max(value1.Right, value2.Right);
            var bottom = Math.Max(value1.Bottom, value2.Bottom);

            return FromEdges(left, top, right, bottom);
        }

        /// <summary>Copies the elements of the bounds to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least four elements. The method copies the bounds' elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(float[] array)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
            Throw.IfArgumentLessThan(array.Length, 4, nameof(array));
            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref array[0]), this);
        }

        /// <summary>Copies the elements of the bounds to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the bounds.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the four bounds elements. In other words, elements <paramref name="index" /> to <paramref name="index" /> + 4 must already exist in <paramref name="array" />.</remarks>
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
            Throw.IfArgumentLessThan(array.Length - index, 4);
            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref array[index]), this);
        }

        /// <summary>Copies the bounds to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source bounds is greater than those available in destination span.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<float> destination)
        {
            Throw.IfArgumentLessThan(destination.Length, 4, nameof(destination));
            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the bounds to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source bounds was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<float> destination)
        {
            if (destination.Length < 4)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);
            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Bounds2" /> object and their corresponding elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Bounds2 other && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another bounds are equal.</summary>
        /// <param name="other">The other bounds.</param>
        /// <returns><see langword="true" /> if the two bounds are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two bounds are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Width"/> and <see cref="Height"/> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Bounds2 other)
            => X.Equals(other.X)
            && Y.Equals(other.Y)
            && Width.Equals(other.Width)
            && Height.Equals(other.Height);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public readonly override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

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
            return $"{{{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}{separator} {Width.ToString(format, formatProvider)}{separator} {Height.ToString(format, formatProvider)}}}";
        }
    }
}
