using Cubusky.Numerics;
using System.Collections.Generic;

namespace Cubusky.Heatmaps.Json.Serialization
{
    internal class PointComparer : IComparer<Point2>, IComparer<Point3>, IComparer<Point4>
    {
        public int Compare(Point2 value, Point2 other)
        {
            int result;
            return (result = value.Y.CompareTo(other.Y)) == 0
                ? value.X.CompareTo(other.X)
                : result;
        }

        public int Compare(Point3 value, Point3 other)
        {
            int result;
            return (result = value.Z.CompareTo(other.Z)) == 0
                ? (result = value.Y.CompareTo(other.Y)) == 0
                    ? value.X.CompareTo(other.X)
                    : result
                : result;
        }

        public int Compare(Point4 value, Point4 other)
        {
            int result;
            return (result = value.W.CompareTo(other.W)) == 0
                ? (result = value.Z.CompareTo(other.Z)) == 0
                    ? (result = value.Y.CompareTo(other.Y)) == 0
                        ? value.X.CompareTo(other.X)
                        : result
                    : result
                : result;
        }
    }
}
