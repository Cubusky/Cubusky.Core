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
    /// <summary>Default converter for <see cref="Heatmap2"/>.</summary>
    /// <remarks>
    /// Json example:
    /// {
    ///     "Matrix": [11, 12, 21, 22, 31, 32],
    ///     "Bounds": [-3, -3, 7, 7],
    ///     "Strengths": [7, -2, 6, -2, 8, -14, 5, -5, 2, -14, 9, -2, 3, -2, 4]
    /// }
    /// </remarks>
    public class Heatmap2JsonConverter : JsonConverter<Heatmap2>
    {
        private const string MatrixPropertyName = "Matrix";
        private const string BoundsPropertyName = "Bounds";
        private const string StrengthsPropertyName = "Strengths";

        private static readonly Matrix3x2JsonConverter matrix3x2JsonConverter = new Matrix3x2JsonConverter();
        private static readonly RectangleJsonConverter rectangleJsonConverter = new RectangleJsonConverter();

        /// <inheritdoc />
        public override Heatmap2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read Start Object
            reader.ThrowIfNotTokenType(JsonTokenType.StartObject);

            reader.ReadOrThrow(MatrixPropertyName);
            var matrix = matrix3x2JsonConverter.Read(ref reader, typeof(Matrix3x2), options);

            reader.ReadOrThrow(BoundsPropertyName);
            var bounds = rectangleJsonConverter.Read(ref reader, typeof(Rectangle), options);

            // Setup heatmap
            Heatmap2 heatmap;
            checked
            {
                try
                {
                    heatmap = new Heatmap2(bounds.Size.X * bounds.Size.Y, matrix);
                }
                catch (OverflowException)
                {
                    heatmap = new Heatmap2(int.MaxValue, matrix);
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
                    offset.Y += Math.DivRem(-strength, bounds.Width, out var remainder);
                    offset.X += remainder;

                    continue;
                }

                heatmap.Add(offset, strength);

                if (++offset.X == max.X)
                {
                    offset.X -= bounds.Width;
                    offset.Y++;
                }
            }
            heatmap.TrimExcess();

            reader.ThrowIfNotTokenType(JsonTokenType.EndArray);
            reader.ReadOrThrow(JsonTokenType.EndObject);

            return heatmap;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Heatmap2 value, JsonSerializerOptions options)
        {
            // Calculate bounds
            var bounds = new Rectangle(value.FirstOrDefault().Item, Point2.Zero);
            var orderedCells = value.OrderBy(strengthByCell =>
            {
                var cell = strengthByCell.Item;
                bounds = Rectangle.Encapsulate(bounds, cell);
                return cell;
            }, new PointComparer()).ToArray(); // To Array is necessary to evaluate the query.
            bounds.Size += Point2.One;

            // Write Start Object
            writer.WriteStartObject();
            writer.WritePropertyName(MatrixPropertyName);
            matrix3x2JsonConverter.Write(writer, value.CellToPositionMatrix, options);
            writer.WritePropertyName(BoundsPropertyName);
            rectangleJsonConverter.Write(writer, bounds, options);

            // Write "Strengths" Array
            var offset = bounds.Position;

            writer.WritePropertyName(StrengthsPropertyName);
            writer.WriteStartArray();
            foreach (var (cell, strength) in orderedCells)
            {
                var next = (offset.Y - cell.Y) * bounds.Width + (offset.X - cell.X) + 1;
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
