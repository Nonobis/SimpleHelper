using System;
using System.IO;
using System.Threading.Tasks;

namespace SimpleHelper.Core
{
    #region Internal Class

    /// <summary>
    /// Calculates a 32bit Cyclic Redundancy Checksum (CRC) using the
    /// same polynomial used by Zip.
    /// </summary>
    public class Crc32
    {
        private readonly uint[] _crc32Table;
        private const int BufferSize = 1024;

        /// <summary>
        /// Returns the CRC32 for the specified stream.
        /// </summary>
        /// <param name="stream">The stream to calculate the CRC32 for</param>
        /// <returns>An unsigned integer containing the CRC32 alculation</returns>
        public uint GetCrc32(Stream stream)
        {
            unchecked
            {
                var crc32Result = 0xFFFFFFFF;
                var buffer = new byte[BufferSize];
                var readSize = BufferSize;

                var count = stream.Read(buffer, 0, readSize);
                while (count > 0)
                {
                    for (var i = 0; i < count; i++)
                    {
                        crc32Result = ((crc32Result) >> 8) ^ _crc32Table[(buffer[i]) ^
                         ((crc32Result) & 0x000000FF)];
                    }
                    count = stream.Read(buffer, 0, readSize);
                }

                return ~crc32Result;
            }
        }

        /// <summary>
        /// Construct an instance of the CRC32 class, pre-initialising the table
        /// for speed of lookup.
        /// </summary>
        public Crc32()
        {
            unchecked
            {
                // This is the official polynomial used by CRC32 in PKZip.
                // Often the polynomial is shown reversed as 0x04C11DB7.
                const uint dwPolynomial = 0xEDB88320;
                uint i, j;
                _crc32Table = new uint[256];
                for (i = 0; i < 256; i++)
                {
                    var dwCrc = i;
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
                    _crc32Table[i] = dwCrc;
                }
            }
        }
    }

    #endregion

    public static class FileHelper
    {
        /// <summary>
        /// Copies the files recursively.
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pTarget">The target.</param>
        /// <param name="pDeleteExistingTarget"></param>
        public static bool CopyFilesRecursively(string pSource, string pTarget, bool pDeleteExistingTarget)
        {
            return CopyFilesRecursively(new DirectoryInfo(pSource), new DirectoryInfo(pTarget), pDeleteExistingTarget);
        }

        /// <summary>
        /// Copies the files recursively.
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pTarget">The target.</param>
        /// <param name="pDeleteExistingTarget"></param>
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
        /// Get file content as Base64
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        public static string FileToBase64(string pFilePath)
        {
            var sValue = string.Empty;
            var oFile = new FileInfo(pFilePath);
            if (!oFile.Exists)
                return sValue;

            var fs = new FileStream(oFile.FullName,
                FileMode.Open,
                FileAccess.Read);
            var filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            sValue = Convert.ToBase64String(filebytes);
            return sValue;
        }

        /// <summary>
        /// Write file from base64 string
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="psFilePath"></param>
        /// <param name="pDeleteIfExisting"></param>
        public static void FromBase64ToFile(string pData, string psFilePath, bool pDeleteIfExisting)
        {
            if (string.IsNullOrEmpty(pData))
                return;

            if (File.Exists(pData) && pDeleteIfExisting)
                File.Delete(psFilePath);

            var filebytes = Convert.FromBase64String(pData);
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
        /// <returns></returns>
        public static long GetCrcFile(string psFile)
        {
            using (var streamFile = new FileStream(psFile, FileMode.Open))
            {
                var crcFile = new Crc32();
                return crcFile.GetCrc32(streamFile);
            }
        }
    }
}
