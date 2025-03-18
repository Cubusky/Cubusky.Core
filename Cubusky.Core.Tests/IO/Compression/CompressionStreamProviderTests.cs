using Cubusky.IO.Compression;
using System.IO;
using System.Text;
using Xunit;

namespace Cubusky.Tests.IO.Compression
{
    public class CompressionStreamProviderTests
    {
        private const string Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        private static readonly byte[] MessageBytes = Encoding.UTF8.GetBytes(Message);

        public static readonly TheoryData<ICompressionStreamProvider> CompressionStreamProviders = new TheoryData<ICompressionStreamProvider>()
        {
            new BrotliCompression(),
            new DeflateCompression(),
            new GZipCompression(),
#if NET8_0_OR_GREATER
            new ZLibCompression(),
#endif
        };

        [Theory]
        [MemberData(nameof(CompressionStreamProviders))]
        public void ICompressionStreamProvider_CompressionStreamAndDecompressionStream(ICompressionStreamProvider compression)
        {
            using var memory = new MemoryStream();
            using (var compressionStream = compression.CompressionStream(memory, leaveOpen: true))
            {
                compressionStream.Write(MessageBytes, 0, MessageBytes.Length);
            }

            Assert.True(memory.Length < MessageBytes.Length);

            memory.Position = 0;
            using var decompressionStream = compression.DecompressionStream(memory);
            using var reader = new StreamReader(decompressionStream);

            Assert.Equal(Message, reader.ReadToEnd());
        }
    }
}
