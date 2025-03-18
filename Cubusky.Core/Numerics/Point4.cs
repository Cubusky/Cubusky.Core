using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cubusky.Numerics
{
    /// <summary>Represents a point with four integer values.</summary>
    public struct Point4 : IEquatable<Point4>, IFormattable
#if NET8_0_OR_GREATER
        , IPoint<Point4>
        , IDivisionOperators<Point4, int, Point4>
        , IModulusOperators<Point4, int, Point4>
        , IMultiplyOperators<Point4, int, Point4>
        , IMinMaxValue<Point4>
#endif
    {
        /// <summary>The X component of the point.</summary>
        public int X;

        /// <summary>The Y component of the point.</summary>
        public int Y;

        /// <summary>The Z component of the point.</summary>
        public int Z;

        /// <summary>The W component of the point.</summary>
        public int W;

        /// <summary>The count of components of the point</summary>
        public static int Count => 4;

        /// <summary>Creates a new <see cref="Point4" /> object whose four elements have the same value.</summary>
        /// <param name="value">The value to assign to all four elements.</param>
        public Point4(int value) : this(value, value, value, value)
        {
        }

        /// <summary>Creates a new <see cref="Point4" /> object from the specified <see cref="Point2" /> object and the specified value.</summary>
        /// <param name="value">The point with two elements.</param>
        /// <param name="z">The additional value to assign to the <see cref="Z" /> field.</param>
        /// <param name="w">The additional value to assign to the <see cref="W" /> field.</param>
        public Point4(Point2 value, int z, int w) : this(value.X, value.Y, z, w)
        {
        }

        /// <summary>Creates a new <see cref="Point4" /> object from the specified <see cref="Point3" /> object and the specified value.</summary>
        /// <param name="value">The point with two elements.</param>
        /// <param name="w">The additional value to assign to the <see cref="W" /> field.</param>
        public Point4(Point3 value, int w) : this(value.X, value.Y, value.Z, w)
        {
        }

        /// <summary>Creates a point whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
        /// <param name="w">The value to assign to the <see cref="W" /> field.</param>
        public Point4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>Constructs a point from the given <see cref="ReadOnlySpan{Int32}" />. The span must contain at least 4 elements.</summary>
        /// <param name="values">The span of elements to assign to the point.</param>
        public Point4(ReadOnlySpan<int> values)
        {
            Throw.IfArgumentLessThan(values.Length, Count, nameof(values));
            this = Unsafe.ReadUnaligned<Point4>(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(values)));
        }

        /// <summary>Gets a point whose 4 elements are equal to zero.</summary>
        /// <value>A point whose four elements are equal to zero (that is, it returns the point <c>(0,0,0,0)</c>.</value>
        public static Point4 Zero => default;

        /// <summary>Gets a point whose 4 elements are equal to one.</summary>
        /// <value>A point whose four elements are equal to one (that is, it returns the point <c>(1,1,1,1)</c>.</value>
        public static Point4 One => new Point4(1);

        /// <summary>Gets the point (1,0,0,0).</summary>
        /// <value>The point <c>(1,0,0,0)</c>.</value>
        public static Point4 UnitX => new Point4(1, 0, 0, 0);

        /// <summary>Gets the point (0,1,0,0).</summary>
        /// <value>The point <c>(0,1,0,0)</c>.</value>
        public static Point4 UnitY => new Point4(0, 1, 0, 0);

        /// <summary>Gets the point (0,0,1,0).</summary>
        /// <value>The point <c>(0,0,1,0)</c>.</value>
        public static Point4 UnitZ => new Point4(0, 0, 1, 0);

        /// <summary>Gets the point (0,0,0,1).</summary>
        /// <value>The point <c>(0,0,0,1)</c>.</value>
        public static Point4 UnitW => new Point4(0, 0, 0, 1);

#if NET8_0_OR_GREATER
        static Point4 IAdditiveIdentity<Point4, Point4>.AdditiveIdentity => Zero;
        static Point4 IMultiplicativeIdentity<Point4, Point4>.MultiplicativeIdentity => One;
#endif

        /// <summary>Represents the smallest possible value of a <see cref="Point4"/></summary>
        public static Point4 MinValue => new Point4(int.MinValue);

        /// <summary>Represents the largest possible value of a <see cref="Point4"/></summary>
        public static Point4 MaxValue => new Point4(int.MaxValue);

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <returns>The the element at <paramref name="index" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
        public int this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => this.GetElement(index);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this = this.WithElement(index, value);
        }


        /// <summary>Adds two points together.</summary>
        /// <param name="left">The first point to add.</param>
        /// <param name="right">The second point to add.</param>
        /// <returns>The summed point.</returns>
        /// <remarks>The <see cref="op_Addition" /> method defines the addition operation for <see cref="Point4" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator +(Point4 left, Point4 right)
        {
            return new Point4(
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z,
                left.W + right.W
            );
        }

        /// <summary>Divides the first point by the second.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The point that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator /(Point4 left, Point4 right)
        {
            return new Point4(
                left.X / right.X,
                left.Y / right.Y,
                left.Z / right.Z,
                left.W / right.W
            );
        }

        /// <summary>Divides the specified point by a specified scalar value.</summary>
        /// <param name="value1">The point.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator /(Point4 value1, int value2) => value1 / new Point4(value2);

        /// <summary>Decrements the point.</summary>
        /// <param name="value">The point to decrement.</param>
        /// <returns>The result of decrementing <paramref name="value" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator --(Point4 value)
        {
            value.X--;
            value.Y--;
            value.Z--;
            value.W--;
            return value;
        }

        /// <summary>Returns a value that indicates whether each pair of elements in two specified points is equal.</summary>
        /// <param name="left">The first point to compare.</param>
        /// <param name="right">The second point to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Point4" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point4 left, Point4 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z)
                && (left.W == right.W);
        }

        /// <summary>Returns a value that indicates whether two specified points are not equal.</summary>
        /// <param name="left">The first point to compare.</param>
        /// <param name="right">The second point to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point4 left, Point4 right) => !(left == right);

        /// <summary>Increments the point.</summary>
        /// <param name="value">The point to increment.</param>
        /// <returns>The result of incrementing <paramref name="value" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator ++(Point4 value)
        {
            value.X++;
            value.Y++;
            value.Z++;
            value.W++;
            return value;
        }

        /// <summary>Divides two values together to compute their modulus or remainder.</summary>
        /// <param name="left">The point which right divides.</param>
        /// <param name="right">The point which divides left.</param>
        /// <returns>The modulus or remainder of left divided by right.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator %(Point4 left, Point4 right)
        {
            return new Point4(
                left.X % right.X,
                left.Y % right.Y,
                left.Z % right.Z,
                left.W % right.W
            );
        }

        /// <summary>Divides two values together to compute their modulus or remainder.</summary>
        /// <param name="left">The point which right divides.</param>
        /// <param name="right">The point which divides left.</param>
        /// <returns>The modulus or remainder of left divided by right.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator %(Point4 left, int right) => left % new Point4(right);

        /// <summary>Returns a new point whose values are the product of each pair of elements in two specified points.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The element-wise product point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator *(Point4 left, Point4 right)
        {
            return new Point4(
                left.X * right.X,
                left.Y * right.Y,
                left.Z * right.Z,
                left.W * right.W
            );
        }

        /// <summary>Multiplies the specified point by the specified scalar value.</summary>
        /// <param name="left">The point.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator *(Point4 left, int right) => left * new Point4(right);

        /// <summary>Multiplies the scalar value by the specified point.</summary>
        /// <param name="left">The point.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator *(int left, Point4 right) => right * left;

        /// <summary>Subtracts the second point from the first.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The point that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <remarks>The <see cref="op_Subtraction" /> method defines the subtraction operation for <see cref="Point4" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator -(Point4 left, Point4 right)
        {
            return new Point4(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W
            );
        }

        /// <summary>Negates the specified point.</summary>
        /// <param name="value">The point to negate.</param>
        /// <returns>The negated point.</returns>
        /// <remarks>The <see cref="op_UnaryNegation" /> method defines the unary negation operation for <see cref="Point4" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 operator -(Point4 value) => new Point4(-value.X, -value.Y, -value.Z, -value.W);

        /// <summary>Plus the specified point.</summary>
        /// <param name="value">The point to plus.</param>
        /// <returns>The plused point.</returns>
        /// <remarks>The <see cref="op_UnaryPlus" /> method defines the unary plus operation for <see cref="Point4" /> objects.</remarks>
        public static Point4 operator +(Point4 value) => new Point4(+value.X, +value.Y, +value.Z, +value.W);

        /// <summary>Defines an implicit conversion of a given <see cref="Point4"/> to a <see cref="Vector4"/>.</summary>
        /// <param name="value">The <see cref="Point4"/> to implicitly convert.</param>
        /// <returns>A <see cref="Vector4"/> instance converted from the <paramref name="value"/> parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4(Point4 value) => new Vector4(value.X, value.Y, value.Z, value.W);

        /// <summary>Defines an explicit conversion of a given <see cref="Vector4"/> to a <see cref="Point4"/>.</summary>
        /// <param name="value">The <see cref="Vector4"/> to explicitly convert.</param>
        /// <returns>A <see cref="Point4"/> instance converted from the <paramref name="value"/> parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point4(Vector4 value) => new Point4((int)value.X, (int)value.Y, (int)value.Z, (int)value.W);

        /// <summary>Returns a point whose elements are the absolute values of each of the specified point's elements.</summary>
        /// <param name="value">A point.</param>
        /// <returns>The absolute value point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Abs(Point4 value)
        {
            return new Point4(
                Math.Abs(value.X),
                Math.Abs(value.Y),
                Math.Abs(value.Z),
                Math.Abs(value.W)
            );
        }

        /// <summary>Adds two points together.</summary>
        /// <param name="left">The first point to add.</param>
        /// <param name="right">The second point to add.</param>
        /// <returns>The summed point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Add(Point4 left, Point4 right) => left + right;

        /// <summary>Restricts a point between a minimum and a maximum value.</summary>
        /// <param name="value1">The point to restrict.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The restricted point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Clamp(Point4 value1, Point4 min, Point4 max)
        {
            // We must follow HLSL behavior in the case user specified min value is bigger than max value.
            return Min(Max(value1, min), max);
        }

        /// <summary>Computes the Euclidean distance between the two given points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Point4 value1, Point4 value2)
        {
            float distanceSquared = DistanceSquared(value1, value2);
            return MathF.Sqrt(distanceSquared);
        }

        /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DistanceSquared(Point4 value1, Point4 value2)
        {
            Point4 difference = value1 - value2;
            return Dot(difference, difference);
        }

        /// <summary>Divides the first point by the second.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The point resulting from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Divide(Point4 left, Point4 right) => left / right;

        /// <summary>Divides the specified point by a specified scalar value.</summary>
        /// <param name="left">The point.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The point that results from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Divide(Point4 left, int divisor) => left / divisor;

        /// <summary>Returns the dot product of two points.</summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Dot(Point4 point1, Point4 point2)
        {
            return (point1.X * point2.X)
                 + (point1.Y * point2.Y)
                 + (point1.Z * point2.Z)
                 + (point1.W * point2.W);
        }

        /// <summary>Returns a point whose elements are the maximum of each of the pairs of elements in two specified points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The maximized point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Max(Point4 value1, Point4 value2)
        {
            return new Point4(
                (value1.X > value2.X) ? value1.X : value2.X,
                (value1.Y > value2.Y) ? value1.Y : value2.Y,
                (value1.Z > value2.Z) ? value1.Z : value2.Z,
                (value1.W > value2.W) ? value1.W : value2.W
            );
        }

        /// <summary>Returns a point whose elements are the minimum of each of the pairs of elements in two specified points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The minimized point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Min(Point4 value1, Point4 value2)
        {
            return new Point4(
                (value1.X < value2.X) ? value1.X : value2.X,
                (value1.Y < value2.Y) ? value1.Y : value2.Y,
                (value1.Z < value2.Z) ? value1.Z : value2.Z,
                (value1.W < value2.W) ? value1.W : value2.W
            );
        }

        /// <summary>Returns a new point whose values are the product of each pair of elements in two specified points.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The element-wise product point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Multiply(Point4 left, Point4 right) => left * right;

        /// <summary>Multiplies a point by a specified scalar.</summary>
        /// <param name="left">The point to multiply.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Multiply(Point4 left, int right) => left * right;

        /// <summary>Multiplies a scalar value by a specified point.</summary>
        /// <param name="left">The scaled value.</param>
        /// <param name="right">The point.</param>
        /// <returns>The scaled point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Multiply(int left, Point4 right) => left * right;

        /// <summary>Negates a specified point.</summary>
        /// <param name="value">The point to negate.</param>
        /// <returns>The negated point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Negate(Point4 value) => -value;

        /// <summary>Subtracts the second point from the first.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The difference point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4 Subtract(Point4 left, Point4 right) => left - right;

        /// <summary>Copies the elements of the point to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least four elements. The method copies the point's elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(int[] array)
        {
            // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
            Throw.IfArgumentLessThan(array.Length, Count, nameof(array));
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref array[0]), this);
        }

        /// <summary>Copies the elements of the point to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the point.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the four point elements. In other words, elements <paramref name="index" />, <paramref name="index" /> + 1, and <paramref name="index" /> + 2 must already exist in <paramref name="array" />.</remarks>
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
            Throw.IfArgumentLessThan(array.Length - index, Count);

            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref array[index]), this);
        }

        /// <summary>Copies the point to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source point is greater than those available in destination span.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<int> destination)
        {
            Throw.IfArgumentLessThan(destination.Length, Count, nameof(destination));
            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the point to the given <see cref="Span{Int32}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source point was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<int> destination)
        {
            if (destination.Length < Count)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<int, byte>(ref MemoryMarshal.GetReference(destination)), this);
            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Point4" /> object and their corresponding elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is Point4 other) && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another point are equal.</summary>
        /// <param name="other">The other point.</param>
        /// <returns><see langword="true" /> if the two points are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two points are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Z" /> and <see cref="W" /> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Point4 other) => X.Equals(other.X)
            && Y.Equals(other.Y)
            && Z.Equals(other.Z)
            && W.Equals(other.W);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);

        /// <summary>Returns the length of the point.</summary>
        /// <returns>The point's length.</returns>
        /// <altmember cref="LengthSquared"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Length()
        {
            float lengthSquared = LengthSquared();
            return MathF.Sqrt(lengthSquared);
        }

        /// <summary>Returns the length of the point squared.</summary>
        /// <returns>The point's length squared.</returns>
        /// <remarks>This operation offers better performance than a call to the <see cref="Length" /> method.</remarks>
        /// <altmember cref="Length"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int LengthSquared() => Dot(this, this);

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the point is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the point is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
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
        /// <remarks>This method returns a string in which each element of the point is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
#if NET7_0_OR_GREATER
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
#else
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
#endif
        {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            return $"<{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}{separator} {Z.ToString(format, formatProvider)}{separator} {W.ToString(format, formatProvider)}>";
        }
    }
}
