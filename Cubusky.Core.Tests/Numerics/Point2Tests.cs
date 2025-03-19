using Cubusky.Numerics;
using System;
using System.Globalization;
using Xunit;

namespace Cubusky.Tests.Numerics
{
    public class Point2Tests
    {
        [Fact]
        public void Point2_Constructor_SetsCorrectValues()
        {
            // Arrange
            int x = 1;
            int y = 2;

            // Act
            var point = new Point2(x, y);

            // Assert
            Assert.Equal(x, point.X);
            Assert.Equal(y, point.Y);
        }

        [Fact]
        public void Point2_Zero_ReturnsPointWithZeroValues()
        {
            // Act
            var point = Point2.Zero;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(0, point.Y);
        }

        [Fact]
        public void Point2_One_ReturnsPointWithOneValues()
        {
            // Act
            var point = Point2.One;

            // Assert
            Assert.Equal(1, point.X);
            Assert.Equal(1, point.Y);
        }

        [Fact]
        public void Point2_UnitX_ReturnsPointWithXValueOne()
        {
            // Act
            var point = Point2.UnitX;

            // Assert
            Assert.Equal(1, point.X);
            Assert.Equal(0, point.Y);
        }

        [Fact]
        public void Point2_UnitY_ReturnsPointWithYValueOne()
        {
            // Act
            var point = Point2.UnitY;

            // Assert
            Assert.Equal(0, point.X);
            Assert.Equal(1, point.Y);
        }

        [Fact]
        public void Point2_Addition_ReturnsSummedPoint()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(3, 4);

            // Act
            var result = point1 + point2;

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(6, result.Y);
        }

        [Fact]
        public void Point2_Subtraction_ReturnsSubtractedPoint()
        {
            // Arrange
            var point1 = new Point2(3, 4);
            var point2 = new Point2(1, 2);

            // Act
            var result = point1 - point2;

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(2, result.Y);
        }

        [Fact]
        public void Point2_Multiplication_ReturnsMultipliedPoint()
        {
            // Arrange
            var point = new Point2(2, 3);
            int scalar = 2;

            // Act
            var result = point * scalar;

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(6, result.Y);
        }

        [Fact]
        public void Point2_Division_ReturnsDividedPoint()
        {
            // Arrange
            var point = new Point2(4, 6);
            int scalar = 2;

            // Act
            var result = point / scalar;

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(3, result.Y);
        }

        [Fact]
        public void Point2_Modulus_ReturnsModulusPoint()
        {
            // Arrange
            var point = new Point2(5, 7);
            int scalar = 3;

            // Act
            var result = point % scalar;

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(1, result.Y);
        }

        [Fact]
        public void Point2_Equality_ReturnsTrueForEqualPoints()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(1, 2);

            // Act
            var result = point1 == point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Point2_Equality_ReturnsFalseForDifferentPoints()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(3, 4);

            // Act
            var result = point1 == point2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Point2_Inequality_ReturnsTrueForDifferentPoints()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(3, 4);

            // Act
            var result = point1 != point2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Point2_Inequality_ReturnsFalseForEqualPoints()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(1, 2);

            // Act
            var result = point1 != point2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Point2_GetHashCode_ReturnsDifferentHashCodesForDifferentPoints()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(3, 4);

            // Act
            var hashCode1 = point1.GetHashCode();
            var hashCode2 = point2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        [Fact]
        public void Point2_GetHashCode_ReturnsSameHashCodeForEqualPoints()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(1, 2);

            // Act
            var hashCode1 = point1.GetHashCode();
            var hashCode2 = point2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void Point2_ToString()
        {
            // Arrange
            var point = new Point2(1, 2);
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Act
            var result = point.ToString();

            // Assert
            Assert.Equal("<1, 2>", result);
        }

        [Fact]
        public void Point2_Abs_ReturnsPointWithAbsoluteValues()
        {
            // Arrange
            var point = new Point2(-1, -2);

            // Act
            var result = Point2.Abs(point);

            // Assert
            Assert.Equal(1, result.X);
            Assert.Equal(2, result.Y);
        }

        [Fact]
        public void Point2_Clamp_ReturnsRestrictedPoint()
        {
            // Arrange
            var value1 = new Point2(1, 7);
            var min = new Point2(2, 4);
            var max = new Point2(4, 6);

            // Act
            var result = Point2.Clamp(value1, min, max);

            // Assert
            Assert.Equal(2, result.X);
            Assert.Equal(6, result.Y);
        }

