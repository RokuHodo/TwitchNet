// standard namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace
TwitchNet.Debugger
{    
    public class
    Benchmark
    {
        private bool running;

        private List<BenchmarkRun> runs;                
        
        public Benchmark()
        {
            runs = new List<BenchmarkRun>();
        }

        public void
        Add(BenchmarkRun run)
        {
            runs.Add(run);
        }

        public void
        Execute()
        {
            if (running)
            {
                return;
            }

            foreach(BenchmarkRun run in runs)
            {
                run.Execute();
            }
        }
    }

    public struct
    BenchmarkRun
    {
        private bool running;

        private Stopwatch watch;

        public string name;

        public ulong iterations;

        public Action action;

        public double total_seconds;
        public double total_milliseconds;
        public double total_microseconds;
        public double total_ticks;

        public double average_seconds;
        public double average_milliseconds;
        public double average_microseconds;
        public double average_ticks;        

        public void
        Execute()
        {
            if (running)
            {
                return;
            }

            watch = new Stopwatch();
            watch.Start();
            for (ulong count = 0; count < iterations; ++count)
            {
                action();
            }
            watch.Stop();
            CalculateResults();
            Print();
            Reset();
        }

        private void
        Reset()
        {
            if (running)
            {
                return;
            }

            total_seconds = 0;
            total_milliseconds = 0;
            total_microseconds = 0;
            total_ticks = 0;

            average_seconds = 0;
            average_milliseconds = 0;
            average_microseconds = 0;
            average_ticks = 0;
        }

        private void
        CalculateResults()
        {
            total_ticks = watch.ElapsedTicks;
            total_microseconds = 1_000_000 * (double)watch.ElapsedTicks / Stopwatch.Frequency;
            total_milliseconds = total_microseconds / 1_000;
            total_seconds = total_milliseconds / 1_000;

            average_ticks = total_ticks / iterations;
            average_microseconds = total_microseconds / iterations;
            average_milliseconds = total_milliseconds / iterations;
            average_seconds = total_seconds / iterations;
        }

        private void
        Print()
        {
            Debug.WriteLine(ConsoleColor.Cyan, "[ Benchmarck Results ]");
            Debug.WriteLine("Benchmark:            " + name);
            Debug.WriteLine();

            Debug.WriteLine("Iterations:           " + string.Format("{0:n0}", iterations));
            Debug.WriteLine();

            Debug.WriteLine("Total seconds:        " + total_seconds + " s");
            Debug.WriteLine("Total milliseconds:   " + total_milliseconds+ " ms");
            Debug.WriteLine("Total microseconds:   " + total_microseconds + " µs");
            Debug.WriteLine("Total ticks:          " + total_ticks);
            Debug.WriteLine();

            Debug.WriteLine("Average seconds:      " + average_seconds + " s");
            Debug.WriteLine("Average milliseconds: " + average_milliseconds + " ms");
            Debug.WriteLine("Average microseconds: " + average_microseconds + " µs");
            Debug.WriteLine("Average ticks:        " + average_ticks);
            Debug.WriteLine();
        }
    }
}