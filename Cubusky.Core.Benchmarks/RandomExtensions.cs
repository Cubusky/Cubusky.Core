using BenchmarkDotNet.Attributes;
using System;

namespace Cubusky.Benchmarks
{
    public class RandomExtensions
    {
        private readonly Random random = new Random();

        [Benchmark]
        public long NextInt64() => random.NextInt64();
    }
}
