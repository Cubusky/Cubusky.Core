using Cubusky.Numerics;
using System;
using System.Globalization;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Numerics
{
    public class RectangleTests
    {
        [Fact]
        public void Constructor_ValidParameters_ShouldCreateRectangle()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            Assert.Equal(1, rectangle.X);
            Assert.Equal(2, rectangle.Y);
            Assert.Equal(3, rectangle.Width);
            Assert.Equal(4, rectangle.Height);
        }

        [Fact]
        public void Constructor_NegativeWidth_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(1, 2, -1, 4));
        }

        [Fact]
        public void Constructor_NegativeHeight_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(1, 2, 3, -1));
        }

        [Fact]
        public void FromEdges_ValidParameters_ShouldCreateRectangle()
        {
            var rectangle = Rectangle.FromEdges(1, 2, 5, 6);
            Assert.Equal(1, rectangle.Left);
            Assert.Equal(2, rectangle.Top);
            Assert.Equal(4, rectangle.Width);
            Assert.Equal(4, rectangle.Height);
        }

        [Fact]
        public void FromEdges_InvalidParameters_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Rectangle.FromEdges(5, 2, 1, 6));
        }

        [Fact]
        public void Contains_PointInsideRectangle_ShouldReturnTrue()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Point2(2, 3);
            Assert.True(rectangle.Contains(point));
        }

        [Fact]
        public void Contains_PointOutsideRectangle_ShouldReturnFalse()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Point2(5, 6);
            Assert.False(rectangle.Contains(point));
        }

        [Fact]
        public void Intersects_RectanglesIntersect_ShouldReturnTrue()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(2, 3, 3, 4);
            Assert.True(rectangle1.Intersects(rectangle2));
        }

        [Fact]
        public void Intersects_RectanglesDoNotIntersect_ShouldReturnFalse()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(5, 6, 3, 4);
            Assert.False(rectangle1.Intersects(rectangle2));
        }

        [Fact]
        public void Equals_RectanglesAreEqual_ShouldReturnTrue()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(1, 2, 3, 4);
            Assert.True(rectangle1.Equals(rectangle2));
        }

        [Fact]
        public void Equals_RectanglesAreNotEqual_ShouldReturnFalse()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(1, 2, 3, 5);
            Assert.False(rectangle1.Equals(rectangle2));
        }

        [Fact]
        public void GetHashCode_EqualRectangles_ShouldReturnSameHashCode()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(1, 2, 3, 4);
            Assert.Equal(rectangle1.GetHashCode(), rectangle2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NotEqualRectangles_ShouldNotReturnSameHashCode()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(1, 2, 3, 5);
            Assert.NotEqual(rectangle1.GetHashCode(), rectangle2.GetHashCode());
        }

        [Fact]
        public void ClosestPoint_PointInsideRectangle_ShouldReturnSamePoint()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Vector2(2, 3);
            var closestPoint = Rectangle.ClosestPoint(rectangle, point);
            Assert.Equal(point, closestPoint);
        }

        [Fact]
        public void ClosestPoint_PointOutsideRectangle_ShouldReturnClosestPoint()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Vector2(10, 10);
            var expected = new Vector2(4, 6);
            var closestPoint = Rectangle.ClosestPoint(rectangle, point);
            Assert.Equal(expected, closestPoint);
        }

        [Fact]
        public void DistanceSquared_PointInsideRectangle_ShouldReturnZero()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Vector2(2, 3);
            var distanceSquared = Rectangle.DistanceSquared(rectangle, point);
            Assert.Equal(0, distanceSquared);
        }

        [Fact]
        public void DistanceSquared_PointOutsideRectangle_ShouldReturnCorrectDistanceSquared()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Vector2(10, 10);
            var distanceSquared = Rectangle.DistanceSquared(rectangle, point);
            Assert.Equal(Vector2.DistanceSquared(point, new Vector2(4, 6)), distanceSquared);
        }

        [Fact]
        public void Distance_PointInsideRectangle_ShouldReturnZero()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Vector2(2, 3);
            var distance = Rectangle.Distance(rectangle, point);
            Assert.Equal(0, distance);
        }

        [Fact]
        public void Distance_PointOutsideRectangle_ShouldReturnCorrectDistance()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Vector2(10, 10);
            var distance = Rectangle.Distance(rectangle, point);
            Assert.Equal(Vector2.Distance(point, new Vector2(4, 6)), distance);
        }

        [Fact]
        public void Encapsulate_PointOutsideRectangle_ShouldExpandRectangle()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var point = new Point2(10, 10);
            var encapsulatedRectangle = Rectangle.Encapsulate(rectangle, point);
            Assert.Equal(Rectangle.FromEdges(1, 2, 10, 10), encapsulatedRectangle);
        }

        [Fact]
        public void Intersect_RectanglesIntersect_ShouldReturnIntersection()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(2, 3, 3, 4);
            var intersection = Rectangle.Intersect(rectangle1, rectangle2);
            Assert.Equal(Rectangle.FromEdges(2, 3, 4, 6), intersection);
        }

        [Fact]
        public void Intersect_RectanglesDoNotIntersect_ShouldReturnEmptyRectangle()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(10, 10, 3, 4);
            var intersection = Rectangle.Intersect(rectangle1, rectangle2);
            Assert.Equal(Rectangle.Zero, intersection);
        }

        [Fact]
        public void Union_Rectangles_ShouldReturnUnion()
        {
            var rectangle1 = new Rectangle(1, 2, 3, 4);
            var rectangle2 = new Rectangle(2, 3, 3, 4);
            var union = Rectangle.Union(rectangle1, rectangle2);
            Assert.Equal(Rectangle.FromEdges(1, 2, 5, 7), union);
        }

        [Fact]
        public void ToString_DefaultFormat_ShouldReturnCorrectString()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var expected = "{1, 2, 3, 4}";
            Assert.Equal(expected, rectangle.ToString());
        }

        [Fact]
        public void ToString_CustomFormat_ShouldReturnCorrectString()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var expected = "{1.00, 2.00, 3.00, 4.00}";
            Assert.Equal(expected, rectangle.ToString("F2", CultureInfo.InvariantCulture));
        }
    }
}
