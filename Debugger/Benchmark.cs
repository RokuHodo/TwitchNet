// standard namespaces
using System;
using System.Diagnostics;

namespace
TwitchNet.Debugger
{    
    internal class
    Benchmark
    {
        private Stopwatch watch;

        /// <summary>
        /// The elapsed time in seconds.
        /// </summary>
        public double elapsed_seconds
        {
            get
            {
                return elapsed_milliseconds / 1_000;
            }
        }

        /// <summary>
        /// The elapsed time in milliseconds.
        /// </summary>
        public double elapsed_milliseconds
        {
            get
            {
                return elapsed_microseconds / 1_000;
            }
        }

        /// <summary>
        /// The elapsed time in micro seconds.
        /// </summary>
        public double elapsed_microseconds
        {
            get
            {
                return 1_000_000 * (double)watch.ElapsedTicks / Stopwatch.Frequency;
            }
        }

        /// <summary>
        /// The elapsed ticks.
        /// </summary>
        public long elapsed_ticks
        {
            get
            {
                return watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Benchmark"/> class.
        /// </summary>
        public Benchmark()
        {
            watch = new Stopwatch();
        }

        /// <summary>
        /// Starts the benchmark.
        /// </summary>
        [Conditional("DEBUG")]
        public void Start()
        {
            watch.Start();
        }

        /// <summary>
        /// Stops the benchmark.
        /// </summary>
        [Conditional("DEBUG")]
        public void
        Stop()
        {
            watch.Stop();
        }

        /// <summary>
        /// Resets the benchmark.
        /// </summary>
        [Conditional("DEBUG")]
        public void
        Reset()
        {
            watch.Reset();
        }

        /// <summary>
        /// Prints the benchamrk results.
        /// </summary>
        [Conditional("DEBUG")]
        public void
        Print()
        {
            Debug.WriteLine(ConsoleColor.Cyan, "[ Benchmarch Results ]");
            Debug.WriteLine("Elapsed seconds:       " + elapsed_seconds + " s");
            Debug.WriteLine("Elapsed milliseconds:  " + elapsed_milliseconds + " ms");
            Debug.WriteLine("Elapsed microseconds:  " + elapsed_microseconds + " µs");
            Debug.WriteLine("Elapsed ticks:         " + elapsed_ticks);
        }
    }
}