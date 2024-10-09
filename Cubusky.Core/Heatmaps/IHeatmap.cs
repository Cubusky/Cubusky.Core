using Cubusky.Numerics;
using System.Collections.Generic;

namespace Cubusky.Heatmaps
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IHeatmap<TPoint, TVector> : ICollection<TPoint>
        where TPoint : IIndexable<int>
    {
        int this[TPoint cell] { get; }

        TPoint GetCell(TVector position);
        TVector GetPosition(TPoint cell);

        void Add(TPoint cell, int strength);
        bool Remove(TPoint cell, int strength);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
