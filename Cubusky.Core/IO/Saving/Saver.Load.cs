using Cubusky.IO.Compression;
using Cubusky.IO.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Cubusky.IO.Saving
{
    public static partial class Saver
    {
        [return: MaybeNull]
        private static TValue Load<TValue>(Stream ioStream, IStreamSerializer serializer)
        {
            using (ioStream)
            {
                return serializer.Deserialize<TValue>(ioStream);
            }
        }

        [return: MaybeNull]
        private static TValue Load<TValue>(Stream ioStream, IStreamSerializer serializer, ICompressionStreamProvider compression)
        {
            using (ioStream)
            {
                using var compressionStream = compression.DecompressionStream(ioStream);
                return serializer.Deserialize<TValue>(compressionStream);
            }
        }

#if NET8_0_OR_GREATER
        private static async ValueTask<TValue?> LoadAsync<TValue>(Stream ioStream, IAsyncStreamSerializer serializer, CancellationToken cancellationToken = default)
#else
        private static async ValueTask<TValue> LoadAsync<TValue>(Stream ioStream, IAsyncStreamSerializer serializer, CancellationToken cancellationToken = default)
#endif
        {
            await using (ioStream)
            {
                return await serializer.DeserializeAsync<TValue>(ioStream, cancellationToken);
            }
        }

#if NET8_0_OR_GREATER
        private static async ValueTask<TValue?> LoadAsync<TValue>(Stream ioStream, IAsyncStreamSerializer serializer, ICompressionStreamProvider compression, CancellationToken cancellationToken = default)
#else
        private static async ValueTask<TValue> LoadAsync<TValue>(Stream ioStream, IAsyncStreamSerializer serializer, ICompressionStreamProvider compression, CancellationToken cancellationToken = default)
#endif
        {
            await using (ioStream)
            {
                await using var compressionStream = compression.DecompressionStream(ioStream);
                return await serializer.DeserializeAsync<TValue>(compressionStream, cancellationToken);
            }
        }

        #region IIO
        /// <returns>The loaded value.</returns>
        /// <inheritdoc cref="LoadAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CancellationToken)"/>
        [return: MaybeNull]
        public static TValue Load<TValue>(this IIOStreamProvider io, IStreamSerializer serializer) => Load<TValue>(io.Read(), serializer);

        /// <returns>The loaded value.</returns>
        /// <inheritdoc cref="LoadAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CancellationToken)"/>
        [return: MaybeNull]
        public static TValue Load<TValue>(this IIOStreamProvider io, IStreamSerializer serializer, ICompressionStreamProvider compression) => Load<TValue>(io.Read(), serializer, compression);

        /// <inheritdoc cref="LoadAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CancellationToken)"/>
#if NET8_0_OR_GREATER
        public static ValueTask<TValue?>
#else
        public static ValueTask<TValue>
#endif
            LoadAsync<TValue>(this IIOStreamProvider io, IAsyncStreamSerializer serializer, CancellationToken cancellationToken = default)
            => LoadAsync<TValue>(io.Read(), serializer, cancellationToken);

        /// <summary>Loads a value from an input / output source using the specified deserializer and optional decompression.</summary>
        /// <typeparam name="TValue">The type of the value to load.</typeparam>
        /// <param name="io">The input / output to load from.</param>
        /// <param name="serializer">The deserializer to load with.</param>
        /// <param name="compression">The decompression to apply.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous load operation, which wraps the value that was loaded.</returns>
#if NET8_0_OR_GREATER
        public static ValueTask<TValue?>
#else
        public static ValueTask<TValue>
#endif
            LoadAsync<TValue>(this IIOStreamProvider io, IAsyncStreamSerializer serializer, ICompressionStreamProvider compression, CancellationToken cancellationToken = default)
            => LoadAsync<TValue>(io.Read(), serializer, compression, cancellationToken);
        #endregion
    }
}
