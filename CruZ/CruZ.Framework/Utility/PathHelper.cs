using System;
using System.IO;

namespace CruZ.Framework.Utility
{
    class PathHelper
    {
        public static bool IsSubPath(string basepath, string subpath)
        {
            if (!Path.IsPathRooted(basepath) || !Path.IsPathRooted(subpath)) throw new ArgumentException("Path must be rooted");
            var rel = Path.GetRelativePath(basepath, subpath);
            rel.Replace("//", "\\");
            return !rel.StartsWith("..\\") && rel != ".";
        }

        public static void UpdateFolderContents(string sourceFolder, string destinationFolder, string pattern = "*", Boolean createFolders = false, Boolean recurseFolders = false)
        {
            try
            {
                if (!sourceFolder.EndsWith(@"\")) { sourceFolder += @"\"; }
                if (!destinationFolder.EndsWith(@"\")) { destinationFolder += @"\"; }
                sourceFolder = Path.GetFullPath(sourceFolder);
                destinationFolder = Path.GetFullPath(destinationFolder);

                SearchOption so = (recurseFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                foreach (string file in Directory.GetFiles(sourceFolder, pattern, so))
                {
                    FileInfo srcFile = new FileInfo(file);

                    // Create a destination that matches the source structure
                    FileInfo destFile = new FileInfo(destinationFolder + srcFile.FullName.StripPrefix(sourceFolder));

                    if (!Directory.Exists(destFile.DirectoryName) && createFolders)
                    {
                        Directory.CreateDirectory(destFile.DirectoryName);
                    }

                    if (!destFile.Exists || srcFile.LastWriteTime > destFile.LastWriteTime)
                    {
                        File.Copy(srcFile.FullName, destFile.FullName, true);
                    }
                }

                foreach (string file in Directory.GetFiles(destinationFolder, pattern, so))
                {
                    FileInfo dstFile = new FileInfo(file);
                    FileInfo srcFile = new FileInfo(sourceFolder + dstFile.FullName.StripPrefix(destinationFolder));

                    if (!srcFile.Exists) dstFile.Delete();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}