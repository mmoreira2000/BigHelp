using System;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BigHelp.Threading.PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Benchmark...");
            Summary result = BenchmarkRunner.Run<LockingBenchmark>();
            Console.WriteLine($"Results:\n{result}");

            Console.WriteLine("Benchmark completo.");
            Console.ReadKey();
        }
    }
}
