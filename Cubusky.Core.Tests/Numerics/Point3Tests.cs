using Cubusky.Numerics;
using System;
using System.Globalization;
using Xunit;

namespace Cubusky.Tests.Numerics
{
    public class Point3Tests
    {
        [Fact]
        public void Point3_Constructor_SetsCorrectValues()
        {
            // Arrange
            int x = 1;
            int y = 2;
            int z = 3;

            // Act
            var point = new Point3(x, y, z);

            // Assert
            Assert.Equal(x, point.X);
            Assert.Equal(y, point.Y);
            Assert.Equal(z, point.Z);
        }

        [Fact]
        public void Point3_Zero_ReturnsPointWithZeroValues()
        {
            // Act
            var point = Point3.Zero;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(0, point.Y);
            Assert.Equal(0, point.Z);
        }

        [Fact]
        public void Point3_One_ReturnsPointWithOneValues()
        {
            // Act
            var point = Point3.One;

            // Assert
            Assert.Equal(1, point.X);
            Assert.Equal(1, point.Y);
            Assert.Equal(1, point.Z);
        }

        [Fact]
        public void Point3_UnitX_ReturnsPointWithXValueOne()
        {
            // Act
            var point = Point3.UnitX;

            // Assert
            Assert.Equal(1, point.X);
            Assert.Equal(0, point.Y);
            Assert.Equal(0, point.Z);
        }

        [Fact]
        public void Point3_UnitY_ReturnsPointWithYValueOne()
        {
            // Act
            var point = Point3.UnitY;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(1, point.Y);
            Assert.Equal(0, point.Z);
        }

        [Fact]
        public void Point3_UnitZ_ReturnsPointWithZValueOne()
        {
            // Act
            var point = Point3.UnitZ;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(0, point.Y);
            Assert.Equal(1, point.Z);
        }

        [Fact]
        public void Point3_Addition_ReturnsSummedPoint()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(4, 5, 6);

            // Act
            var result = point1 + point2;

            // Assert
            Assert.Equal(5, result.X);
            Assert.Equal(7, result.Y);
            Assert.Equal(9, result.Z);
        }

        [Fact]
        public void Point3_Subtraction_ReturnsSubtractedPoint()
        {
            // Arrange
            var point1 = new Point3(4, 5, 6);
            var point2 = new Point3(1, 2, 3);

            // Act
            var result = point1 - point2;

            // Assert
            Assert.Equal(3, result.X);
            Assert.Equal(3, result.Y);
            Assert.Equal(3, result.Z);
        }

        [Fact]
        public void Point3_Multiplication_ReturnsMultipliedPoint()
        {
            // Arrange
            var point = new Point3(2, 3, 4);
            int scalar = 2;

            // Act
            var result = point * scalar;

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(6, result.Y);
            Assert.Equal(8, result.Z);
        }

        [Fact]
        public void Point3_Division_ReturnsDividedPoint()
        {
            // Arrange
            var point = new Point3(4, 6, 8);
            int scalar = 2;

            // Act
            var result = point / scalar;

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(3, result.Y);
            Assert.Equal(4, result.Z);
        }

        [Fact]
        public void Point3_Modulus_ReturnsModulusPoint()
        {
            // Arrange
            var point = new Point3(5, 7, 9);
            int scalar = 3;

            // Act
            var result = point % scalar;

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(1, result.Y);
            Assert.Equal(0, result.Z);
        }

        [Fact]
        public void Point3_Equality_ReturnsTrueForEqualPoints()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(1, 2, 3);

            // Act
            var result = point1 == point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Point3_Equality_ReturnsFalseForDifferentPoints()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(4, 5, 6);

            // Act
            var result = point1 == point2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Point3_Inequality_ReturnsTrueForDifferentPoints()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(4, 5, 6);

            // Act
            var result = point1 != point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Point3_Inequality_ReturnsFalseForEqualPoints()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(1, 2, 3);

            // Act
            var result = point1 != point2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Point3_GetHashCode_ReturnsDifferentHashCodesForDifferentPoints()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(4, 5, 6);

            // Act
            var hashCode1 = point1.GetHashCode();
            var hashCode2 = point2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void Point3_GetHashCode_ReturnsSameHashCodeForEqualPoints()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(1, 2, 3);

            // Act
            var hashCode1 = point1.GetHashCode();
            var hashCode2 = point2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void Point3_ToString()
        {
            // Arrange
            var point = new Point3(1, 2, 3);
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Act
            var result = point.ToString();

            // Assert
            Assert.Equal("<1, 2, 3>", result);
        }

        [Fact]
        public void Point3_Abs_ReturnsPointWithAbsoluteValues()
        {
            // Arrange
            var point = new Point3(-1, -2, -3);

            // Act
            var result = Point3.Abs(point);

            // Assert
            Assert.Equal(1, result.X);
            Assert.Equal(2, result.Y);
            Assert.Equal(3, result.Z);
        }

