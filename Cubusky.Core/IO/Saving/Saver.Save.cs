using Cubusky.IO.Compression;
using Cubusky.IO.Serialization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Cubusky.IO.Saving
{
    /// <summary>Provides a collection of static methods for saving and loading to a <see cref="IIOStreamProvider"/> with a <see cref="IStreamSerializer"/> and an optional <see cref="ICompressionStreamProvider"/>.</summary>
    public static partial class Saver
    {
        private static void Save<TValue>(Stream ioStream, IStreamSerializer serializer, TValue value)
        {
            using (ioStream)
            {
                serializer.Serialize(ioStream, value);
            }
        }

        private static void Save<TValue>(Stream ioStream, IStreamSerializer serializer, TValue value, ICompressionStreamProvider compression, CompressionLevel compressionLevel = default)
        {
            using (ioStream)
            {
                using var compressionStream = compression.CompressionStream(ioStream, compressionLevel);
                serializer.Serialize(compressionStream, value);
            }
        }

        private static async Task SaveAsync<TValue>(Stream ioStream, IAsyncStreamSerializer serializer, TValue value, CancellationToken cancellationToken = default)
        {
            await using (ioStream)
            {
                await serializer.SerializeAsync(ioStream, value, cancellationToken);
            }
        }

        private static async Task SaveAsync<TValue>(Stream ioStream, IAsyncStreamSerializer serializer, TValue value, ICompressionStreamProvider compression, CompressionLevel compressionLevel = default, CancellationToken cancellationToken = default)
        {
            await using (ioStream)
            {
                await using var compressionStream = compression.CompressionStream(ioStream, compressionLevel);
                await serializer.SerializeAsync(compressionStream, value, cancellationToken);
            }
        }

        #region IIO
        /// <returns></returns>
        /// <inheritdoc cref="SaveAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CompressionLevel, TValue, CancellationToken)"/>
        public static void Save<TValue>(this IIOStreamProvider io, IStreamSerializer serializer, TValue value)
            => Save(io.Write(), serializer, value);

        /// <returns></returns>
        /// <inheritdoc cref="SaveAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CompressionLevel, TValue, CancellationToken)"/>
        public static void Save<TValue>(this IIOStreamProvider io, IStreamSerializer serializer, ICompressionStreamProvider compression, TValue value)
            => Save(io.Write(), serializer, value, compression);

        /// <returns></returns>
        /// <inheritdoc cref="SaveAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CompressionLevel, TValue, CancellationToken)"/>
        public static void Save<TValue>(this IIOStreamProvider io, IStreamSerializer serializer, ICompressionStreamProvider compression, CompressionLevel compressionLevel, TValue value)
            => Save(io.Write(), serializer, value, compression, compressionLevel);

        /// <inheritdoc cref="SaveAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CompressionLevel, TValue, CancellationToken)"/>
        public static Task SaveAsync<TValue>(this IIOStreamProvider io, IAsyncStreamSerializer serializer, TValue value, CancellationToken cancellationToken = default)
            => SaveAsync(io.Write(), serializer, value, cancellationToken: cancellationToken);

        /// <inheritdoc cref="SaveAsync{TValue}(IIOStreamProvider, IAsyncStreamSerializer, ICompressionStreamProvider, CompressionLevel, TValue, CancellationToken)"/>
        public static Task SaveAsync<TValue>(this IIOStreamProvider io, IAsyncStreamSerializer serializer, ICompressionStreamProvider compression, TValue value, CancellationToken cancellationToken = default)
            => SaveAsync(io.Write(), serializer, value, compression, cancellationToken: cancellationToken);

        /// <summary>Saves the specified value to an input / output source using the specified serializer and optional compression.</summary>
        /// <typeparam name="TValue">The type of the value to save.</typeparam>
        /// <param name="io">The input / output to save to.</param>
        /// <param name="serializer">The serializer to save with.</param>
        /// <param name="compression">The compression to apply.</param>
        /// <param name="compressionLevel">The level of compression to apply.</param>
        /// <param name="value">The value to save.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public static Task SaveAsync<TValue>(this IIOStreamProvider io, IAsyncStreamSerializer serializer, ICompressionStreamProvider compression, CompressionLevel compressionLevel, TValue value, CancellationToken cancellationToken = default)
            => SaveAsync(io.Write(), serializer, value, compression, compressionLevel, cancellationToken: cancellationToken);
        #endregion
    }
}
