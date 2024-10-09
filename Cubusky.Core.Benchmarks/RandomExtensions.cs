using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;

namespace Cubusky.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [HideColumns("Job")]
    [HideColumns("Error", "StdDev", "RatioSD", "Alloc Ratio")]
    [MemoryDiagnoser]
    public class RandomExtensions
    {
        private readonly Random random = new Random();

        [Benchmark]
        public long NextInt64() => random.NextInt64();
    }
}
