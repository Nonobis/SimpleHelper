using System.IO;
using System.Threading.Tasks;

namespace SimpleHelper
{
    #region Internal Class

    /// <summary>
    /// Calculates a 32bit Cyclic Redundancy Checksum (CRC) using the
    /// same polynomial used by Zip.
    /// </summary>
    public class CRC32
    {
        uint[] crc32Table;
        const int BUFFER_SIZE = 1024;

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
    }

    #endregion

    public static class FileHelper
    {
        /// <summary>
        /// Copies the files recursively.
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pTarget">The target.</param>
        public static bool CopyFilesRecursively(string pSource, string pTarget, bool pDeleteExistingTarget)
        {
            return CopyFilesRecursively(new DirectoryInfo(pSource), new DirectoryInfo(pTarget), pDeleteExistingTarget);
        }

        /// <summary>
        /// Copies the files recursively.
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pTarget">The target.</param>
        public static bool CopyFilesRecursively(DirectoryInfo pSource, DirectoryInfo pTarget, bool pDeleteExistingTarget)
        {
            try
            {
                foreach (DirectoryInfo dir in pSource.GetDirectories())
                {
                    DirectoryInfo targetSubDirectory = pTarget.CreateSubdirectory(dir.Name);
                    CopyFilesRecursively(dir, targetSubDirectory, pDeleteExistingTarget);
                }

                Parallel.ForEach(pSource.GetFiles(),
                filePath =>
                {
                    Task task = Task.Factory.StartNew(() =>
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
        /// Gets the CRC of file.
        /// </summary>
        /// <param name="psFile">The file path.</param>
        /// <returns></returns>
        public static long GetCRCFile(string psFile)
        {
            using (var streamFile = new FileStream(psFile, FileMode.Open))
            {
                var crcFile = new CRC32();
                return crcFile.GetCrc32(streamFile as Stream);
            }
        }
    }
}
