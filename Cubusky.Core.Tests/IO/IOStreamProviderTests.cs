using Cubusky.IO;
using System.IO;
using Xunit;

namespace Cubusky.Tests.IO
{
    [Collection(TmpDirCollection.Name)]
    public class IOStreamProviderTests
    {
        private readonly TmpDirFixture tmpDir;

        public IOStreamProviderTests(TmpDirFixture tmpDir)
        {
            this.tmpDir = tmpDir;
        }

        public static readonly TheoryData<IIOStreamProvider> IOStreamProviders = new TheoryData<IIOStreamProvider>()
        {
            new FileIO(TmpDirFixture.Path + "/test.txt"),
        };

        [Theory]
        [MemberData(nameof(IOStreamProviders))]
        public void IIOStreamProvider_WriteExistsReadDelete(IIOStreamProvider io)
        {
            using (var inputStream = io.Write())
            {
                Assert.True(io.Exists());

                using var writer = new StreamWriter(inputStream);
                writer.Write("Hello, World!");
            }

            using (var outputStream = io.Read())
            {
                using var reader = new StreamReader(outputStream);
                Assert.Equal("Hello, World!", reader.ReadToEnd());
            }

            io.Delete();
            Assert.False(io.Exists());
        }
    }
}
