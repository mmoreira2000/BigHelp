using System;
using System.Collections.Generic;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace BigHelp.Threading.PerformanceTests
{
    [ClrJob(baseline: true), CoreJob, CoreRtJob]
    [RPlotExporter, RankColumn]
    public class LockingBenchmark
    {
        [Params(1, 10, 100, 1000)]
        public int N;

        private List<int> _listaDynamicLock;
        private List<int> _listaStaticLock;

        public static object StaticLock = new object();
        public static DynamicLocks DynamicLocks = new DynamicLocks();

        [GlobalSetup]
        public void Setup()
        {
            _listaDynamicLock = new List<int>();
            _listaStaticLock = new List<int>();
        }

        [Benchmark(Baseline = true)]
        public void RunStaticLock()
        {
            var t1 = new Thread(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    lock (StaticLock)
                    {
                        _listaStaticLock.Add(i);
                    }
                }
            });
            var t2 = new Thread(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    lock (StaticLock)
                    {
                        _listaStaticLock.Add(i);
                    }
                }
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }

        [Benchmark]
        public void RunDynamicLock()
        {
            var t1 = new Thread(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    lock (DynamicLocks.AquireLock("lockMe"))
                    {
                        _listaDynamicLock.Add(i);
                    }
                }
            });
            var t2 = new Thread(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    lock (DynamicLocks.AquireLock("lockMe"))
                    {
                        _listaDynamicLock.Add(i);
                    }
                }
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }



    }
}