/*
 * FAQ:
 * 
 * Q. Why is there no copy constructor for the Heatmap class?
 * A. Because the heatmap stores a simplified cell representation, yet we are unsure which positions it is supposed to represent. There are to many possibilities to consider when copying a heatmap, hence it should be left to a cudstom implementation to solve this issue specifically based on the program's needs.
 * 
 */

using Cubusky.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Cubusky.Heatmaps
{
    /// <summary>Provides extension methods for the <see cref="IHeatmap{TPoint, TVector}"/> interface.</summary>
    public static class HeatmapExtensions
    {
        /// <summary>Gets the strengt at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The position vector.</param>
        /// <returns>The strength at the specified position.</returns>
        public static int Strength<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap[heatmap.GetCell(position)];

        /// <summary>Adds a strength at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap.Add(heatmap.GetCell(position));

        /// <summary>Adds a strength at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        /// <param name="strength">The strength to add at the specified position.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the strength value is negative or zero.</exception>
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position, int strength) where TPoint : IIndexable<int> => heatmap.Add(heatmap.GetCell(position), strength);

        /// <summary>Determines whether the <see cref="IHeatmap{TPoint, TVector}"/> contains a cell at the specified position.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The position to check.</param>
        /// <returns><see langword="true" /> if the <see cref="IHeatmap{TPoint, TVector}"/> contains a cell at the specified position; otherwise, <see langword="false" />.</returns>
        public static bool Contains<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap.Contains(heatmap.GetCell(position));

        /// <summary>Removes a strength at the specified position. If the remaining strength is 0 or less, the cell is removed from the heatmap.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        /// <returns><see langword="true" /> if the strength was successfully removed; otherwise, <see langword="false" />. This method returns <see langword="false" /> if <paramref name="position"/> is not found in the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        public static bool Remove<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap.Remove(heatmap.GetCell(position));

        /// <summary>Removes a strength at the specified position. If the remaining strength is 0 or less, the cell is removed from the heatmap.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="position">The vector representing the position in the heatmap.</param>
        /// <param name="strength">The strength to remove at the specified cell. If the remaining strength is zero or less, the cell is removed from the heatmap.</param>
        /// <returns><see langword="true" /> if the strength was successfully removed; otherwise, <see langword="false" />. This method returns <see langword="false" /> if <paramref name="position"/> is not found in the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the strength value is negative or zero.</exception>
        public static bool Remove<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position, int strength) where TPoint : IIndexable<int> => heatmap.Remove(heatmap.GetCell(position), strength);

        /// <summary>Returns an enumerator that iterates through the positions of the <see cref="IHeatmap{TPoint, TVector}"/>.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <returns>An <see cref="IEnumerator{T}"/> for the positions of the <see cref="IHeatmap{TPoint, TVector}"/>.</returns>
        public static IEnumerable<TVector> GetPositions<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap) where TPoint : IIndexable<int> => heatmap.Select(heatmap.GetPosition);

        /// <summary>Converts the heatmap to a compact array representation.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <returns>
        /// <para>A compact array representation of the heatmap.</para>
        /// <para>Depending on count n of the <typeparamref name="TPoint"/>, the first n elements represent the cell in the heatmap, and the element after that represents the strength of that cell.</para>
        /// <para> For example, if n = 2, then the compact array for the heatmap [0, 0] = 1, [0, 1] = 2 would look like [0, 0, 1, 0, 1, 2]</para>
        /// </returns>
        public static int[] ToCompactArray<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap) where TPoint : IIndexable<int>
        {
            var cells = heatmap.GetEnumerator();
            if (!cells.MoveNext())
            {
                return Array.Empty<int>();
            }

#if NET8_0_OR_GREATER
            var count = TPoint.Count;
#else
            var count = cells.Current.Count();
#endif

            int[] array = new int[heatmap.Count * (count + 1)];
            int i = 0;
            do
            {
                var cell = cells.Current;
                for (int j = 0; j < count; j++)
                {
                    array[i++] = cell[j];
                }

                array[i++] = heatmap[cell];
            } while (cells.MoveNext());

            return array;
        }

        /// <summary>Adds cells from a compact array representation to the heatmap.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="compactArray">The compact array representation of strength by cell.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when length of the array is not divisable by <typeparamref name="TPoint"/>'s Count + 1.</exception>
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, int[] compactArray) where TPoint : IIndexable<int>, new()
        {
#if NET8_0_OR_GREATER
            var count = TPoint.Count;
            Throw.IfArgumentNotEqual(compactArray.Length % (count + 1), 0, nameof(compactArray));
#endif

            var cell = new TPoint();

#if !NET8_0_OR_GREATER
            var count = cell.Count();
            Throw.IfArgumentNotEqual(compactArray.Length % (count + 1), 0, nameof(compactArray));
#endif

            for (int i = 0; i < compactArray.Length; i += count + 1)
            {
                for (int j = 0; j < count; j++)
                {
                    cell[j] = compactArray[i + j];
                }
                heatmap.Add(cell, compactArray[i + count]);
            }
        }

        /// <summary>Converts the heatmap to a multidimensional array representation.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="offset">The offset of the multidimensional array.</param>
        /// <returns>
        /// <para>A multidimensional array representation of the heatmap, where the amount of dimensions is equal to count n of the <typeparamref name="TPoint"/>.</para>
        /// <para>The indices of the array plus the offset represents the cell in the heatmap, and the value at that index represents the strength of that cell.</para>
        /// <para>For example, if n = 2, then the multidimensional array for the heatmap [-2, 5] = 1, [-2, 6] = 2 would look like [ [1], [2] ] with an offset of TPoint(-2, 5)</para>
        /// </returns>
