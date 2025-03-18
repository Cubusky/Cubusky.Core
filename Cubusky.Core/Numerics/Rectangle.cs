using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cubusky.Numerics
{
    /// <summary>Represents a rectangle with four integer values.</summary>
    public struct Rectangle : IEquatable<Rectangle>, IFormattable
    {
        /// <summary>The X component of the rectangle.</summary>
        public int X;

        /// <summary>The Y component of the rectangle.</summary>
        public int Y;

        private int width;
        private int height;

        /// <summary>The width of the rectangle.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.</exception>
        public int Width
        {
            readonly get => width;
            set
            {
                Throw.IfArgumentNegative(value, nameof(Width));
                width = value;
            }
        }

        /// <summary>The height of the rectangle.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.</exception>
        public int Height
        {
            readonly get => height;
            set
            {
                Throw.IfArgumentNegative(value, nameof(Height));
                height = value;
            }
        }

        /// <summary>Creates a new <see cref="Rectangle" /> object whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="width">The value to assign to the <see cref="Width" /> field.</param>
        /// <param name="height">The value to assign to the <see cref="Height" /> field.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> or <paramref name="height"/> is negative.</exception>
        public Rectangle(int x, int y, int width, int height)
        {
            Throw.IfArgumentNegative(width, nameof(width));
            Throw.IfArgumentNegative(height, nameof(height));

            X = x;
            Y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>Creates a new <see cref="Rectangle" /> object from the specified <see cref="Point2"/> objects.</summary>
        /// <param name="position">The value to assign to the <see cref="X"/> and <see cref="Y"/> fields.</param>
        /// <param name="size">The value to assign to the <see cref="Width"/> and <see cref="Height"/> fields.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="size"/> has an element that is negative.</exception>
        public Rectangle(Point2 position, Point2 size) : this(position.X, position.Y, size.X, size.Y) { }

        /// <summary>Constructs a rectangle from the given <see cref="ReadOnlySpan{Int32}" />. The span must contain at least 4 elements.</summary>
        /// <param name="values">The span of elements to assign to the rectangle.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the third or fourth element of the span is negative.</exception>
        public Rectangle(ReadOnlySpan<int> values)
        {
            Throw.IfArgumentLessThan(values.Length, 4, nameof(values));
            Throw.IfArgumentNegative(values[2], "values[2]");
            Throw.IfArgumentNegative(values[3], "values[3]");
            this = Unsafe.ReadUnaligned<Rectangle>(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(values)));
        }

        /// <summary>Gets a rectangle whose 4 elements are equal to zero.</summary>
        /// <value>A rectangle whose four elements are equal to zero (that is, it returns the rectangle <c>(0,0,0,0)</c>.</value>
        public static readonly Rectangle Zero;

        /// <summary>Returns a value that indicates whether each pair of elements in two specified rectangles is equal.</summary>
        /// <param name="left">The first rectangle to compare.</param>
        /// <param name="right">The second rectangle to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Rectangle" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Width == right.Width)
                && (left.Height == right.Height);
        }

        /// <summary>Returns a value that indicates whether two specified rectangles are not equal.</summary>
        /// <param name="left">The first rectangle to compare.</param>
        /// <param name="right">The second rectangle to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);

        /// <summary>Defines an implicit conversion of a given <see cref="Rectangle"/> to a <see cref="Bounds2"/>.</summary>
        /// <param name="value">The <see cref="Rectangle"/> to implicitly convert.</param>
        /// <returns>A <see cref="Bounds2"/> instance converted from the <paramref name="value"/> parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Bounds2(Rectangle value) => new Bounds2(value.X, value.Y, value.Width, value.Height);

        /// <summary>Defines an explicit conversion of a given <see cref="Bounds2"/> to a <see cref="Rectangle"/>.</summary>
        /// <param name="value">The <see cref="Bounds2"/> to explicitly convert.</param>
        /// <returns>A <see cref="Rectangle"/> instance converted from the <paramref name="value"/> parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rectangle(Bounds2 value) => new Rectangle((int)value.X, (int)value.Y, (int)value.Width, (int)value.Height);

        /// <summary>Creates a new <see cref='Rectangle'/> object from the specified values.</summary>
        /// <param name="left">The x-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="top">The y-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="right">The x-coordinate of the bottom-right corner of the rectangle.</param>
        /// <param name="bottom">The y-coordinate of the bottom-right corner of the rectangle.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="right"/> or <paramref name="bottom"/> is less than <paramref name="left"/> or <paramref name="top"/> respectively.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle FromEdges(int left, int top, int right, int bottom) => new Rectangle(left, top, checked(right - left), checked(bottom - top));

        /// <summary>Creates a new <see cref="Rectangle" /> object from the specified <see cref="Point2"/> objects.</summary>
        /// <param name="topLeft">The coordinates of the top-left corner of the rectangle.</param>
        /// <param name="bottomRight">The coordinates of the bottom-right corner of the rectangle.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="bottomRight"/> is less than <paramref name="topLeft"/> in any dimension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle FromEdges(Point2 topLeft, Point2 bottomRight) => FromEdges(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);

        /// <summary>Gets the coordinate of the top-left corner of the rectangle.</summary>
        public Point2 Position
        {
            readonly get => new Point2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>Gets the size of the rectangle.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> has an element that is negative.</exception>
        public Point2 Size
        {
            readonly get => new Point2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        /// <summary>Gets the x-coordinate of the left edge of the rectangle.</summary>
        public int Left
        {
            readonly get => X;
            set => X = value;
        }

        /// <summary>Gets the y-coordinate of the top edge of the rectangle.</summary>
        public int Top
        {
            readonly get => Y;
            set => Y = value;
        }

        /// <summary>Gets the x-coordinate of the right edge of the rectangle.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Left"/>.</exception>
        public int Right
        {
            readonly get => checked(X + Width);
            set => Width = checked(value - X);
        }

        /// <summary>Gets the y-coordinate of the bottom edge of the rectangle.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Top"/>.</exception>
        public int Bottom
        {
            readonly get => checked(Y + Height);
            set => Height = checked(value - Y);
        }

        /// <summary><see langword="true"/> if the <see cref="Width"/> and <see cref="Height"/> of the rectangle are 0; otherwise, <see langword="false"/>.</summary>
        public readonly bool IsEmpty => Width == 0 && Height == 0;

        /// <summary>Creates a new <see cref="Rectangle"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="x">The value to adjust the <see cref="X"/> component by.</param>
        /// <param name="y">The value to adjust the <see cref="Y"/> component by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Rectangle Offset(int x, int y) => new Rectangle(checked(X + x), checked(Y + y), Width, Height);

        /// <summary>Creates a new <see cref="Rectangle"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Position"/> by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Rectangle Offset(Point2 value) => Offset(value.X, value.Y);

        /// <summary>Creates a new <see cref="Rectangle"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="width">The value to adjust the <see cref="Width"/> component by.</param>
        /// <param name="height">The value to adjust the <see cref="Height"/> component by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/> or <paramref name="height"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Rectangle Expand(int width, int height) => new Rectangle(X, Y, checked(Width + width), checked(Height + height));

        /// <summary>Creates a new <see cref="Rectangle"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Size"/> by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Rectangle Expand(Point2 value) => Expand(value.X, value.Y);

        /// <summary>Determines whether the specified point is contained within the rectangle.</summary>
        /// <param name="point">The point to locate within the rectangle.</param>
        /// <returns><see langword="true"/> if the point is contained within the rectangle; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Point2 point)
            => point.X >= Left
            && point.X <= Right
            && point.Y >= Top
            && point.Y <= Bottom;

        /// <summary>Determines whether the specified rectangle is entirely contained within the rectangle.</summary>
        /// <param name="other">The rectangle to entirely locate within the rectangle.</param>
        /// <returns><see langword="true"/> if the rectangle is entirely contained within the rectangle; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Rectangle other)
            => other.Left >= Left
            && other.Right <= Right
            && other.Top >= Top
            && other.Bottom <= Bottom;

        /// <summary>Determines whether the specified rectangle intersects with the rectangle.</summary>
        /// <param name="other">The rectangle to locate intersecting with the rectangle.</param>
        /// <returns><see langword="true"/> if the rectangle intersects with the rectangle; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Intersects(Rectangle other)
            => other.Left < Right
            && other.Right > Left
            && other.Top < Bottom
            && other.Bottom > Top;

        /// <summary>Returns the closest point between a specified rectangle and point.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="point">The point.</param>
        /// <returns>The closest point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ClosestPoint(Rectangle rectangle, Vector2 point)
        {
            var x = Math.Clamp(point.X, rectangle.Left, rectangle.Right);
            var y = Math.Clamp(point.Y, rectangle.Top, rectangle.Bottom);
            return new Vector2(x, y);
        }

        /// <summary>Returns the Euclidean distance squared between a specified rectangle and point.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Rectangle rectangle, Vector2 point) => Vector2.DistanceSquared(point, ClosestPoint(rectangle, point));

        /// <summary>Computes the Euclidean distance squared between a specified rectangle and point.</summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Rectangle rectangle, Vector2 point) => Vector2.Distance(point, ClosestPoint(rectangle, point));

        /// <summary>Returns the encapsulation of the specified point within the rectangle.</summary>
        /// <param name="rectangle">The rectangle to encapsulate the point within.</param>
        /// <param name="point">The point to encapsulate within the rectangle.</param>
        /// <returns>The encapsulation of the specified point within the rectangle.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Encapsulate(Rectangle rectangle, Point2 point)
        {
            var left = Math.Min(rectangle.Left, point.X);
            var top = Math.Min(rectangle.Top, point.Y);
            var right = Math.Max(rectangle.Right, point.X);
            var bottom = Math.Max(rectangle.Bottom, point.Y);

            return FromEdges(left, top, right, bottom);
        }

        /// <summary>Returns the intersection between the two specified rectangles, or an empty rectangle if they do not intersect.</summary>
        /// <param name="value1">The first rectangle.</param>
        /// <param name="value2">The second rectangle.</param>
        /// <returns>The intersection or an empty rectangle.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Intersect(Rectangle value1, Rectangle value2)
        {
            var left = Math.Max(value1.Left, value2.Left);
            var top = Math.Max(value1.Top, value2.Top);
            var right = Math.Min(value1.Right, value2.Right);
            var bottom = Math.Min(value1.Bottom, value2.Bottom);

            return right >= left && bottom >= top
                ? FromEdges(left, top, right, bottom)
                : Zero;
        }

        /// <summary>Returns the union of the two specified rectangles.</summary>
        /// <param name="value1">The first rectangle.</param>
        /// <param name="value2">The second rectangle.</param>
        /// <returns>The union.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle Union(Rectangle value1, Rectangle value2)
        {
            var left = Math.Min(value1.Left, value2.Left);
            var top = Math.Min(value1.Top, value2.Top);
            var right = Math.Max(value1.Right, value2.Right);
            var bottom = Math.Max(value1.Bottom, value2.Bottom);

            return FromEdges(left, top, right, bottom);
        }

        /// <summary>Copies the elements of the rectangle to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least four elements. The method copies the rectangle's elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(int[] array)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
            Throw.IfArgumentLessThan(array.Length, 4, nameof(array));
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref array[0]), this);
        }

        /// <summary>Copies the elements of the rectangle to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the rectangle.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the four rectangle elements. In other words, elements <paramref name="index" /> to <paramref name="index" /> + 4 must already exist in <paramref name="array" />.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
        /// -or-
        /// <paramref name="index" /> is greater than or equal to the array length.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(int[] array, int index)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
            Throw.IfArgumentGreaterThanOrEqual((uint)index, (uint)array.Length, nameof(index));
            Throw.IfArgumentLessThan(array.Length - index, 4);
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref array[index]), this);
        }

        /// <summary>Copies the rectangle to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source rectangle is greater than those available in destination span.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<int> destination)
        {
            Throw.IfArgumentLessThan(destination.Length, 4, nameof(destination));
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the rectangle to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source rectangle was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source rectangle.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<int> destination)
        {
            if (destination.Length < 4)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(destination)), this);
            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Rectangle" /> object and their corresponding elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Rectangle other && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another rectangle are equal.</summary>
        /// <param name="other">The other rectangle.</param>
        /// <returns><see langword="true" /> if the two rectangles are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two rectangles are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Width"/> and <see cref="Height"/> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Rectangle other)
            => X.Equals(other.X)
            && Y.Equals(other.Y)
            && Width.Equals(other.Width)
            && Height.Equals(other.Height);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public readonly override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the position and size of the rectangle is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "{" and "}" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public readonly override string ToString() => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the position and size of the rectangle is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "{" and "}" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
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
        /// <remarks>This method returns a string in which each element of the position and size of the rectangle is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "{" and "}" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
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
