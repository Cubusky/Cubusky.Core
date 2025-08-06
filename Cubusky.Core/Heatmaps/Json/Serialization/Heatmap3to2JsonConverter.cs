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
    /// <summary>Default converter for <see cref="Heatmap3to2"/>.</summary>
    /// <remarks>
    /// Json example:
    /// {
    ///     "Matrix": [0.2971,0.4537,0.0191,0.8069,0.4482,0.1602,0.3329,0.4238,0.3899,0.9365,0.4164,0.2365,0.6978,0.9152,0.8771,0.086],
    ///     "Swizzle": 0,
    ///     "Bounds": [-3,-3,7,7],
    ///     "Strengths": [7,-2,6,-2,8,-14,5,-5,2,-14,9,-2,3,-2,4]
    /// }
    /// </remarks>
    public class Heatmap3to2JsonConverter : JsonConverter<Heatmap3to2>
    {
        private const string MatrixPropertyName = "Matrix";
        private const string SwizzlePropertyName = "Swizzle";
        private const string BoundsPropertyName = "Bounds";
        private const string StrengthsPropertyName = "Strengths";

        private static readonly Matrix4x4JsonConverter matrix4x4JsonConverter = new Matrix4x4JsonConverter();
        private static readonly RectangleJsonConverter rectangleJsonConverter = new RectangleJsonConverter();

        /// <inheritdoc />
        public override Heatmap3to2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read Start Object
            reader.ThrowIfNotTokenType(JsonTokenType.StartObject);

            reader.ReadOrThrow(MatrixPropertyName);
            var matrix = matrix4x4JsonConverter.Read(ref reader, typeof(Matrix4x4), options);

            reader.ReadOrThrow(SwizzlePropertyName);
            reader.ThrowIfNotTokenType(JsonTokenType.Number);
            var swizzle = (Swizzle3to2)reader.GetByte();

            reader.ReadOrThrow(BoundsPropertyName);
            var bounds = rectangleJsonConverter.Read(ref reader, typeof(Rectangle), options);

            // Setup heatmap
            Heatmap3to2 heatmap;
            checked
            {
                try
                {
                    heatmap = new Heatmap3to2(bounds.Size.X * bounds.Size.Y, matrix, swizzle);
                }
                catch (OverflowException)
                {
                    heatmap = new Heatmap3to2(int.MaxValue, matrix, swizzle);
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
                    offset.Y += Math.DivRem(-strength + offset.X - bounds.X, bounds.Width, out var remainder);
                    offset.X = remainder + bounds.X;

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
        public override void Write(Utf8JsonWriter writer, Heatmap3to2 value, JsonSerializerOptions options)
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
            matrix4x4JsonConverter.Write(writer, value.CellToPositionMatrix, options);
            writer.WriteNumber(SwizzlePropertyName, (int)value.Swizzle);
            writer.WritePropertyName(BoundsPropertyName);
            rectangleJsonConverter.Write(writer, bounds, options);

            // Write "Strengths" Array
            var offset = bounds.Position;

            writer.WritePropertyName(StrengthsPropertyName);
            writer.WriteStartArray();
            foreach (var (cell, strength) in orderedCells)
            {
                var next = (offset.Y - cell.Y) * bounds.Width + (offset.X - cell.X);
                if (next < 0)
                {
                    writer.WriteNumberValue(next);
                }
                writer.WriteNumberValue(strength);

                offset = cell;
                offset.X++;
            }

            // Write End Array and End Object
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
