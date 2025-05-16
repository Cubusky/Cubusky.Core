using Cubusky.Numerics;
using Cubusky.Numerics.Json.Serialization;
using Cubusky.Text.Json;
using System;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cubusky.Heatmaps.Json.Serialization
{
    /// <summary>Default converter for <see cref="Heatmap3"/>.</summary>
    /// <remarks>
    /// Json example:
    /// {
    ///     "Matrix": [0.2971,0.4537,0.0191,0.8069,0.4482,0.1602,0.3329,0.4238,0.3899,0.9365,0.4164,0.2365,0.6978,0.9152,0.8771,0.086],
    ///     "Bounds": [-3, -3, -3, 7, 7, 7],
    ///     "Strengths": [1,-2,2,-2,3,-14,4,-2,5,-2,6,-14,7,-2,8,-2,9,-98,10,-2,11,-2,12,-14,13,-2,14,-2,15,-14,16,-2,17,-2,18,-98,19,-2,20,-2,21,-14,22,-2,23,-2,24,-14,25,-2,26,-2,27]
    /// }
    /// </remarks>
    public class Heatmap3JsonConverter : JsonConverter<Heatmap3>
    {
        private const string MatrixPropertyName = "Matrix";
        private const string BoundsPropertyName = "Bounds";
        private const string StrengthsPropertyName = "Strengths";

        private static readonly Matrix4x4JsonConverter matrix4x4JsonConverter = new Matrix4x4JsonConverter();
        private static readonly BoxJsonConverter boxJsonConverter = new BoxJsonConverter();

        /// <inheritdoc />
        public override Heatmap3? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read Start Object
            reader.ThrowIfNotTokenType(JsonTokenType.StartObject);

            reader.ReadOrThrow(MatrixPropertyName);
            var matrix = matrix4x4JsonConverter.Read(ref reader, typeof(Matrix4x4), options);

            reader.ReadOrThrow(BoundsPropertyName);
            var bounds = boxJsonConverter.Read(ref reader, typeof(Box), options);

            // Setup heatmap
            Heatmap3 heatmap;
            checked
            {
                try
                {
                    heatmap = new Heatmap3(bounds.Size.X * bounds.Size.Y * bounds.Size.Z, matrix);
                }
                catch (OverflowException)
                {
                    heatmap = new Heatmap3(int.MaxValue, matrix);
                }
            }

            // Read "Strengths" Array
            var offset = bounds.Position;
            var max = offset + bounds.Size;

            reader.ReadOrThrow(StrengthsPropertyName);
            reader.ThrowIfNotTokenType(JsonTokenType.StartArray);
            while (reader.Read(JsonTokenType.Number))
            {
                var strength = reader.GetInt32();

                if (strength < 0)
                {
                    offset.Z += Math.DivRem(-strength, bounds.Width * bounds.Height, out var remainder);
                    offset.Y += Math.DivRem(remainder, bounds.Width, out remainder);
                    offset.X += remainder;

                    continue;
                }

                heatmap.Add(offset, strength);

                if (++offset.X == max.X)
                {
                    offset.X -= bounds.Width;
                    if (++offset.Y == max.Y)
                    {
                        offset.Y -= bounds.Height;
                        offset.Z++;
                    }
                }
            }
            heatmap.TrimExcess();

            reader.ThrowIfNotTokenType(JsonTokenType.EndArray);
            reader.ReadOrThrow(JsonTokenType.EndObject);

            return heatmap;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Heatmap3 value, JsonSerializerOptions options)
        {
            // Calculate bounds
            var bounds = new Box(value.FirstOrDefault().Key, Point3.Zero);
            var orderedCells = value.OrderBy(strengthByCell =>
            {
                var cell = strengthByCell.Key;
                bounds = Box.Encapsulate(bounds, cell);
                return cell;
            }, new PointComparer()).ToArray(); // To Array is necessary to evaluate the query.
            bounds.Size += Point3.One;

            // Write Start Object
            writer.WriteStartObject();
            writer.WritePropertyName(MatrixPropertyName);
            matrix4x4JsonConverter.Write(writer, value.CellToPositionMatrix, options);
            writer.WritePropertyName(BoundsPropertyName);
            boxJsonConverter.Write(writer, bounds, options);

            // Write "Strengths" Array
            var offset = bounds.Position;

            writer.WritePropertyName(StrengthsPropertyName);
            writer.WriteStartArray();
            foreach (var (cell, strength) in orderedCells)
            {
                var next = (offset.Z - cell.Z) * bounds.Height * bounds.Width
                    + (offset.Y - cell.Y) * bounds.Width
                    + (offset.X - cell.X)
                    + 1;
                if (next < 0)
                {
                    writer.WriteNumberValue(next);
                }
                offset = cell;

                writer.WriteNumberValue(strength);
            }

            // Write End Array and End Object
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
