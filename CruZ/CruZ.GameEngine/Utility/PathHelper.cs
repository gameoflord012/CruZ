using System;
using System.IO;

namespace CruZ.GameEngine.Utility
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

        // TEST: 
        //public static string Normalized(string path)
        //{
        //    string normalized;

        //    if(Path.IsPathRooted(path))
        //    {
        //        normalized = Path.GetFullPath(path, "D");
        //        normalized = normalized.Remove(0, 3); // remove D:\
        //    }
        //    else
        //    {
        //        normalized = Path.GetFullPath(path);
        //    }

        //    normalized = normalized.TrimEnd('\\');
        //    return normalized;
        //}

        public static void UpdateFolder(string sourceFolder, string destinationFolder, string pattern = "*", bool createFolders = false, bool recursively = false)
        {
            try
            {
                if (!sourceFolder.EndsWith(@"\")) { sourceFolder += @"\"; }
                if (!destinationFolder.EndsWith(@"\")) { destinationFolder += @"\"; }
                sourceFolder = Path.GetFullPath(sourceFolder);
                destinationFolder = Path.GetFullPath(destinationFolder);

                SearchOption so = recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

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
            catch
            {
                throw;
            }
        }
    }
}