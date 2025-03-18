using Cubusky.IO;
using Cubusky.IO.Compression;
using Cubusky.IO.Saving;
using Cubusky.IO.Serialization;
using Cubusky.Tests.IO.Serialization;
using System.IO;
using Xunit;

namespace Cubusky.Tests.IO.Saving
{
    [Collection(TmpDirCollection.Name)]
    public class SaverTests
    {
        private readonly TmpDirFixture tmpDir;

        public SaverTests(TmpDirFixture tmpDir)
        {
            this.tmpDir = tmpDir;
        }

        public static readonly FileIO FileIO = new FileIO(TmpDirFixture.Path + "/test.txt");

        public static readonly TheoryData<IIOStreamProvider, IStreamSerializer, ICompressionStreamProvider> SaverData = new TheoryData<IIOStreamProvider, IStreamSerializer, ICompressionStreamProvider>()
        {
            { FileIO, new JsonSerializer(), new BrotliCompression() },
            { FileIO, new JsonSerializer(), new DeflateCompression() },
            { FileIO, new JsonSerializer(), new GZipCompression() },
#if NET8_0_OR_GREATER
            { FileIO, new JsonSerializer(), new ZLibCompression() },
#endif
        };

        [Theory]
        [MemberData(nameof(SaverData))]
        public void Saver_Save(IIOStreamProvider io, IStreamSerializer serializer, ICompressionStreamProvider _)
        {
            io.Delete();
            io.Save(serializer, Person.JohnDoe);

            using var reader = new StreamReader(io.Read());
            Assert.Equal(Person.Json, reader.ReadToEnd());
        }

        [Theory]
        [MemberData(nameof(SaverData))]
        public void Saver_Load(IIOStreamProvider io, IStreamSerializer serializer, ICompressionStreamProvider _)
        {
            io.Delete();
            using (var writer = new StreamWriter(io.Write()))
            {
                writer.Write(Person.Json);
            }

            var person = io.Load<Person>(serializer);
            Assert.Equal(Person.JohnDoe, person);
        }

        [Theory]
        [MemberData(nameof(SaverData))]
        public void Saver_SaveCompress(IIOStreamProvider io, IStreamSerializer serializer, ICompressionStreamProvider compression)
        {
            io.Delete();
            io.Save(serializer, compression, Person.JohnDoe);

            using var decompressed = compression.DecompressionStream(io.Read());
            using var reader = new StreamReader(decompressed);
            Assert.Equal(Person.Json, reader.ReadToEnd());
        }

        [Theory]
        [MemberData(nameof(SaverData))]
        public void Saver_LoadDecompress(IIOStreamProvider io, IStreamSerializer serializer, ICompressionStreamProvider compression)
        {
            io.Delete();
            using (var compressed = compression.CompressionStream(io.Write()))
            {
                using var writer = new StreamWriter(compressed);
                writer.Write(Person.Json);
            }

            var person = io.Load<Person>(serializer, compression);
            Assert.Equal(Person.JohnDoe, person);
        }
    }
}
