using Cubusky.Numerics;
using System;
using System.Globalization;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Numerics
{
    public class Bounds2Tests
    {
        [Fact]
        public void Constructor_ValidParameters_ShouldCreateBounds2()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            Assert.Equal(1, bounds.X);
            Assert.Equal(2, bounds.Y);
            Assert.Equal(3, bounds.Width);
            Assert.Equal(4, bounds.Height);
        }

        [Fact]
        public void Constructor_NegativeWidth_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bounds2(1, 2, -1, 4));
        }

        [Fact]
        public void Constructor_NegativeHeight_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bounds2(1, 2, 3, -1));
        }

        [Fact]
        public void Center_Get_ShouldReturnCorrectCenter()
        {
            var bounds = new Bounds2(1, 2, 4, 6);
            var expectedCenter = new Vector2(3, 5);
            Assert.Equal(expectedCenter, bounds.Center);
        }

        [Fact]
        public void Center_Set_ShouldUpdatePosition()
        {
            var bounds = new Bounds2(1, 2, 4, 6);
            var newCenter = new Vector2(5, 7);
            bounds.Center = newCenter;
            Assert.Equal(newCenter, bounds.Center);
            Assert.Equal(3, bounds.X);
            Assert.Equal(4, bounds.Y);
        }

        [Fact]
        public void Extents_Get_ShouldReturnCorrectExtents()
        {
            var bounds = new Bounds2(1, 2, 4, 6);
            var expectedExtents = new Vector2(2, 3);
            Assert.Equal(expectedExtents, bounds.Extents);
            Assert.Equal(4, bounds.Width);
            Assert.Equal(6, bounds.Height);
        }

        [Fact]
        public void Extents_Set_ShouldUpdateSizeAndPosition()
        {
            var bounds = new Bounds2(1, 2, 4, 6);
            var newExtents = new Vector2(3, 4);
            bounds.Extents = newExtents;
            Assert.Equal(newExtents, bounds.Extents);
            Assert.Equal(0, bounds.X);
            Assert.Equal(1, bounds.Y);
            Assert.Equal(6, bounds.Width);
            Assert.Equal(8, bounds.Height);
        }

        [Fact]
        public void Expand_FromTopLeft_ShouldNotMoveBounds2()
        {
            var bounds = new Bounds2(1, 2, 4, 6);
            bounds = bounds.Expand(new Vector2(1, 2), new Vector2(1, 2));
            Assert.Equal(1, bounds.X);
            Assert.Equal(2, bounds.Y);
            Assert.Equal(5, bounds.Width);
            Assert.Equal(8, bounds.Height);
        }

        [Fact]
        public void Expand_FromBottomRight_ShouldMoveBounds2()
        {
            var bounds = new Bounds2(1, 2, 4, 6);
            bounds = bounds.Expand(new Vector2(1, 2), new Vector2(5, 8));
            Assert.Equal(0, bounds.X);
            Assert.Equal(0, bounds.Y);
            Assert.Equal(5, bounds.Width);
            Assert.Equal(8, bounds.Height);
        }

        [Fact]
        public void Expand_FromCenter_ShouldPartiallyMoveBounds2()
        {
            var bounds = new Bounds2(1, 2, 4, 6);
            bounds = bounds.Expand(new Vector2(1, 2), new Vector2(3, 5));
            Assert.Equal(0.5f, bounds.X);
            Assert.Equal(1, bounds.Y);
            Assert.Equal(5, bounds.Width);
            Assert.Equal(8, bounds.Height);
        }

        [Fact]
        public void FromEdges_ValidParameters_ShouldCreateBounds2()
        {
            var bounds = Bounds2.FromEdges(1, 2, 5, 6);
            Assert.Equal(1, bounds.Left);
            Assert.Equal(2, bounds.Top);
            Assert.Equal(4, bounds.Width);
            Assert.Equal(4, bounds.Height);
        }

        [Fact]
        public void FromEdges_InvalidParameters_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Bounds2.FromEdges(5, 2, 1, 6));
        }

        [Fact]
        public void Contains_PointInsideBounds_ShouldReturnTrue()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(2, 3);
            Assert.True(bounds.Contains(point));
        }

        [Fact]
        public void Contains_PointOutsideBounds_ShouldReturnFalse()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(5, 6);
            Assert.False(bounds.Contains(point));
        }

        [Fact]
        public void Intersects_BoundsIntersect_ShouldReturnTrue()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(2, 3, 3, 4);
            Assert.True(bounds1.Intersects(bounds2));
        }

        [Fact]
        public void Intersects_BoundsDoNotIntersect_ShouldReturnFalse()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(5, 6, 3, 4);
            Assert.False(bounds1.Intersects(bounds2));
        }

        [Fact]
        public void Equals_BoundsAreEqual_ShouldReturnTrue()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(1, 2, 3, 4);
            Assert.True(bounds1.Equals(bounds2));
        }

        [Fact]
        public void Equals_BoundsAreNotEqual_ShouldReturnFalse()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(1, 2, 3, 5);
            Assert.False(bounds1.Equals(bounds2));
        }

        [Fact]
        public void GetHashCode_EqualBounds_ShouldReturnSameHashCode()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(1, 2, 3, 4);
            Assert.Equal(bounds1.GetHashCode(), bounds2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NotEqualBounds_ShouldNotReturnSameHashCode()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(1, 2, 3, 5);
            Assert.NotEqual(bounds1.GetHashCode(), bounds2.GetHashCode());
        }

        [Fact]
        public void ClosestPoint_PointInsideBounds_ShouldReturnSamePoint()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(2, 3);
            var closestPoint = Bounds2.ClosestPoint(bounds, point);
            Assert.Equal(point, closestPoint);
        }

        [Fact]
        public void ClosestPoint_PointOutsideBounds_ShouldReturnClosestPoint()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(10, 10);
            var expected = new Vector2(4, 6);
            var closestPoint = Bounds2.ClosestPoint(bounds, point);
            Assert.Equal(expected, closestPoint);
        }

        [Fact]
        public void DistanceSquared_PointInsideBounds_ShouldReturnZero()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(2, 3);
            var distanceSquared = Bounds2.DistanceSquared(bounds, point);
            Assert.Equal(0, distanceSquared);
        }

        [Fact]
        public void DistanceSquared_PointOutsideBounds_ShouldReturnCorrectDistanceSquared()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(10, 10);
            var distanceSquared = Bounds2.DistanceSquared(bounds, point);
            Assert.Equal(Vector2.DistanceSquared(point, new Vector2(4, 6)), distanceSquared);
        }

        [Fact]
        public void Distance_PointInsideBounds_ShouldReturnZero()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(2, 3);
            var distance = Bounds2.Distance(bounds, point);
            Assert.Equal(0, distance);
        }

        [Fact]
        public void Distance_PointOutsideBounds_ShouldReturnCorrectDistance()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(10, 10);
            var distance = Bounds2.Distance(bounds, point);
            Assert.Equal(Vector2.Distance(point, new Vector2(4, 6)), distance);
        }

        [Fact]
        public void Encapsulate_PointOutsideBounds_ShouldExpandBounds()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var point = new Vector2(10, 10);
            var encapsulatedBounds = Bounds2.Encapsulate(bounds, point);
            Assert.Equal(Bounds2.FromEdges(1, 2, 10, 10), encapsulatedBounds);
        }

        [Fact]
        public void Intersect_BoundsIntersect_ShouldReturnIntersection()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(2, 3, 3, 4);
            var intersection = Bounds2.Intersect(bounds1, bounds2);
            Assert.Equal(Bounds2.FromEdges(2, 3, 4, 6), intersection);
        }

        [Fact]
        public void Intersect_BoundsDoNotIntersect_ShouldReturnEmptyBounds()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(10, 10, 3, 4);
            var intersection = Bounds2.Intersect(bounds1, bounds2);
            Assert.Equal(Bounds2.Zero, intersection);
        }

        [Fact]
        public void Union_Bounds_ShouldReturnUnion()
        {
            var bounds1 = new Bounds2(1, 2, 3, 4);
            var bounds2 = new Bounds2(2, 3, 3, 4);
            var union = Bounds2.Union(bounds1, bounds2);
            Assert.Equal(Bounds2.FromEdges(1, 2, 5, 7), union);
        }

        [Fact]
        public void ToString_DefaultFormat_ShouldReturnCorrectString()
        {
            var separator = NumberFormatInfo.GetInstance(CultureInfo.CurrentCulture).NumberGroupSeparator;

            var bounds = new Bounds2(1, 2, 3, 4);
            var expected = $"{{1{separator} 2{separator} 3{separator} 4}}";
            Assert.Equal(expected, bounds.ToString());
        }

        [Fact]
        public void ToString_CustomFormat_ShouldReturnCorrectString()
        {
            var bounds = new Bounds2(1, 2, 3, 4);
            var expected = "{1.00, 2.00, 3.00, 4.00}";
            Assert.Equal(expected, bounds.ToString("F2", CultureInfo.InvariantCulture));
        }
    }
}
