using System;
using System.Collections.Generic;
using System.Linq;

namespace Cubusky.Heatmaps
{
    /// <summary>Provides extension methods for the <see cref="IHeatmap{TPoint, TVector}"/> interface.</summary>
    public static class HeatmapExtensions
    {
        /// <summary>Gets the strengt at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The position vector.</param>
        /// <returns>The strength at the specified position.</returns>
        public static int Strength<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) => heatmap[heatmap.GetCell(position)];

        /// <summary>Adds a strength at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) => heatmap.Add(heatmap.GetCell(position));

        /// <summary>Adds a strength at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        /// <param name="strength">The strength to add at the specified position.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the strength value is negative or zero.</exception>
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position, int strength) => heatmap.Add(heatmap.GetCell(position), strength);

        /// <summary>Determines whether the <see cref="IHeatmap{TPoint, TVector}"/> contains a cell at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The position to check.</param>
        /// <returns><see langword="true" /> if the <see cref="IHeatmap{TPoint, TVector}"/> contains a cell at the specified position; otherwise, <see langword="false" />.</returns>
        public static bool Contains<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) => heatmap.Contains(heatmap.GetCell(position));

        /// <summary>Removes a strength at the specified position. If the remaining strength is 0 or less, the cell is removed from the heatmap.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        /// <returns><see langword="true" /> if the strength was successfully removed; otherwise, <see langword="false" />. This method returns <see langword="false" /> if <paramref name="position"/> is not found in the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        public static bool Remove<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) => heatmap.Remove(heatmap.GetCell(position));

        /// <summary>Removes a strength at the specified position. If the remaining strength is 0 or less, the cell is removed from the heatmap.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        /// <param name="strength">The strength to remove at the specified cell. If the remaining strength is zero or less, the cell is removed from the heatmap.</param>
        /// <returns><see langword="true" /> if the strength was successfully removed; otherwise, <see langword="false" />. This method returns <see langword="false" /> if <paramref name="position"/> is not found in the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the strength value is negative or zero.</exception>
        public static bool Remove<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position, int strength) => heatmap.Remove(heatmap.GetCell(position), strength);

        /// <summary>Returns an enumerator that iterates through the positions of the <see cref="IHeatmap{TPoint, TVector}"/>.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <returns>An <see cref="IEnumerator{T}"/> for the positions of the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        public static IEnumerable<TVector> GetPositions<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap) => heatmap.Select(heatmap.GetPosition);
    }
}
