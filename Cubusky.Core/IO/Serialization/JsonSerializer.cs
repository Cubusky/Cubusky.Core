using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Json = System.Text.Json.JsonSerializer;

namespace Cubusky.IO.Serialization
{
    /// <summary>Provides functionality to serialize from- and deserialize to objects or value types using the <see cref="System.Text.Json.JsonSerializer"/>.</summary>
    public class JsonSerializer : IStreamSerializer, IAsyncStreamSerializer
    {
        private static class DynamicCodeSuppress
        {
            public static class IL2026
            {
                public const string Category = "Trimming";
                public const string CheckId = "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code";
            }

            public static class IL3050
            {
                public const string Category = "AOT";
                public const string CheckId = "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.";
            }

            public const string Justification = "Members annotated with the 'RequiresUnreferencedCodeAttribute' and 'RequiresDynamicCodeAttribute' will not be called because the initialization steps required for these members are already decorated with these attributes.";
        }

        private readonly JsonTypeInfo? jsonTypeInfo;
        private readonly JsonSerializerOptions? options;
        private readonly JsonSerializerContext? context;

        /// <summary>Initializes a new instance of the <see cref="JsonSerializer"/> class.</summary>
        /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
        public JsonSerializer(JsonTypeInfo jsonTypeInfo)
        {
            this.jsonTypeInfo = jsonTypeInfo;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonSerializer"/> class.</summary>
        /// <param name="options">Options to control serialization behavior.</param>
#if NET8_0_OR_GREATER
        [RequiresUnreferencedCode("Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
        [RequiresDynamicCode("Use System.Text.Json source generation for native AOT applications.")]
#endif
        public JsonSerializer(JsonSerializerOptions? options = null)
        {
            this.options = options;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonSerializer"/> class.</summary>
        /// <param name="context">A metadata provider for serializable types.</param>
        public JsonSerializer(JsonSerializerContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        [SuppressMessage(DynamicCodeSuppress.IL2026.Category, DynamicCodeSuppress.IL2026.CheckId, Justification = DynamicCodeSuppress.Justification)]
        [SuppressMessage(DynamicCodeSuppress.IL3050.Category, DynamicCodeSuppress.IL3050.CheckId, Justification = DynamicCodeSuppress.Justification)]
        public void Serialize(Stream stream, object? value, Type inputType)
        {
            if (jsonTypeInfo != null)
            {
                Json.Serialize(stream, value, jsonTypeInfo);
            }
            else if (context != null)
            {
                Json.Serialize(stream, value, inputType, context);
            }
            else
            {
                Json.Serialize(stream, value, inputType, options);
            }
        }

        /// <inheritdoc />
        [SuppressMessage(DynamicCodeSuppress.IL2026.Category, DynamicCodeSuppress.IL2026.CheckId, Justification = DynamicCodeSuppress.Justification)]
        [SuppressMessage(DynamicCodeSuppress.IL3050.Category, DynamicCodeSuppress.IL3050.CheckId, Justification = DynamicCodeSuppress.Justification)]
        public Task SerializeAsync(Stream stream, object? value, Type inputType, CancellationToken cancellationToken = default)
        {
            if (jsonTypeInfo != null)
            {
                return Json.SerializeAsync(stream, value, jsonTypeInfo, cancellationToken);
            }
            else if (context != null)
            {
                return Json.SerializeAsync(stream, value, inputType, context, cancellationToken);
            }
            else
            {
                return Json.SerializeAsync(stream, value, inputType, options, cancellationToken);
            }
        }

        /// <inheritdoc />
        [SuppressMessage(DynamicCodeSuppress.IL2026.Category, DynamicCodeSuppress.IL2026.CheckId, Justification = DynamicCodeSuppress.Justification)]
        [SuppressMessage(DynamicCodeSuppress.IL3050.Category, DynamicCodeSuppress.IL3050.CheckId, Justification = DynamicCodeSuppress.Justification)]
        public object? Deserialize(Stream stream, Type returnType)
        {
            if (jsonTypeInfo != null)
            {
                return Json.Deserialize(stream, jsonTypeInfo);
            }
            else if (context != null)
            {
                return Json.Deserialize(stream, returnType, context);
            }
            else
            {
                return Json.Deserialize(stream, returnType, options);
            }
        }

        /// <inheritdoc />
        [SuppressMessage(DynamicCodeSuppress.IL2026.Category, DynamicCodeSuppress.IL2026.CheckId, Justification = DynamicCodeSuppress.Justification)]
        [SuppressMessage(DynamicCodeSuppress.IL3050.Category, DynamicCodeSuppress.IL3050.CheckId, Justification = DynamicCodeSuppress.Justification)]
        public ValueTask<object?> DeserializeAsync(Stream stream, Type returnType, CancellationToken cancellationToken = default)
        {
            if (jsonTypeInfo != null)
            {
                return Json.DeserializeAsync(stream, jsonTypeInfo, cancellationToken);
            }
            else if (context != null)
            {
                return Json.DeserializeAsync(stream, returnType, context, cancellationToken);
            }
            else
            {
                return Json.DeserializeAsync(stream, returnType, options, cancellationToken);
            }
        }
    }
}
