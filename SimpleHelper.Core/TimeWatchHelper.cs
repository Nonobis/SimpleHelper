using System;
using System.Diagnostics;

namespace SimpleHelper.Core
{
    public static class TimeWatchHelper
    {
        /// <summary>
        /// The Stopwatch
        /// </summary>
        static Stopwatch sw;

        /// <summary>
        /// Gets the elapsed time.
        /// </summary>
        /// <value>The elapsed time.</value>
        public static TimeSpan ElapsedTime
        {
            get
            {
                if (sw != null)
                    return sw.Elapsed;
                return new TimeSpan(-1);
            }
        }

        /// <summary>
        ///   Gets the elapsed time as formatted string
        /// </summary>
        /// <returns></returns>
        public static string ElapsedTimeFormatted
        {
            get
            {
                if (sw != null)
                    return
                        $"{sw.Elapsed.Hours:D2}h:{sw.Elapsed.Minutes:D2}m:{sw.Elapsed.Seconds:D2}s:{sw.Elapsed.Milliseconds:D3}ms";

                return "";
            }
        }


        /// <summary>
        /// Starts this instance.
        /// </summary>
        public static void Start()
        {
            sw = Stopwatch.StartNew();
        }

        /// <summary>
        ///  Restartwatches this instance.
        /// </summary>
        public static void Restart()
        {
            sw.Restart();
        }

        /// <summary>
        /// Stopwatches this instance.
        /// </summary>
        public static void Stop()
        {
            if (sw != null && sw.IsRunning)
                sw.Stop();
            sw = null;
        }
    }
}
