using BenchmarkDotNet.Running;

namespace Cubusky.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RandomExtensions>();
        }
    }
}
