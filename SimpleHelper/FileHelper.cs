using System;
using System.IO;
using System.Threading.Tasks;

namespace SimpleHelper
{

    /// <summary>
    /// Class FileHelper.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Copies the files recursively.
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pTarget">The target.</param>
        /// <param name="pDeleteExistingTarget">if set to <c>true</c> [p delete existing target].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CopyFilesRecursively(string pSource, string pTarget, bool pDeleteExistingTarget)
        {
            return CopyFilesRecursively(new DirectoryInfo(pSource), new DirectoryInfo(pTarget), pDeleteExistingTarget);
        }

        /// <summary>
        /// Copies the files recursively.
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pTarget">The target.</param>
        /// <param name="pDeleteExistingTarget">if set to <c>true</c> [p delete existing target].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CopyFilesRecursively(DirectoryInfo pSource, DirectoryInfo pTarget, bool pDeleteExistingTarget)
        {
            try
            {
                foreach (var dir in pSource.GetDirectories())
                {
                    var targetSubDirectory = pTarget.CreateSubdirectory(dir.Name);
                    CopyFilesRecursively(dir, targetSubDirectory, pDeleteExistingTarget);
                }

                Parallel.ForEach(pSource.GetFiles(),
                filePath =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (File.Exists(Path.Combine(pTarget.FullName, filePath.Name)))
                            File.Delete(Path.Combine(pTarget.FullName, filePath.Name));

                        filePath.CopyTo(Path.Combine(pTarget.FullName, filePath.Name));
                    });
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts to base64.
        /// </summary>
        /// <param name="pFilePath">The p file path.</param>
        /// <returns>System.String.</returns>
        public static string FileToBase64(string pFilePath)
        {
            string sValue = string.Empty;
            var oFile = new FileInfo(pFilePath);
            if (!oFile.Exists)
                return sValue;

            FileStream fs = new FileStream(oFile.FullName,
                FileMode.Open,
                FileAccess.Read);
            byte[] filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            sValue =
                Convert.ToBase64String(filebytes,
                    Base64FormattingOptions.InsertLineBreaks);
            return sValue;
        }

        /// <summary>
        /// Converts to file.
        /// </summary>
        /// <param name="pData">The p data.</param>
        /// <param name="psFilePath">The ps file path.</param>
        /// <param name="pDeleteIfExisting">if set to <c>true</c> [p delete if existing].</param>
        public static void FromBase64ToFile(string pData, string psFilePath, bool pDeleteIfExisting)
        {
            if (string.IsNullOrEmpty(pData))
                return;

            if (File.Exists(pData) && pDeleteIfExisting)
                File.Delete(psFilePath);

            byte[] filebytes = Convert.FromBase64String(pData);
            using (var fs = new FileStream(psFilePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None))
            {
                fs.Write(filebytes, 0, filebytes.Length);
            }
        }

        /// <summary>
        /// Gets the CRC of file.
        /// </summary>
        /// <param name="psFile">The file path.</param>
        /// <returns>System.Int64.</returns>
        public static long GetCrcFile(string psFile)
        {
            using (var streamFile = new FileStream(psFile, FileMode.Open))
            {
                var crcFile = new CRC32();
                return crcFile.GetCrc32(streamFile);
            }
        }
    }

    /// <summary>
    /// Calculates a 32bit Cyclic Redundancy Checksum (CRC) using the
    /// same polynomial used by Zip.
    /// </summary>
    internal class CRC32
    {
        /// <summary>
        /// The buffer size
        /// </summary>
        const int BUFFER_SIZE = 1024;

        /// <summary>
        /// The CRC32 table
        /// </summary>
        uint[] crc32Table;
        /// <summary>
        /// Construct an instance of the CRC32 class, pre-initialising the table
        /// for speed of lookup.
        /// </summary>
        public CRC32()
        {
            unchecked
            {
                // This is the official polynomial used by CRC32 in PKZip.
                // Often the polynomial is shown reversed as 0x04C11DB7.
                uint dwPolynomial = 0xEDB88320;
                uint i, j;
                crc32Table = new uint[256];
                uint dwCrc;
                for (i = 0; i < 256; i++)
                {
                    dwCrc = i;
                    for (j = 8; j > 0; j--)
                    {
                        if ((dwCrc & 1) == 1)
                        {
                            dwCrc = (dwCrc >> 1) ^ dwPolynomial;
                        }
                        else
                        {
                            dwCrc >>= 1;
                        }
                    }
                    crc32Table[i] = dwCrc;
                }
            }
        }

        /// <summary>
        /// Returns the CRC32 for the specified stream.
        /// </summary>
        /// <param name="stream">The stream to calculate the CRC32 for</param>
        /// <returns>An unsigned integer containing the CRC32 alculation</returns>
        public uint GetCrc32(Stream stream)
        {
            unchecked
            {
                uint crc32Result;
                crc32Result = 0xFFFFFFFF;
                byte[] buffer = new byte[BUFFER_SIZE];
                int readSize = BUFFER_SIZE;

                int count = stream.Read(buffer, 0, readSize);
                while (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        crc32Result = ((crc32Result) >> 8) ^ crc32Table[(buffer[i]) ^
                         ((crc32Result) & 0x000000FF)];
                    }
                    count = stream.Read(buffer, 0, readSize);
                }

                return ~crc32Result;
            }
        }
    }
}
