using System.IO;

namespace SimpleHelper
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
    }
}
