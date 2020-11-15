using System.IO;

namespace SimpleHelper.Core
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// Clears the content of the folder.
        /// </summary>
        /// <param name="psFolder">The ps folder.</param>
        public static void ClearFolderContent(string psFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(psFolder);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolderContent(di.FullName);
                di.Delete();
            }
        }

        /// <summary>
        /// Copy directory with sub directory
        /// </summary>
        /// <param name="sourceDirectory">The source directory folder</param>
        /// <param name="destinationDirectory">The destination folder</param>
        /// <param name="overwrite">overwrite files inside the directory</param>
        public static void CopyDirectory(string sourceDirectory, string destinationDirectory, bool overwrite = true)
        {

            try
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDirectory);

                if (!sourceDirectoryInfo.Exists)
                    throw new DirectoryNotFoundException($"Source directory {sourceDirectory} not found");

                if (!Directory.Exists(destinationDirectory))
                    Directory.CreateDirectory(destinationDirectory);


                foreach (FileInfo sourceFileInfo in sourceDirectoryInfo.GetFiles())
                {
                    sourceFileInfo.CopyTo(Path.Combine(destinationDirectory, sourceFileInfo.Name), overwrite);
                }

                foreach (DirectoryInfo sourceSubDirectoryInfo in sourceDirectoryInfo.GetDirectories())
                {
                    CopyDirectory(sourceSubDirectoryInfo.FullName, Path.Combine(destinationDirectory, sourceSubDirectoryInfo.Name), overwrite);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// check given directory is hidden or not
        /// </summary>
        /// <param name="directoryPath">Path to the directory</param>
        /// <returns>Returns bool Is Hidden or not</returns>
        public static bool IsHidden(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            if ((dir.Attributes & FileAttributes.Hidden) == (FileAttributes.Hidden))
            {
                return true;
            }
            return false;
        }
    }
}
