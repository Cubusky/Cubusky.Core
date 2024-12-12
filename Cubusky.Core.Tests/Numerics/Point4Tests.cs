using System;
using System.Numerics;
using Xunit;

namespace Cubusky.Numerics.Tests
{
    public class Point4Tests
    {
        [Fact]
        public void Point4_Constructor_SetsCorrectValues()
        {
            // Arrange
            int x = 1;
            int y = 2;
            int z = 3;
            int w = 4;

            // Act
            var point = new Point4(x, y, z, w);

            // Assert
            Assert.Equal(x, point.X);
            Assert.Equal(y, point.Y);
            Assert.Equal(z, point.Z);
            Assert.Equal(w, point.W);
        }

        [Fact]
        public void Point4_Zero_ReturnsPointWithZeroValues()
        {
            // Act
            var point = Point4.Zero;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(0, point.Y);
            Assert.Equal(0, point.Z);
            Assert.Equal(0, point.W);
        }

        [Fact]
        public void Point4_One_ReturnsPointWithOneValues()
        {
            // Act
            var point = Point4.One;

            // Assert
            Assert.Equal(1, point.X);
            Assert.Equal(1, point.Y);
            Assert.Equal(1, point.Z);
            Assert.Equal(1, point.W);
        }

        [Fact]
        public void Point4_UnitX_ReturnsPointWithXValueOne()
        {
            // Act
            var point = Point4.UnitX;

            // Assert
            Assert.Equal(1, point.X);
            Assert.Equal(0, point.Y);
            Assert.Equal(0, point.Z);
            Assert.Equal(0, point.W);
        }

        [Fact]
        public void Point4_UnitY_ReturnsPointWithYValueOne()
        {
            // Act
            var point = Point4.UnitY;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(1, point.Y);
            Assert.Equal(0, point.Z);
            Assert.Equal(0, point.W);
        }

        [Fact]
        public void Point4_UnitZ_ReturnsPointWithZValueOne()
        {
            // Act
            var point = Point4.UnitZ;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(0, point.Y);
            Assert.Equal(1, point.Z);
            Assert.Equal(0, point.W);
        }

        [Fact]
        public void Point4_UnitW_ReturnsPointWithWValueOne()
        {
            // Act
            var point = Point4.UnitW;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(0, point.Y);
            Assert.Equal(0, point.Z);
            Assert.Equal(1, point.W);
        }

        [Fact]
        public void Point4_Addition_ReturnsSummedPoint()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(5, 6, 7, 8);

            // Act
            var result = point1 + point2;

            // Assert
            Assert.Equal(6, result.X);
            Assert.Equal(8, result.Y);
            Assert.Equal(10, result.Z);
            Assert.Equal(12, result.W);
        }

        [Fact]
        public void Point4_Subtraction_ReturnsSubtractedPoint()
        {
            // Arrange
            var point1 = new Point4(5, 6, 7, 8);
            var point2 = new Point4(1, 2, 3, 4);

            // Act
            var result = point1 - point2;

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(4, result.Y);
            Assert.Equal(4, result.Z);
            Assert.Equal(4, result.W);
        }

        [Fact]
        public void Point4_Multiplication_ReturnsMultipliedPoint()
        {
            // Arrange
            var point = new Point4(2, 3, 4, 5);
            int scalar = 2;

            // Act
            var result = point * scalar;

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(6, result.Y);
            Assert.Equal(8, result.Z);
            Assert.Equal(10, result.W);
        }

        [Fact]
        public void Point4_Division_ReturnsDividedPoint()
        {
            // Arrange
            var point = new Point4(4, 6, 8, 10);
            int scalar = 2;

            // Act
            var result = point / scalar;

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(3, result.Y);
            Assert.Equal(4, result.Z);
            Assert.Equal(5, result.W);
        }

        [Fact]
        public void Point4_Modulus_ReturnsModulusPoint()
        {
            // Arrange
            var point = new Point4(5, 7, 9, 11);
            int scalar = 3;

            // Act
            var result = point % scalar;

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(1, result.Y);
            Assert.Equal(0, result.Z);
            Assert.Equal(2, result.W);
        }

        [Fact]
        public void Point4_Equality_ReturnsTrueForEqualPoints()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(1, 2, 3, 4);

            // Act
            var result = point1 == point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Point4_Equality_ReturnsFalseForDifferentPoints()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(5, 6, 7, 8);

            // Act
            var result = point1 == point2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Point4_Inequality_ReturnsTrueForDifferentPoints()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(5, 6, 7, 8);

            // Act
            var result = point1 != point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Point4_Inequality_ReturnsFalseForEqualPoints()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(1, 2, 3, 4);

            // Act
            var result = point1 != point2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Point4_GetHashCode_ReturnsDifferentHashCodesForDifferentPoints()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(5, 6, 7, 8);

            // Act
            var hashCode1 = point1.GetHashCode();
            var hashCode2 = point2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void Point4_GetHashCode_ReturnsSameHashCodeForEqualPoints()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(1, 2, 3, 4);

            // Act
            var hashCode1 = point1.GetHashCode();
            var hashCode2 = point2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void Point4_ToString()
        {
            // Arrange
            var point = new Point4(1, 2, 3, 4);

            // Act
            var result = point.ToString();

            var v = new Vector4(1, 2, 3, 4);
            var s = v.ToString();

            // Assert
            Assert.Equal("<1. 2. 3. 4>", result);
        }

