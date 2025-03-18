using Cubusky.Numerics;
using System;
using System.Globalization;
using System.Numerics;
using Xunit;

namespace Cubusky.Tests.Numerics
{
    public class Bounds3Tests
    {
        [Fact]
        public void Constructor_ValidParameters_ShouldCreateBounds3()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            Assert.Equal(1, bounds.X);
            Assert.Equal(2, bounds.Y);
            Assert.Equal(3, bounds.Z);
            Assert.Equal(4, bounds.Width);
            Assert.Equal(5, bounds.Height);
            Assert.Equal(6, bounds.Depth);
        }

        [Fact]
        public void Constructor_NegativeWidth_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bounds3(1, 2, 3, -1, 5, 6));
        }

        [Fact]
        public void Constructor_NegativeHeight_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bounds3(1, 2, 3, 4, -1, 6));
        }

        [Fact]
        public void Constructor_NegativeDepth_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Bounds3(1, 2, 3, 4, 5, -1));
        }

        [Fact]
        public void Center_Get_ShouldReturnCorrectCenter()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 6, 8);
            var expectedCenter = new Vector3(3, 5, 7);
            Assert.Equal(expectedCenter, bounds.Center);
        }

        [Fact]
        public void Center_Set_ShouldUpdatePosition()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 6, 8);
            var newCenter = new Vector3(5, 7, 9);
            bounds.Center = newCenter;
            Assert.Equal(newCenter, bounds.Center);
            Assert.Equal(3, bounds.X);
            Assert.Equal(4, bounds.Y);
            Assert.Equal(5, bounds.Z);
        }

        [Fact]
        public void Extents_Get_ShouldReturnCorrectExtents()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 6, 8);
            var expectedExtents = new Vector3(2, 3, 4);
            Assert.Equal(expectedExtents, bounds.Extents);
            Assert.Equal(4, bounds.Width);
            Assert.Equal(6, bounds.Height);
            Assert.Equal(8, bounds.Depth);
        }

        [Fact]
        public void Extents_Set_ShouldUpdateSizeAndPosition()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 6, 8);
            var newExtents = new Vector3(3, 4, 5);
            bounds.Extents = newExtents;
            Assert.Equal(newExtents, bounds.Extents);
            Assert.Equal(0, bounds.X);
            Assert.Equal(1, bounds.Y);
            Assert.Equal(2, bounds.Z);
            Assert.Equal(6, bounds.Width);
            Assert.Equal(8, bounds.Height);
            Assert.Equal(10, bounds.Depth);
        }

        [Fact]
        public void Expand_FromFrontTopLeft_ShouldNotMoveBounds2()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 6, 8);
            bounds = bounds.Expand(new Vector3(1, 2, 3), new Vector3(1, 2, 3));
            Assert.Equal(1, bounds.X);
            Assert.Equal(2, bounds.Y);
            Assert.Equal(3, bounds.Z);
            Assert.Equal(5, bounds.Width);
            Assert.Equal(8, bounds.Height);
            Assert.Equal(11, bounds.Depth);
        }

        [Fact]
        public void Expand_FromBottomRight_ShouldMoveBounds2()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 6, 8);
            bounds = bounds.Expand(new Vector3(1, 2, 3), new Vector3(5, 8, 11));
            Assert.Equal(0, bounds.X);
            Assert.Equal(0, bounds.Y);
            Assert.Equal(0, bounds.Z);
            Assert.Equal(5, bounds.Width);
            Assert.Equal(8, bounds.Height);
            Assert.Equal(11, bounds.Depth);
        }

        [Fact]
        public void Expand_FromCenter_ShouldPartiallyMoveBounds2()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 6, 8);
            bounds = bounds.Expand(new Vector3(1, 2, 3), new Vector3(3, 5, 7));
            Assert.Equal(0.5f, bounds.X);
            Assert.Equal(1, bounds.Y);
            Assert.Equal(1.5f, bounds.Z);
            Assert.Equal(5, bounds.Width);
            Assert.Equal(8, bounds.Height);
            Assert.Equal(11, bounds.Depth);
        }

        [Fact]
        public void FromEdges_ValidParameters_ShouldCreateBounds3()
        {
            var bounds = Bounds3.FromEdges(1, 2, 3, 5, 6, 7);
            Assert.Equal(1, bounds.Left);
            Assert.Equal(2, bounds.Top);
            Assert.Equal(3, bounds.Front);
            Assert.Equal(4, bounds.Width);
            Assert.Equal(4, bounds.Height);
            Assert.Equal(4, bounds.Depth);
        }

        [Fact]
        public void FromEdges_InvalidParameters_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Bounds3.FromEdges(5, 2, 3, 1, 6, 7));
        }

        [Fact]
        public void Contains_PointInsideBounds_ShouldReturnTrue()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(2, 3, 4);
            Assert.True(bounds.Contains(point));
        }

        [Fact]
        public void Contains_PointOutsideBounds_ShouldReturnFalse()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            Assert.False(bounds.Contains(point));
        }

        [Fact]
        public void Intersects_BoundsIntersect_ShouldReturnTrue()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(2, 3, 4, 4, 5, 6);
            Assert.True(bounds1.Intersects(bounds2));
        }

        [Fact]
        public void Intersects_BoundsDoNotIntersect_ShouldReturnFalse()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(10, 10, 10, 4, 5, 6);
            Assert.False(bounds1.Intersects(bounds2));
        }

        [Fact]
        public void Equals_BoundsAreEqual_ShouldReturnTrue()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(1, 2, 3, 4, 5, 6);
            Assert.True(bounds1.Equals(bounds2));
        }

        [Fact]
        public void Equals_BoundsAreNotEqual_ShouldReturnFalse()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(1, 2, 3, 4, 5, 7);
            Assert.False(bounds1.Equals(bounds2));
        }

        [Fact]
        public void GetHashCode_EqualBounds_ShouldReturnSameHashCode()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(1, 2, 3, 4, 5, 6);
            Assert.Equal(bounds1.GetHashCode(), bounds2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NotEqualBounds_ShouldNotReturnSameHashCode()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(1, 2, 3, 4, 5, 7);
            Assert.NotEqual(bounds1.GetHashCode(), bounds2.GetHashCode());
        }

        [Fact]
        public void ClosestPoint_PointInsideBounds_ShouldReturnSamePoint()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(2, 3, 4);
            var closestPoint = Bounds3.ClosestPoint(bounds, point);
            Assert.Equal(point, closestPoint);
        }

        [Fact]
        public void ClosestPoint_PointOutsideBounds_ShouldReturnClosestPoint()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            var expected = new Vector3(5, 7, 9);
            var closestPoint = Bounds3.ClosestPoint(bounds, point);
            Assert.Equal(expected, closestPoint);
        }

        [Fact]
        public void DistanceSquared_PointInsideBounds_ShouldReturnZero()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(2, 3, 4);
            var distanceSquared = Bounds3.DistanceSquared(bounds, point);
            Assert.Equal(0, distanceSquared);
        }

        [Fact]
        public void DistanceSquared_PointOutsideBounds_ShouldReturnCorrectDistanceSquared()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            var distanceSquared = Bounds3.DistanceSquared(bounds, point);
            Assert.Equal(Vector3.DistanceSquared(point, new Vector3(5, 7, 9)), distanceSquared);
        }

        [Fact]
        public void Distance_PointInsideBounds_ShouldReturnZero()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(2, 3, 4);
            var distance = Bounds3.Distance(bounds, point);
            Assert.Equal(0, distance);
        }

        [Fact]
        public void Distance_PointOutsideBounds_ShouldReturnCorrectDistance()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            var distance = Bounds3.Distance(bounds, point);
            Assert.Equal(Vector3.Distance(point, new Vector3(5, 7, 9)), distance);
        }

        [Fact]
        public void Encapsulate_PointOutsideBounds_ShouldExpandBounds()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var point = new Vector3(10, 10, 10);
            var encapsulatedBounds = Bounds3.Encapsulate(bounds, point);
            Assert.Equal(Bounds3.FromEdges(1, 2, 3, 10, 10, 10), encapsulatedBounds);
        }

        [Fact]
        public void Intersect_BoundsIntersect_ShouldReturnIntersection()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(2, 3, 4, 4, 5, 6);
            var intersection = Bounds3.Intersect(bounds1, bounds2);
            Assert.Equal(Bounds3.FromEdges(2, 3, 4, 5, 7, 9), intersection);
        }

        [Fact]
        public void Intersect_BoundsDoNotIntersect_ShouldReturnEmptyBounds()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(10, 10, 10, 4, 5, 6);
            var intersection = Bounds3.Intersect(bounds1, bounds2);
            Assert.Equal(Bounds3.Zero, intersection);
        }

        [Fact]
        public void Union_Bounds_ShouldReturnUnion()
        {
            var bounds1 = new Bounds3(1, 2, 3, 4, 5, 6);
            var bounds2 = new Bounds3(2, 3, 4, 4, 5, 6);
            var union = Bounds3.Union(bounds1, bounds2);
            Assert.Equal(Bounds3.FromEdges(1, 2, 3, 6, 8, 10), union);
        }

        [Fact]
        public void ToString_DefaultFormat_ShouldReturnCorrectString()
        {
            var separator = NumberFormatInfo.GetInstance(CultureInfo.CurrentCulture).NumberGroupSeparator;

            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var expected = $"{{1{separator} 2{separator} 3{separator} 4{separator} 5{separator} 6}}";
            Assert.Equal(expected, bounds.ToString());
        }

        [Fact]
        public void ToString_CustomFormat_ShouldReturnCorrectString()
        {
            var bounds = new Bounds3(1, 2, 3, 4, 5, 6);
            var expected = "{1.00, 2.00, 3.00, 4.00, 5.00, 6.00}";
            Assert.Equal(expected, bounds.ToString("F2", CultureInfo.InvariantCulture));
        }
    }
}
