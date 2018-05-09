using System;
using System.Diagnostics;

namespace TwitchNet.Debugger
{
    internal class
    Benchmark
    {
        private Stopwatch watch;

        public double elapsed_seconds
        {
            get
            {
                return elapsed_milliseconds / 1_000;
            }
        }

        public double elapsed_milliseconds
        {
            get
            {
                return elapsed_microseconds / 1_000;
            }
        }

        public double elapsed_microseconds
        {
            get
            {
                return 1_000_000 * (double)watch.ElapsedTicks / Stopwatch.Frequency;
            }
        }

        public long elapsed_ticks
        {
            get
            {
                return watch.ElapsedTicks;
            }
        }

        internal Benchmark()
        {
            watch = new Stopwatch();
        }

        internal void Start()
        {
            watch.Start();
        }

        internal void
        Stop()
        {
            watch.Stop();
        }

        internal void
        Reset()
        {
            watch.Reset();
        }

        internal void Print()
        {
            Debug.WriteLine(ConsoleColor.Cyan, "[ Benchmarch Results ]");
            Debug.WriteLine("Elapsed seconds:       " + elapsed_seconds + " s");
            Debug.WriteLine("Elapsed milliseconds:  " + elapsed_milliseconds + " ms");
            Debug.WriteLine("Elapsed microseconds:  " + elapsed_microseconds + " µs");
            Debug.WriteLine("Elapsed ticks:         " + elapsed_ticks);
        }
    }
}