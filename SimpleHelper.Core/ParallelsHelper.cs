using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHelper.Core
{
    #region Intenal Class

    internal class InfinitePartitioner : Partitioner<bool>
    {
        public override IList<IEnumerator<bool>> GetPartitions(int partitionCount)
        {
            if (partitionCount < 1)
                throw new ArgumentOutOfRangeException(nameof(partitionCount));
            return (from i in Enumerable.Range(0, partitionCount)
                    select InfiniteEnumerator()).ToArray();
        }

        public override bool SupportsDynamicPartitions { get { return true; } }

        public override IEnumerable<bool> GetDynamicPartitions()
        {
            return new InfiniteEnumerators();
        }

        private static IEnumerator<bool> InfiniteEnumerator()
        {
            while (true) yield return true;
        }

        private class InfiniteEnumerators : IEnumerable<bool>
        {
            public IEnumerator<bool> GetEnumerator()
            {
                return InfiniteEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }

    #endregion

    public static class ParallelsHelper
    {
        /// <summary>
        /// Execute While Loop with parallelism 
        /// </summary>
        public static void While(Func<bool> condition, Action body)
        {
            Parallel.ForEach(IterateUntilFalse(condition), ignored => body());
        }

        /// <summary>
        /// Execute While Loop with parallelism.
        /// </summary>
        static IEnumerable<bool> IterateUntilFalse(Func<bool> condition)
        {
            while (condition()) yield return true;
        }

        /// <summary>
        /// Whiles the specified parallel options.
        /// </summary>
        public static void While(ParallelOptions parallelOptions, Func<bool> condition, Action<ParallelLoopState> body)
        {
            Parallel.ForEach(new InfinitePartitioner(), parallelOptions,
                (ignored, loopState) =>
                {
                    if (condition())
                        body(loopState);
                    else loopState.Stop();
                });
        }

        /// <summary>
        /// Exceute action in parallels
        /// </summary>
        public static void ExecuteParallel(params Action[] tasks)
        {
            // Initialize the reset events to keep track of completed threads
            ManualResetEvent[] resetEvents = new ManualResetEvent[tasks.Length];

            // Launch each method in it's own thread
            for (int i = 0; i < tasks.Length; i++)
            {
                resetEvents[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback((object index) =>
                {
                    int taskIndex = (int)index;

                    // Execute the method
                    tasks[taskIndex]();

                    // Tell the calling thread that we're done
                    resetEvents[taskIndex].Set();
                }), i);
            }

            // Wait for all threads to execute
            WaitHandle.WaitAll(resetEvents);
        }
    }
}
