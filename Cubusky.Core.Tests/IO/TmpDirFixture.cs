using System;
using System.IO;
using Xunit;

namespace Cubusky.Tests.IO
{
    public class TmpDirFixture : IDisposable
    {
        public static readonly string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "tmp");

        public TmpDirFixture()
        {
            Directory.CreateDirectory(Path);
        }

        public void Dispose()
        {
            Directory.Delete(Path, true);
            GC.SuppressFinalize(this);
        }
    }

    [CollectionDefinition(Name)]
    public class TmpDirCollection : ICollectionFixture<TmpDirFixture>
    {
        public const string Name = "TmpDir";
    }
}