        [Fact]
        public void Point4_Abs_ReturnsPointWithAbsoluteValues()
        {
            // Arrange
            var point = new Point4(-1, -2, -3, -4);

            // Act
            var result = Point4.Abs(point);

            // Assert
            Assert.Equal(1, result.X);
            Assert.Equal(2, result.Y);
            Assert.Equal(3, result.Z);
            Assert.Equal(4, result.W);
        }

        [Fact]
        public void Point4_Clamp_ReturnsRestrictedPoint()
        {
            // Arrange
            var value1 = new Point4(1, 4, 7, 10);
            var min = new Point4(2, 3, 4, 5);
            var max = new Point4(6, 7, 8, 9);

            // Act
            var result = Point4.Clamp(value1, min, max);

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(4, result.Y);
            Assert.Equal(7, result.Z);
            Assert.Equal(9, result.W);
        }

        [Fact]
        public void Point4_Distance_ReturnsEuclideanDistance()
        {
            // Arrange
            var value1 = new Point4(1, 2, 3, 4);
            var value2 = new Point4(5, 6, 7, 8);

            // Act
            var result = Point4.Distance(value1, value2);

            // Assert
            Assert.Equal(8f, result, 1E-5f);
        }

        [Fact]
        public void Point4_DistanceSquared_ReturnsEuclideanDistanceSquared()
        {
            // Arrange
            var value1 = new Point4(1, 2, 3, 4);
            var value2 = new Point4(5, 6, 7, 8);

            // Act
            var result = Point4.DistanceSquared(value1, value2);

            // Assert
            Assert.Equal(64, result);
        }

        [Fact]
        public void Point4_Dot_ReturnsDotProduct()
        {
            // Arrange
            var point1 = new Point4(1, 2, 3, 4);
            var point2 = new Point4(5, 6, 7, 8);

            // Act
            var result = Point4.Dot(point1, point2);

            // Assert
            Assert.Equal(70, result);
        }

        [Fact]
        public void Point4_Max_ReturnsMaximizedPoint()
        {
            // Arrange
            var value1 = new Point4(1, 5, 3, 7);
            var value2 = new Point4(4, 2, 8, 6);

            // Act
            var result = Point4.Max(value1, value2);

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(5, result.Y);
            Assert.Equal(8, result.Z);
            Assert.Equal(7, result.W);
        }

        [Fact]
        public void Point4_Min_ReturnsMinimizedPoint()
        {
            // Arrange
            var value1 = new Point4(1, 5, 3, 7);
            var value2 = new Point4(4, 2, 8, 6);

            // Act
            var result = Point4.Min(value1, value2);

            // Assert
            Assert.Equal(1, result.X);
            Assert.Equal(2, result.Y);
            Assert.Equal(3, result.Z);
            Assert.Equal(6, result.W);
        }

        [Fact]
        public void Point4_Length_ReturnsCorrectLength()
        {
            // Arrange
            var point = new Point4(2, 3, 4, 5);
            float expectedLength = 7.348469f;

            // Act
            var result = point.Length();

            // Assert
            Assert.Equal(expectedLength, result, 1E-5f);
        }

        [Fact]
        public void Point4_LengthSquared_ReturnsCorrectLengthSquared()
        {
            // Arrange
            var point = new Point4(2, 3, 4, 5);
            int expectedLengthSquared = 54;

            // Act
            var result = point.LengthSquared();

            // Assert
            Assert.Equal(expectedLengthSquared, result);
        }

        [Fact]
        public void Point4_CopyTo_Array_CopiesElementsToSpecifiedArray()
        {
            // Arrange
            var point = new Point4(1, 2, 3, 4);
            int[] array = new int[4];

            // Act
            point.CopyTo(array);

            // Assert
            Assert.Equal(1, array[0]);
            Assert.Equal(2, array[1]);
            Assert.Equal(3, array[2]);
            Assert.Equal(4, array[3]);
        }

        [Fact]
        public void Point4_CopyTo_Array_Index_CopiesElementsToSpecifiedArrayStartingAtIndex()
        {
            // Arrange
            var point = new Point4(1, 2, 3, 4);
            int[] array = new int[6];

            // Act
            point.CopyTo(array, 2);

            // Assert
            Assert.Equal(0, array[0]);
            Assert.Equal(0, array[1]);
            Assert.Equal(1, array[2]);
            Assert.Equal(2, array[3]);
            Assert.Equal(3, array[4]);
            Assert.Equal(4, array[5]);
        }

        [Fact]
        public void Point4_CopyTo_Span_CopiesElementsToSpecifiedSpan()
        {
            // Arrange
            var point = new Point4(1, 2, 3, 4);
            Span<int> destination = new int[4];

            // Act
            point.CopyTo(destination);

            // Assert
            Assert.Equal(1, destination[0]);
            Assert.Equal(2, destination[1]);
            Assert.Equal(3, destination[2]);
            Assert.Equal(4, destination[3]);
        }

        [Fact]
        public void Point4_TryCopyTo_Span_ReturnsTrueAndCopiesElementsToSpecifiedSpan()
        {
            // Arrange
            var point = new Point4(1, 2, 3, 4);
            Span<int> destination = new int[4];

            // Act
            var result = point.TryCopyTo(destination);

            // Assert
            Assert.True(result);
            Assert.Equal(1, destination[0]);
            Assert.Equal(2, destination[1]);
            Assert.Equal(3, destination[2]);
            Assert.Equal(4, destination[3]);
        }

        [Fact]
        public void Point4_TryCopyTo_Span_ReturnsFalseWhenDestinationSpanIsTooSmall()
        {
            // Arrange
            var point = new Point4(1, 2, 3, 4);
            Span<int> destination = new int[3];

            // Act
            var result = point.TryCopyTo(destination);

            // Assert
            Assert.False(result);
        }
    }
}