        [Fact]
        public void Point2_Distance_ReturnsEuclideanDistance()
        {
            // Arrange
            var value1 = new Point2(1, 2);
            var value2 = new Point2(3, 4);

            // Act
            var result = Point2.Distance(value1, value2);

            // Assert
            Assert.Equal(2.828427f, result, 1E-5f);
        }

        [Fact]
        public void Point2_DistanceSquared_ReturnsEuclideanDistanceSquared()
        {
            // Arrange
            var value1 = new Point2(1, 2);
            var value2 = new Point2(3, 4);

            // Act
            var result = Point2.DistanceSquared(value1, value2);

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public void Point2_Dot_ReturnsDotProduct()
        {
            // Arrange
            var point1 = new Point2(1, 2);
            var point2 = new Point2(3, 4);

            // Act
            var result = Point2.Dot(point1, point2);

            // Assert
            Assert.Equal(11, result);
        }

        [Fact]
        public void Point2_Max_ReturnsMaximizedPoint()
        {
            // Arrange
            var value1 = new Point2(1, 5);
            var value2 = new Point2(4, 2);

            // Act
            var result = Point2.Max(value1, value2);

            // Assert
            Assert.Equal(4, result.X);
            Assert.Equal(5, result.Y);
        }

        [Fact]
        public void Point2_Min_ReturnsMinimizedPoint()
        {
            // Arrange
            var value1 = new Point2(1, 5);
            var value2 = new Point2(4, 2);

            // Act
            var result = Point2.Min(value1, value2);

            // Assert
            Assert.Equal(1, result.X);
            Assert.Equal(2, result.Y);
        }

        [Fact]
        public void Point2_Length_ReturnsCorrectLength()
        {
            // Arrange
            var point = new Point2(3, 4);
            float expectedLength = 5f;

            // Act
            var result = point.Length();

            // Assert
            Assert.Equal(expectedLength, result, 1E-5f);
        }

        [Fact]
        public void Point2_LengthSquared_ReturnsCorrectLengthSquared()
        {
            // Arrange
            var point = new Point2(3, 4);
            int expectedLengthSquared = 25;

            // Act
            var result = point.LengthSquared();

            // Assert
            Assert.Equal(expectedLengthSquared, result);
        }

        [Fact]
        public void Point2_CopyTo_Array_CopiesElementsToSpecifiedArray()
        {
            // Arrange
            var point = new Point2(1, 2);
            int[] array = new int[2];

            // Act
            point.CopyTo(array);

            // Assert
            Assert.Equal(1, array[0]);
            Assert.Equal(2, array[1]);
        }

        [Fact]
        public void Point2_CopyTo_Array_Index_CopiesElementsToSpecifiedArrayStartingAtIndex()
        {
            // Arrange
            var point = new Point2(1, 2);
            int[] array = new int[4];

            // Act
            point.CopyTo(array, 2);

            // Assert
            Assert.Equal(0, array[0]);
            Assert.Equal(0, array[1]);
            Assert.Equal(1, array[2]);
            Assert.Equal(2, array[3]);
        }

        [Fact]
        public void Point2_CopyTo_Span_CopiesElementsToSpecifiedSpan()
        {
            // Arrange
            var point = new Point2(1, 2);
            Span<int> destination = new int[2];

            // Act
            point.CopyTo(destination);

            // Assert
            Assert.Equal(1, destination[0]);
            Assert.Equal(2, destination[1]);
        }

        [Fact]
        public void Point2_TryCopyTo_Span_ReturnsTrueAndCopiesElementsToSpecifiedSpan()
        {
            // Arrange
            var point = new Point2(1, 2);
            Span<int> destination = new int[2];

            // Act
            var result = point.TryCopyTo(destination);

            // Assert
            Assert.True(result);
            Assert.Equal(1, destination[0]);
            Assert.Equal(2, destination[1]);
        }

        [Fact]
        public void Point2_TryCopyTo_Span_ReturnsFalseWhenDestinationSpanIsTooSmall()
        {
            // Arrange
            var point = new Point2(1, 2);
            Span<int> destination = new int[1];

            // Act
            var result = point.TryCopyTo(destination);

            // Assert
            Assert.False(result);
        }
    }
}
