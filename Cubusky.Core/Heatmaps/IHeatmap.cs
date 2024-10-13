using Cubusky.Numerics;
using System;
using System.Collections.Generic;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a heatmap that maps cells / points to their corresponding positions / vectors.</summary>
    /// <typeparam name="TPoint">The type of the points representing the cells in the heatmap.</typeparam>
    /// <typeparam name="TVector">The type of the vectors representing the positions in the heatmap.</typeparam>
    public interface IHeatmap<TPoint, TVector> : ICollection<TPoint>
        where TPoint : IIndexable<int>
    {
        /// <summary>Gets the strength at the specified cell.</summary>
        /// <param name="cell">The point representing the cell in the heatmap.</param>
        /// <returns>The strength at the specified cell.</returns>
        int this[TPoint cell] { get; }

        /// <summary>Gets the cell corresponding to the specified position.</summary>
        /// <param name="position">The position vector.</param>
        /// <returns>The cell corresponding to the specified position.</returns>
        TPoint GetCell(TVector position);

        /// <summary>Gets the position corresponding to the specified cell.</summary>
        /// <param name="cell">The point representing the cell in the heatmap.</param>
        /// <returns>The position corresponding to the specified cell.</returns>
        TVector GetPosition(TPoint cell);

        /// <summary>Adds a strength at the specified cell.</summary>
        /// <param name="cell">The point representing the cell in the heatmap.</param>
        /// <param name="strength">The strength to add at the specified cell.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the strength value is negative or zero.</exception>
        void Add(TPoint cell, int strength);

        /// <summary>Removes a strength at the specified cell. If the remaining strength is 0 or less, the cell is removed from the heatmap.</summary>
        /// <param name="cell">The point representing the cell in the heatmap.</param>
        /// <param name="strength">The strength to remove at the specified cell. If the remaining strength is zero or less, the cell is removed from the heatmap.</param>
        /// <returns><see langword="true" /> if the strength was successfully removed; otherwise, <see langword="false" />. This method returns <see langword="false" /> if <paramref name="cell"/> is not found in the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the strength value is negative or zero.</exception>
        bool Remove(TPoint cell, int strength);
    }
}
