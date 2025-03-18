using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cubusky.Numerics
{
    /// <summary>Represents a box with six integer values.</summary>
    public struct Box : IEquatable<Box>, IFormattable
    {
        /// <summary>The X component of the box.</summary>
        public int X;

        /// <summary>The Y component of the box.</summary>
        public int Y;

        /// <summary>The Z component of the box.</summary>
        public int Z;

        private int width;
        private int height;
        private int depth;

        /// <summary>The width of the box.</summary>
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

        /// <summary>The height of the box.</summary>
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

        /// <summary>The depth of the box.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is negative.</exception>
        public int Depth
        {
            readonly get => depth;
            set
            {
                Throw.IfArgumentNegative(value, nameof(Depth));
                depth = value;
            }
        }

        /// <summary>Creates a new <see cref="Box" /> object whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
        /// <param name="width">The value to assign to the <see cref="Width" /> field.</param>
        /// <param name="height">The value to assign to the <see cref="Height" /> field.</param>
        /// <param name="depth">The value to assign to the <see cref="Depth" /> field.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/>, <paramref name="height"/> or <paramref name="depth"/> is negative.</exception>
        public Box(int x, int y, int z, int width, int height, int depth)
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

        /// <summary>Creates a new <see cref="Box"/> object from the specified <see cref="Rectangle"/> object and the specified values.</summary>
        /// <param name="value">The rectangle with four elements.</param>
        /// <param name="z">The additional value to assign to the <see cref="Z"/> field.</param>
        /// <param name="depth">The additional value to assign to the <see cref="Depth"/> field.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depth"/> is negative.</exception>
        public Box(Rectangle value, int z, int depth) : this(value.X, value.Y, z, value.Width, value.Height, depth) { }

        /// <summary>Creates a new <see cref="Box" /> object from the specified <see cref="Point3"/> objects.</summary>
        /// <param name="position">The value to assign to the <see cref="X"/>, <see cref="Y"/> and <see cref="Z"/> fields.</param>
        /// <param name="size">The value to assign to the <see cref="Width"/>, <see cref="Height"/> and <see cref="Depth"/> fields.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="size"/> has an element that is negative.</exception>
        public Box(Point3 position, Point3 size) : this(position.X, position.Y, position.Z, size.X, size.Y, size.Z) { }

        /// <summary>Constructs a box from the given <see cref="ReadOnlySpan{Int32}" />. The span must contain at least 6 elements.</summary>
        /// <param name="values">The span of elements to assign to the box.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the fourth, fifth or the sixth element of the span is negative.</exception>
        public Box(ReadOnlySpan<int> values)
        {
            Throw.IfArgumentLessThan(values.Length, 6, nameof(values));
            Throw.IfArgumentNegative(values[3], "values[3]");
            Throw.IfArgumentNegative(values[4], "values[4]");
            Throw.IfArgumentNegative(values[5], "values[5]");
            this = Unsafe.ReadUnaligned<Box>(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(values)));
        }

        /// <summary>Gets a box whose 6 elements are equal to zero.</summary>
        /// <value>A box whose six elements are equal to zero (that is, it returns the box <c>(0,0,0,0,0,0)</c>.</value>
        public static readonly Box Zero;

        /// <summary>Returns a value that indicates whether each pair of elements in two specified boxes is equal.</summary>
        /// <param name="left">The first box to compare.</param>
        /// <param name="right">The second box to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Box" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Box left, Box right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z)
                && (left.Width == right.Width)
                && (left.Height == right.Height)
                && (left.Depth == right.Depth);
        }

        /// <summary>Returns a value that indicates whether two specified boxes are not equal.</summary>
        /// <param name="left">The first box to compare.</param>
        /// <param name="right">The second box to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Box left, Box right) => !(left == right);

        /// <summary>Defines an implicit conversion of a given <see cref="Box"/> to a <see cref="Bounds3"/>.</summary>
        /// <param name="value">The <see cref="Box"/> to implicitly convert.</param>
        /// <returns>A <see cref="Bounds3"/> instance converted from the <paramref name="value"/> parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Bounds3(Box value) => new Bounds3(value.X, value.Y, value.Z, value.Width, value.Height, value.Depth);

        /// <summary>Defines an implicit conversion of a given <see cref="Bounds3"/> to a <see cref="Box"/>.</summary>
        /// <param name="value">The <see cref="Bounds3"/> to explicitly convert.</param>
        /// <returns>A <see cref="Box"/> instance converted from the <paramref name="value"/> parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Box(Bounds3 value) => new Box((int)value.X, (int)value.Y, (int)value.Z, (int)value.Width, (int)value.Height, (int)value.Depth);

        /// <summary>Creates a new <see cref='Box'/> object from the specified values.</summary>
        /// <param name="left">The x-coordinate of the front-top-left corner of the box.</param>
        /// <param name="top">The y-coordinate of the front-top-left corner of the box.</param>
        /// <param name="front">The z-coordinate of the front-top-left corner of the box.</param>
        /// <param name="right">The x-coordinate of the back-bottom-right corner of the box.</param>
        /// <param name="bottom">The y-coordinate of the back-bottom-right corner of the box.</param>
        /// <param name="back">The z-coordinate of the back-bottom-right corner of the box.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="right"/>, <paramref name="bottom"/> or <paramref name="back"/> is less than <paramref name="left"/>, <paramref name="top"/> or <paramref name="front"/> respectively.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Box FromEdges(int left, int top, int front, int right, int bottom, int back) => new Box(left, top, front, checked(right - left), checked(bottom - top), checked(back - front));

        /// <summary>Creates a new <see cref="Box" /> object from the specified <see cref="Point3"/> objects.</summary>
        /// <param name="frontTopLeft">The coordinates of the front-top-left corner of the box.</param>
        /// <param name="backBottomRight">The coordinates of the back-bottom-right corner of the box.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="backBottomRight"/> is less than <paramref name="frontTopLeft"/> in any dimension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Box FromEdges(Point3 frontTopLeft, Point3 backBottomRight) => FromEdges(frontTopLeft.X, frontTopLeft.Y, frontTopLeft.Z, backBottomRight.X, backBottomRight.Y, backBottomRight.Z);

        /// <summary>Gets the coordinate of the front-top-left corner of the box.</summary>
        public Point3 Position
        {
            readonly get => new Point3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        /// <summary>Gets the size of the box.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> has an element that is negative.</exception>
        public Point3 Size
        {
            readonly get => new Point3(Width, Height, Depth);
            set
            {
                Width = value.X;
                Height = value.Y;
                Depth = value.Z;
            }
        }

        /// <summary>Gets the x-coordinate of the left edge of the box.</summary>
        public int Left
        {
            readonly get => X;
            set => X = value;
        }

        /// <summary>Gets the y-coordinate of the top edge of the box.</summary>
        public int Top
        {
            readonly get => Y;
            set => Y = value;
        }

        /// <summary>Gets the z-coordinate of the front edge of the box.</summary>
        public int Front
        {
            readonly get => Z;
            set => Z = value;
        }

        /// <summary>Gets the x-coordinate of the right edge of the box.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Left"/>.</exception>
        public int Right
        {
            readonly get => checked(X + Width);
            set => Width = checked(value - X);
        }

        /// <summary>Gets the y-coordinate of the bottom edge of the box.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Top"/>.</exception>
        public int Bottom
        {
            readonly get => checked(Y + Height);
            set => Height = checked(value - Y);
        }

        /// <summary>Gets the z-coordinate of the back edge of the box.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than <see cref="Front"/>.</exception>
        public int Back
        {
            readonly get => checked(Z + Depth);
            set => Depth = checked(value - Z);
        }

        /// <summary><see langword="true"/> if the <see cref="Width"/>, <see cref="Height"/> and <see cref="Depth"/> of the box are 0; otherwise, <see langword="false"/>.</summary>
        public readonly bool IsEmpty => Width == 0 && Height == 0 && Depth == 0;

        /// <summary>Creates a new <see cref="Box"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="x">The value to adjust the <see cref="X"/> component by.</param>
        /// <param name="y">The value to adjust the <see cref="Y"/> component by.</param>
        /// <param name="z">The value to adjust the <see cref="Z"/> component by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Box Offset(int x, int y, int z) => new Box(checked(X + x), checked(Y + y), checked(Z + z), Width, Height, Depth);

        /// <summary>Creates a new <see cref="Box"/> object adjusting the <see cref="Position"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Position"/> by.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Box Offset(Point3 value) => Offset(value.X, value.Y, value.Z);

        /// <summary>Creates a new <see cref="Box"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="width">The value to adjust the <see cref="Width"/> component by.</param>
        /// <param name="height">The value to adjust the <see cref="Height"/> component by.</param>
        /// <param name="depth">The value to adjust the <see cref="Depth"/> component by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="width"/>, <paramref name="height"/> or <paramref name="depth"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Box Expand(int width, int height, int depth) => new Box(X, Y, Z, checked(Width + width), checked(Height + height), checked(Depth + depth));

        /// <summary>Creates a new <see cref="Box"/> object adjusting the <see cref="Size"/> by the specified value.</summary>
        /// <param name="value">The value to adjust the <see cref="Size"/> by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> decreases the <see cref="Size"/> in the negative.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Box Expand(Point3 value) => Expand(value.X, value.Y, value.Z);

        /// <summary>Determines whether the specified point is contained within the box.</summary>
        /// <param name="point">The point to locate within the box.</param>
        /// <returns><see langword="true"/> if the point is contained within the box; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Point3 point)
            => point.X >= Left
            && point.X <= Right
            && point.Y >= Top
            && point.Y <= Bottom
            && point.Z >= Front
            && point.Z <= Back;

        /// <summary>Determines whether the specified box is entirely contained within the box.</summary>
        /// <param name="other">The box to entirely locate within the box.</param>
        /// <returns><see langword="true"/> if the box is entirely contained within the box; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(Box other)
            => other.Left >= Left
            && other.Right <= Right
            && other.Top >= Top
            && other.Bottom <= Bottom
            && other.Front >= Front
            && other.Back <= Back;

        /// <summary>Determines whether the specified box intersects with the box.</summary>
        /// <param name="other">The box to locate intersecting with the box.</param>
        /// <returns><see langword="true"/> if the box intersects with the box; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Intersects(Box other)
            => other.Left < Right
            && other.Right > Left
            && other.Top < Bottom
            && other.Bottom > Top
            && other.Front < Back
            && other.Back > Front;

        /// <summary>Returns the closest point between a specified box and point.</summary>
        /// <param name="box">The box.</param>
        /// <param name="point">The point.</param>
        /// <returns>The closest point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ClosestPoint(Box box, Vector3 point)
        {
            var x = Math.Clamp(point.X, box.Left, box.Right);
            var y = Math.Clamp(point.Y, box.Top, box.Bottom);
            var z = Math.Clamp(point.Z, box.Front, box.Back);
            return new Vector3(x, y, z);
        }

        /// <summary>Returns the Euclidean distance squared between a specified box and point.</summary>
        /// <param name="box">The box.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSquared(Box box, Vector3 point) => Vector3.DistanceSquared(point, ClosestPoint(box, point));

        /// <summary>Computes the Euclidean distance squared between a specified box and point.</summary>
        /// <param name="box">The box.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Box box, Vector3 point) => Vector3.Distance(point, ClosestPoint(box, point));

        /// <summary>Returns the encapsulation of the specified point within the box.</summary>
        /// <param name="box">The box to encapsulate the point within.</param>
        /// <param name="point">The point to encapsulate within the box.</param>
        /// <returns>The encapsulation of the specified point within the box.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Box Encapsulate(Box box, Point3 point)
        {
            var left = Math.Min(box.Left, point.X);
            var top = Math.Min(box.Top, point.Y);
            var front = Math.Min(box.Front, point.Z);
            var right = Math.Max(box.Right, point.X);
            var bottom = Math.Max(box.Bottom, point.Y);
            var back = Math.Max(box.Back, point.Z);

            return FromEdges(left, top, front, right, bottom, back);
        }

        /// <summary>Returns the intersection between the two specified boxes, or an empty box if they do not intersect.</summary>
        /// <param name="value1">The first box.</param>
        /// <param name="value2">The second box.</param>
        /// <returns>The intersection or an empty box.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Box Intersect(Box value1, Box value2)
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

        /// <summary>Returns the union of the two specified boxes.</summary>
        /// <param name="value1">The first box.</param>
        /// <param name="value2">The second box.</param>
        /// <returns>The union.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Box Union(Box value1, Box value2)
        {
            var left = Math.Min(value1.Left, value2.Left);
            var top = Math.Min(value1.Top, value2.Top);
            var front = Math.Min(value1.Front, value2.Front);
            var right = Math.Max(value1.Right, value2.Right);
            var bottom = Math.Max(value1.Bottom, value2.Bottom);
            var back = Math.Max(value1.Back, value2.Back);

            return FromEdges(left, top, front, right, bottom, back);
        }

        /// <summary>Copies the elements of the box to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least six elements. The method copies the boxes' elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(int[] array)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
            Throw.IfArgumentLessThan(array.Length, 6, nameof(array));
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref array[0]), this);
        }

        /// <summary>Copies the elements of the box to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the box.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the six box elements. In other words, elements <paramref name="index" /> to <paramref name="index" /> + 6 must already exist in <paramref name="array" />.</remarks>
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
            Throw.IfArgumentLessThan(array.Length - index, 6);
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref array[index]), this);
        }

        /// <summary>Copies the box to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 6.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source box is greater than those available in destination span.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<int> destination)
        {
            Throw.IfArgumentLessThan(destination.Length, 6, nameof(destination));
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the box to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 6.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source box was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source box.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<int> destination)
        {
            if (destination.Length < 6)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(destination)), this);
            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Box" /> object and their corresponding elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals([NotNullWhen(true)] object? obj) => obj is Box other && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another box are equal.</summary>
        /// <param name="other">The other box.</param>
        /// <returns><see langword="true" /> if the two boxes are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two boxes are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Z"/>, <see cref="Width"/>, <see cref="Height"/> and <see cref="Depth"/> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Box other)
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
        /// <remarks>This method returns a string in which each element of the position and size of the box is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "{" and "}" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public readonly override string ToString() => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the position and size of the box is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "{" and "}" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
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
        /// <remarks>This method returns a string in which each element of the position and size of the box is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "{" and "}" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
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
