using System;
using System.Threading;

namespace SimpleHelper.Core
{
    /// <summary>
    /// Class for managing Semaphore
    /// </summary>
    public class SemaphoreHelper : IDisposable
    {
        readonly Mutex _mut;
        int _count;
        readonly int _max;
        readonly ManualResetEvent _latch;

        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreHelper"/> class.
        /// </summary>
        /// <param name="maxParallelThreadCount">The max parallel thread count</param>                
        /// <remarks></remarks>
        public SemaphoreHelper(int maxParallelThreadCount)
        {
            _mut = new Mutex();
            _count = maxParallelThreadCount;
            _max = maxParallelThreadCount;
            _latch = new ManualResetEvent(_count > 0);
        }

        /// <summary>
        /// Gets the ticket
        /// </summary>
        /// <remarks></remarks>
        public void GetTicket()
        {
            while (true)
            {
                _latch.WaitOne();
                _mut.WaitOne();
                if (_count > 0)
                {
                    _count--;
                    _mut.ReleaseMutex();
                    return;
                }
                _mut.ReleaseMutex();
            }
        }

        /// <summary>
        /// Releases the ticket
        /// </summary>
        /// <remarks></remarks>
        public void ReleaseTicket()
        {
            _mut.WaitOne();
            _count++;
            if (_count > _max)
            {
                _count = _max;
            }
            if (_count > 0)
            {
                _latch.Set();
            }
            _mut.ReleaseMutex();
        }

        /// <summary>
        /// Exécute les tâches définies par l'application associées à la libération ou à la redéfinition des ressources non managées.
        /// </summary>
        public void Dispose()
        {
            if (_latch != null)
            {
                _latch.Dispose();
            }

            if (_mut != null)
            {
                _mut.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the running thread count.
        /// </summary>
        /// <value>
        /// The running thread count.
        /// </value>
        public int RunningThreadCount
        {
            get
            {
                return _max - _count;
            }
        }
    }
}
