using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleHelper.Core
{
    public static class LocalComputerHelper
    {
        /// <summary>
        /// Geteuids this instance.
        /// </summary>
        /// <returns></returns>
        [DllImport("libc")] private static extern uint geteuid();

        /// <summary>
        /// Gets the user temporary path.
        /// </summary>
        /// <returns></returns>
        public static string GetUserTempPath()
        {
            string path = Path.GetTempPath();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = Path.Combine(path, geteuid().ToString());
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
