using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHelper
{

    /// <summary>
    /// Class InfinitePartitioner.
    /// Implements the <see cref="System.Collections.Concurrent.Partitioner{System.Boolean}" />
    /// </summary>
    /// <seealso cref="System.Collections.Concurrent.Partitioner{System.Boolean}" />
    internal class InfinitePartitioner : Partitioner<bool>
    {
        /// <summary>
        /// Partitionne la collection sous-jacente dans le nombre donné de partitions.
        /// </summary>
        /// <param name="partitionCount">Le nombre de partitions à créer.</param>
        /// <returns>Une liste contenant <paramref name="partitionCount" /> énumérateurs.</returns>
        /// <exception cref="ArgumentOutOfRangeException">partitionCount</exception>
        public override IList<IEnumerator<bool>> GetPartitions(int partitionCount)
        {
            if (partitionCount < 1)
                throw new ArgumentOutOfRangeException(nameof(partitionCount));
            return (from i in Enumerable.Range(0, partitionCount)
                    select InfiniteEnumerator()).ToArray();
        }

        /// <summary>
        /// Détermine si des partitions supplémentaires peuvent être créées dynamiquement.
        /// </summary>
        /// <value><c>true</c> if [supports dynamic partitions]; otherwise, <c>false</c>.</value>
        public override bool SupportsDynamicPartitions { get { return true; } }

        /// <summary>
        /// Crée un objet qui peut partitionner la collection sous-jacente dans un nombre variable de partitions.
        /// </summary>
        /// <returns>Objet qui peut créer des partitions sur la source de données sous-jacente.</returns>
        public override IEnumerable<bool> GetDynamicPartitions()
        {
            return new InfiniteEnumerators();
        }

        /// <summary>
        /// Infinites the enumerator.
        /// </summary>
        /// <returns>IEnumerator&lt;System.Boolean&gt;.</returns>
        private static IEnumerator<bool> InfiniteEnumerator()
        {
            while (true) yield return true;
        }

        /// <summary>
        /// Class InfiniteEnumerators.
        /// Implements the <see cref="System.Collections.Generic.IEnumerable{System.Boolean}" />
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IEnumerable{System.Boolean}" />
        private class InfiniteEnumerators : IEnumerable<bool>
        {
            /// <summary>
            /// Retourne un énumérateur qui itère au sein de la collection.
            /// </summary>
            /// <returns>Énumérateur permettant d'effectuer une itération au sein de la collection.</returns>
            public IEnumerator<bool> GetEnumerator()
            {
                return InfiniteEnumerator();
            }
            /// <summary>
            /// Retourne un énumérateur qui itère au sein d'une collection.
            /// </summary>
            /// <returns>Objet <see cref="T:System.Collections.IEnumerator" /> pouvant être utilisé pour itérer au sein de la collection.</returns>
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }
    }

    /// <summary>
    /// Class ParallelsHelper.
    /// </summary>
    public static class ParallelsHelper
    {
        /// <summary>
        /// Execute While Loop with parallelism
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="body">The body.</param>
        public static void While(Func<bool> condition, Action body)
        {
            Parallel.ForEach(IterateUntilFalse(condition), ignored => body());
        }

        /// <summary>
        /// Execute While Loop with parallelism.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>IEnumerable&lt;System.Boolean&gt;.</returns>
        static IEnumerable<bool> IterateUntilFalse(Func<bool> condition)
        {
            while (condition()) yield return true;
        }

        /// <summary>
        /// Whiles the specified parallel options.
        /// </summary>
        /// <param name="parallelOptions">The parallel options.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="body">The body.</param>
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
        /// <param name="tasks">The tasks.</param>
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
