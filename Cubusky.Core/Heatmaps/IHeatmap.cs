using Cubusky.Collections.Generic;

namespace Cubusky.Heatmaps
{
    /// <summary>Represents a heatmap that maps cells / points to their corresponding positions / vectors.</summary>
    /// <typeparam name="TPoint">The type of the points representing the cells in the heatmap.</typeparam>
    /// <typeparam name="TVector">The type of the vectors representing the positions in the heatmap.</typeparam>
    public interface IHeatmap<TPoint, TVector> : ICounter<TPoint> //ICollection<KeyValuePair<TPoint, int>>
    {
        /// <summary>Gets the cell corresponding to the specified position.</summary>
        /// <param name="position">The position vector.</param>
        /// <returns>The cell corresponding to the specified position.</returns>
        TPoint GetCell(TVector position);

        /// <summary>Gets the position corresponding to the specified cell.</summary>
        /// <param name="cell">The point representing the cell in the heatmap.</param>
        /// <returns>The position corresponding to the specified cell.</returns>
        TVector GetPosition(TPoint cell);
    }
}
