using Cubusky.Numerics;
using Cubusky.Numerics.Json.Serialization;
using Cubusky.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json.Serialization;

#if !NET8_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace Cubusky.Heatmaps.Json.Serialization
{
    /// <summary>Default converter for <see cref="Heatmap3to2"/>.</summary>
    public partial class Heatmap3to2JsonConverter : JsonConverter<Heatmap3to2, Heatmap3to2JsonConverter.JsonObject>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public struct JsonObject
        {
            [JsonConverter(typeof(Matrix4x4JsonConverter))]
            public Matrix4x4 Matrix { get; set; }

            [JsonConverter(typeof(RectangleJsonConverter))]
            public Rectangle Bounds { get; set; }

            public int[] Strengths { get; set; }

            public readonly void Deconstruct(out Matrix4x4 matrix, out Rectangle bounds, out int[] strengths)
            {
                matrix = this.Matrix;
                bounds = this.Bounds;
                strengths = this.Strengths;
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#if NET8_0_OR_GREATER
        [JsonSerializable(typeof(JsonObject), GenerationMode = JsonSourceGenerationMode.Metadata)]
        internal partial class Heatmap3to2Context : JsonSerializerContext { }

        /// <summary>Initializes a new instance of the <see cref="Heatmap3JsonConverter"/> class.</summary>
        public Heatmap3to2JsonConverter() : base(new Heatmap3to2Context()) { }
#else
        /// <summary>Initializes a new instance of the <see cref="Heatmap2JsonConverter"/> class.</summary>
        public Heatmap3to2JsonConverter() : base(new DefaultJsonTypeInfoResolver()) { }
#endif

        /// <inheritdoc/>
        protected override Heatmap3to2 Revert(JsonObject value)
        {
            var (matrix, bounds, strengths) = value;
            var offset = bounds.Position;
            var max = offset + bounds.Size;

            var heatmap = new Heatmap3to2(matrix);
            for (var i = 0; i < strengths.Length; i++)
            {
                var strength = strengths[i];

                if (strength < 0)
                {
                    offset.Y += Math.DivRem(-strength, bounds.Width, out var remainder);
                    offset.X += remainder;

                    strength = strengths[++i];
                }

                heatmap.Add(offset, strength);

                if (++offset.X == max.X)
                {
                    offset.X -= bounds.Width;
                    offset.Y++;
                }
            }

            return heatmap;
        }

        /// <inheritdoc/>
        protected override JsonObject Convert(Heatmap3to2 value)
        {
            // Calculate bounds
            var bounds = new Rectangle(value.FirstOrDefault().Key, Point2.Zero);
            var orderedCells = value.OrderBy(strengthByCell =>
            {
                var cell = strengthByCell.Key;
                bounds = Rectangle.Encapsulate(bounds, cell);
                return cell;
            }, new PointComparer()).ToArray(); // To Array is necessary to evaluate the query.
            bounds.Size += Point2.One;

            // Order strengths
            var strengths = new List<int>(orderedCells.Length * 2);
            var offset = bounds.Position;
            foreach (var (cell, strength) in orderedCells)
            {
                var next = (offset.Y - cell.Y) * bounds.Width + (offset.X - cell.X) + 1;
                if (next < 0)
                {
                    strengths.Add(next);
                }
                offset = cell;

                strengths.Add(strength);
            }

            // Return JSON object
            return new JsonObject
            {
                Matrix = value.CellToPositionMatrix,
                Bounds = bounds,
                Strengths = strengths.ToArray()
            };
        }
    }
}
