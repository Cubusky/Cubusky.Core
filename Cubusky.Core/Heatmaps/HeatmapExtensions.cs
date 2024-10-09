using Cubusky.Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Cubusky.Heatmaps
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class HeatmapExtensions
    {
        public static int Strength<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap[heatmap.GetCell(position)];
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap.Add(heatmap.GetCell(position));
        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position, int strength) where TPoint : IIndexable<int> => heatmap.Add(heatmap.GetCell(position), strength);
        public static bool Contains<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap.Contains(heatmap.GetCell(position));
        public static bool Remove<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position) where TPoint : IIndexable<int> => heatmap.Remove(heatmap.GetCell(position));
        public static bool Remove<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, TVector position, int strength) where TPoint : IIndexable<int> => heatmap.Remove(heatmap.GetCell(position), strength);
        public static IEnumerable<TVector> GetPositions<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap) where TPoint : IIndexable<int> => heatmap.Select(heatmap.GetPosition);

        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, IHeatmap<TPoint, TVector> other) where TPoint : IIndexable<int>
        {
            foreach (var cell in other)
            {
                heatmap.Add(cell, other[cell]);
            }
        }

        public static void Remove<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, IHeatmap<TPoint, TVector> other) where TPoint : IIndexable<int>
        {
            foreach (var cell in other)
            {
                heatmap.Remove(cell, other[cell]);
            }
        }

        // TODO: Test memory allocation using ReadOnlySpan<int> instead of int[] to see if it's lower.
        public static int[] ToCompactArray<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap) where TPoint : IIndexable<int>
        {
            var cells = heatmap.GetEnumerator();
            if (!cells.MoveNext())
            {
                return Array.Empty<int>();
            }

#if NET8_0_OR_GREATER
            int[] array = new int[heatmap.Count * (TPoint.Count + 1)];
#else
            int[] array = new int[heatmap.Count * (cells.Current.Count() + 1)];
#endif

            int i = 0;
            do
            {
                foreach (var item in cells.Current)
                {
                    array[i++] = item;
                }
                array[i++] = heatmap[cells.Current];
            } while (cells.MoveNext());

            return array;
        }

        // TODO: Test performance using ReadOnlySpan<int> instead of int[] to see if it's faster.
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

#if NET8_0_OR_GREATER
        [RequiresDynamicCode("The code for an array of the specified type might not be available.")]
#endif
#if NET9_0_OR_GREATER
        //[Obsolete("This method requires dynamic code and is not AOT compatible. Use the ToMultidimensionalArray(Type, out TPoint) implementation instead.")]
#endif
        // TODO: In the XML comments remark that the loop has to be iterated twice.
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

#if NET9_0_OR_GREATER
        //public static Array ToMultidimensionalArray<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, Type arrayType, out TPoint offset) where TPoint : IIndexable<int>, new()
        //{
        //    throw new NotImplementedException();
        //}
#endif

        public static void Add<TPoint, TVector>(this IHeatmap<TPoint, TVector> heatmap, Array multidimensionalArray, TPoint offset) where TPoint : IIndexable<int>
        {
#if NET8_0_OR_GREATER
            var count = TPoint.Count;
#else
            var count = offset.Count();
#endif

            if (count != multidimensionalArray.Rank)
            {
                throw new ArgumentException("The array rank must be equal to the point length.", nameof(multidimensionalArray));
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

            void Add() => heatmap.Add(offset, (int)multidimensionalArray.GetValue(indices)!);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