#if NET8_0_OR_GREATER
        [RequiresDynamicCode("The code for an array of the specified type might not be available.")]
#endif
#if NET9_0_OR_GREATER
        //[Obsolete("This method requires dynamic code and is not AOT compatible. Use the ToMultidimensionalArray(Type, out TPoint) implementation instead.")]
#endif
        public static Array ToMultidimensionalArray<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, out TPoint offset) where TPoint : IIndexable<int>, new()
        {
            // Return an empty multidimensional array if the heatmap doesn't contain any cells.
            using var cells = heatmap.GetEnumerator();
            if (!cells.MoveNext())
            {
                offset = new TPoint();

#if NET8_0_OR_GREATER
                return Array.CreateInstance(typeof(int), new int[TPoint.Count]);
#else
                return Array.CreateInstance(typeof(int), new int[offset.Count()]);
#endif
            }

            // Get our max, offset and count.
            var max = offset = cells.Current;
#if NET8_0_OR_GREATER
            var count = TPoint.Count;
#else
            var count = offset.Count();
#endif

            // Get the minimum and maximum points on the multidimensional array.
            do
            {
                var cell = cells.Current;
                for (int i = 0; i < count; i++)
                {
                    offset[i] = Math.Min(offset[i], cell[i]);
                    max[i] = Math.Max(max[i], cell[i]);
                }
            } while (cells.MoveNext());

            // Create a multidimensional array of the right length.
            var indices = new int[count];
            for (int i = 0; i < count; i++)
            {
                indices[i] = max[i] - offset[i] + 1;
            }
            var array = Array.CreateInstance(typeof(int), indices);

            // Set the values of the multidimensional array.
            cells.Reset();
            while (cells.MoveNext())
            {
                var cell = cells.Current;
                for (int i = 0; i < count; i++)
                {
                    indices[i] = cell[i] - offset[i];
                }

                array.SetValue(heatmap[cell], indices);
            }

            return array;
        }

        /// <summary>Adds cells from a multidimensional array representation to the heatmap.</summary>
        /// <param name="heatmap">The <see cref="IHeatmap{TPoint, TVector}"/> interface instance.</param>
        /// <param name="multidimensionalArray">The multidimensional array representation of strength by cell.</param>
        /// <param name="offset">The offset of the multidimensional array.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the count of dimensions in the array is not equal to <typeparamref name="TPoint"/>'s Count.</exception>
        /// <exception cref="ArgumentException">Thrown when the array element type is not int.</exception>
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, Array multidimensionalArray, TPoint offset) where TPoint : IIndexable<int>
        {
#if NET8_0_OR_GREATER
            var count = TPoint.Count;
#else
            var count = offset.Count();
#endif

            if (count != multidimensionalArray.Rank)
            {
                throw new ArgumentOutOfRangeException("The array rank must be equal to the point length.", nameof(multidimensionalArray));
            }
            if (multidimensionalArray.GetType().GetElementType() != typeof(int))
            {
                throw new ArgumentException("The array element type must be int.", nameof(multidimensionalArray));
            }

            int dimension = multidimensionalArray.Rank;
            int[] indices = new int[multidimensionalArray.Rank];

            // List of actions to skip if dimension == 0 check inside for loop
            // TODO: Check if this is faster than if-checking inside the for loop
            var actions = new Action[multidimensionalArray.Rank];
            actions[0] = Add;
            for (int i = 1; i < multidimensionalArray.Rank; i++)
            {
                actions[i] = RecursiveAdd;
            }

            RecursiveAdd();

            void RecursiveAdd()
            {
                var length = multidimensionalArray.GetLength(--dimension);

                for (; indices[dimension] < length; indices[dimension]++)
                {
                    actions[dimension]();
                    offset[dimension]++;
                }

                offset[dimension] -= length;
                indices[dimension++] = 0;
            }

            void Add()
            {
                var strength = (int)multidimensionalArray.GetValue(indices)!;
                if (strength > 0)
                {
                    heatmap.Add(offset, strength);
                }
            }
        }
    }
}
