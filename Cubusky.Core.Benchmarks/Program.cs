using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

namespace Cubusky.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
            => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                .Run(args, DefaultConfig.Instance
                    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core31))
                    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core80).AsBaseline())
                    .HideColumns("Job, Error", "StdDev", "RatioSD", "Alloc Ratio")
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Declared))
                );
    }
}
