using System;

namespace SimpleHelper.Core
{
    public static class RandomHelper
    {
        /// <summary>
        /// The getrandom
        /// </summary>
        static readonly Random getrandom = new Random();

        /// <summary>
        /// The synchronize lock
        /// </summary>
        static readonly object syncLock = new object();

        /// <summary>
        /// Gets the random number.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { 
                // synchronize
                return getrandom.Next(min, max);
            }
        }
    }
}