        [Fact]
        public void Point3_Clamp_ReturnsRestrictedPoint()
        {
            // Arrange
            var value1 = new Point3(1, 4, 7);
            var min = new Point3(2, 3, 4);
            var max = new Point3(4, 5, 6);

            // Act
            var result = Point3.Clamp(value1, min, max);

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(4, result.Y);
            Assert.Equal(6, result.Z);
        }

        [Fact]
        public void Point3_Cross_ReturnsCrossProduct()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(4, 5, 6);

            // Act
            var result = Point3.Cross(point1, point2);

            // Assert
            Assert.Equal(-3, result.X);
            Assert.Equal(6, result.Y);
            Assert.Equal(-3, result.Z);
        }

        [Fact]
        public void Point3_Distance_ReturnsEuclideanDistance()
        {
            // Arrange
            var value1 = new Point3(1, 2, 3);
            var value2 = new Point3(4, 5, 6);

            // Act
            var result = Point3.Distance(value1, value2);

            // Assert
            Assert.Equal(5.196152f, result, 1E-5f);
        }

        [Fact]
        public void Point3_DistanceSquared_ReturnsEuclideanDistanceSquared()
        {
            // Arrange
            var value1 = new Point3(1, 2, 3);
            var value2 = new Point3(4, 5, 6);

            // Act
            var result = Point3.DistanceSquared(value1, value2);

            // Assert
            Assert.Equal(27, result);
        }

        [Fact]
        public void Point3_Dot_ReturnsDotProduct()
        {
            // Arrange
            var point1 = new Point3(1, 2, 3);
            var point2 = new Point3(4, 5, 6);

            // Act
            var result = Point3.Dot(point1, point2);

            // Assert
            Assert.Equal(32, result);
        }

        [Fact]
        public void Point3_Max_ReturnsMaximizedPoint()
        {
            // Arrange
            var value1 = new Point3(1, 5, 3);
            var value2 = new Point3(4, 2, 6);

            // Act
            var result = Point3.Max(value1, value2);

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(5, result.Y);
            Assert.Equal(6, result.Z);
        }

        [Fact]
        public void Point3_Min_ReturnsMinimizedPoint()
        {
            // Arrange
            var value1 = new Point3(1, 5, 3);
            var value2 = new Point3(4, 2, 6);

            // Act
            var result = Point3.Min(value1, value2);

            // Assert
            Assert.Equal(1, result.X);
            Assert.Equal(2, result.Y);
            Assert.Equal(3, result.Z);
        }

        [Fact]
        public void Point3_Length_ReturnsCorrectLength()
        {
            // Arrange
            var point = new Point3(3, 4, 5);
            float expectedLength = 7.071068f;

            // Act
            var result = point.Length();

            // Assert
            Assert.Equal(expectedLength, result, 1E-5f);
        }

        [Fact]
        public void Point3_LengthSquared_ReturnsCorrectLengthSquared()
        {
            // Arrange
            var point = new Point3(3, 4, 5);
            int expectedLengthSquared = 50;

            // Act
            var result = point.LengthSquared();

            // Assert
            Assert.Equal(expectedLengthSquared, result);
        }

        [Fact]
        public void Point3_CopyTo_Array_CopiesElementsToSpecifiedArray()
        {
            // Arrange
            var point = new Point3(1, 2, 3);
            int[] array = new int[3];

            // Act
            point.CopyTo(array);

            // Assert
            Assert.Equal(1, array[0]);
            Assert.Equal(2, array[1]);
            Assert.Equal(3, array[2]);
        }

        [Fact]
        public void Point3_CopyTo_Array_Index_CopiesElementsToSpecifiedArrayStartingAtIndex()
        {
            // Arrange
            var point = new Point3(1, 2, 3);
            int[] array = new int[5];

            // Act
            point.CopyTo(array, 2);

            // Assert
            Assert.Equal(0, array[0]);
            Assert.Equal(0, array[1]);
            Assert.Equal(1, array[2]);
            Assert.Equal(2, array[3]);
            Assert.Equal(3, array[4]);
        }

        [Fact]
        public void Point3_CopyTo_Span_CopiesElementsToSpecifiedSpan()
        {
            // Arrange
            var point = new Point3(1, 2, 3);
            Span<int> destination = new int[3];

            // Act
            point.CopyTo(destination);

            // Assert
            Assert.Equal(1, destination[0]);
            Assert.Equal(2, destination[1]);
            Assert.Equal(3, destination[2]);
        }

        [Fact]
        public void Point3_TryCopyTo_Span_ReturnsTrueAndCopiesElementsToSpecifiedSpan()
        {
            // Arrange
            var point = new Point3(1, 2, 3);
            Span<int> destination = new int[3];

            // Act
            var result = point.TryCopyTo(destination);

            // Assert
            Assert.True(result);
            Assert.Equal(1, destination[0]);
            Assert.Equal(2, destination[1]);
            Assert.Equal(3, destination[2]);
        }

        [Fact]
        public void Point3_TryCopyTo_Span_ReturnsFalseWhenDestinationSpanIsTooSmall()
        {
            // Arrange
            var point = new Point3(1, 2, 3);
            Span<int> destination = new int[2];

            // Act
            var result = point.TryCopyTo(destination);

            // Assert
            Assert.False(result);
        }
    }
}
