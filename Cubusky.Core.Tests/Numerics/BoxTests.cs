using Cubusky.Numerics;
using System;
using System.Globalization;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Numerics
{
    public class BoxTests
    {
        [Fact]
        public void Constructor_ValidParameters_ShouldCreateBox()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            Assert.Equal(1, box.X);
            Assert.Equal(2, box.Y);
            Assert.Equal(3, box.Z);
            Assert.Equal(4, box.Width);
            Assert.Equal(5, box.Height);
            Assert.Equal(6, box.Depth);
        }

        [Fact]
        public void Constructor_NegativeWidth_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Box(1, 2, 3, -1, 5, 6));
        }

        [Fact]
        public void Constructor_NegativeHeight_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Box(1, 2, 3, 4, -1, 6));
        }

        [Fact]
        public void Constructor_NegativeDepth_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Box(1, 2, 3, 4, 5, -1));
        }

        [Fact]
        public void FromEdges_ValidParameters_ShouldCreateBox()
        {
            var box = Box.FromEdges(1, 2, 3, 5, 6, 7);
            Assert.Equal(1, box.Left);
            Assert.Equal(2, box.Top);
            Assert.Equal(3, box.Front);
            Assert.Equal(4, box.Width);
            Assert.Equal(4, box.Height);
            Assert.Equal(4, box.Depth);
        }

        [Fact]
        public void FromEdges_InvalidParameters_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Box.FromEdges(5, 2, 3, 1, 6, 7));
        }

        [Fact]
        public void Contains_PointInsideBox_ShouldReturnTrue()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Point3(2, 3, 4);
            Assert.True(box.Contains(point));
        }

        [Fact]
        public void Contains_PointOutsideBox_ShouldReturnFalse()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Point3(6, 7, 8);
            Assert.False(box.Contains(point));
        }

        [Fact]
        public void Intersects_BoxesIntersect_ShouldReturnTrue()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(3, 4, 5, 4, 5, 6);
            Assert.True(box1.Intersects(box2));
        }

        [Fact]
        public void Intersects_BoxesDoNotIntersect_ShouldReturnFalse()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(6, 7, 8, 4, 5, 6);
            Assert.False(box1.Intersects(box2));
        }

        [Fact]
        public void Equals_BoxesAreEqual_ShouldReturnTrue()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(1, 2, 3, 4, 5, 6);
            Assert.True(box1.Equals(box2));
        }

        [Fact]
        public void Equals_BoxesAreNotEqual_ShouldReturnFalse()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(1, 2, 3, 4, 5, 7);
            Assert.False(box1.Equals(box2));
        }

        [Fact]
        public void GetHashCode_EqualBoxes_ShouldReturnSameHashCode()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(1, 2, 3, 4, 5, 6);
            Assert.Equal(box1.GetHashCode(), box2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NotEqualBoxes_ShouldNotReturnSameHashCode()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(1, 2, 3, 4, 5, 7);
            Assert.NotEqual(box1.GetHashCode(), box2.GetHashCode());
        }

        [Fact]
        public void ClosestPoint_PointInsideBox_ShouldReturnSamePoint()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Vector3(2, 3, 4);
            var closestPoint = Box.ClosestPoint(box, point);
            Assert.Equal(point, closestPoint);
        }

        [Fact]
        public void ClosestPoint_PointOutsideBox_ShouldReturnClosestPoint()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            var expected = new Vector3(5, 7, 9);
            var closestPoint = Box.ClosestPoint(box, point);
            Assert.Equal(expected, closestPoint);
        }

        [Fact]
        public void DistanceSquared_PointInsideBox_ShouldReturnZero()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Vector3(2, 3, 4);
            var distanceSquared = Box.DistanceSquared(box, point);
            Assert.Equal(0, distanceSquared);
        }

        [Fact]
        public void DistanceSquared_PointOutsideBox_ShouldReturnCorrectDistanceSquared()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            var distanceSquared = Box.DistanceSquared(box, point);
            Assert.Equal(Vector3.DistanceSquared(point, new Vector3(5, 7, 9)), distanceSquared);
        }

        [Fact]
        public void Distance_PointInsideBox_ShouldReturnZero()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Vector3(2, 3, 4);
            var distance = Box.Distance(box, point);
            Assert.Equal(0, distance);
        }

        [Fact]
        public void Distance_PointOutsideBox_ShouldReturnCorrectDistance()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            var distance = Box.Distance(box, point);
            Assert.Equal(Vector3.Distance(point, new Vector3(5, 7, 9)), distance);
        }

        [Fact]
        public void Encapsulate_PointOutsideBox_ShouldExpandBox()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var point = new Point3(10, 10, 10);
            var encapsulatedBox = Box.Encapsulate(box, point);
            Assert.Equal(Box.FromEdges(1, 2, 3, 10, 10, 10), encapsulatedBox);
        }

        [Fact]
        public void Intersect_BoxesIntersect_ShouldReturnIntersection()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(3, 4, 5, 4, 5, 6);
            var intersection = Box.Intersect(box1, box2);
            Assert.Equal(Box.FromEdges(3, 4, 5, 5, 7, 9), intersection);
        }

        [Fact]
        public void Intersect_BoxesDoNotIntersect_ShouldReturnEmptyBox()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(10, 10, 10, 4, 5, 6);
            var intersection = Box.Intersect(box1, box2);
            Assert.Equal(Box.Zero, intersection);
        }

        [Fact]
        public void Union_Boxes_ShouldReturnUnion()
        {
            var box1 = new Box(1, 2, 3, 4, 5, 6);
            var box2 = new Box(3, 4, 5, 4, 5, 6);
            var union = Box.Union(box1, box2);
            Assert.Equal(Box.FromEdges(1, 2, 3, 7, 9, 11), union);
        }

        [Fact]
        public void ToString_DefaultFormat_ShouldReturnCorrectString()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var expected = "{1, 2, 3, 4, 5, 6}";
            Assert.Equal(expected, box.ToString());
        }

        [Fact]
        public void ToString_CustomFormat_ShouldReturnCorrectString()
        {
            var box = new Box(1, 2, 3, 4, 5, 6);
            var expected = "{1.00, 2.00, 3.00, 4.00, 5.00, 6.00}";
            Assert.Equal(expected, box.ToString("F2", CultureInfo.InvariantCulture));
        }
    }
}
