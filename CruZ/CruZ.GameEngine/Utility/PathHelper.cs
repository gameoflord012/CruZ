using System;
using System.IO;

using CruZ.GameEngine.Resource;

namespace CruZ.GameEngine.Utility
{
    class PathHelper
    {
        public static bool IsSubpath(string basepath, string subpath)
        {
            if (!Path.IsPathRooted(basepath) || !Path.IsPathRooted(subpath)) throw new ArgumentException("Path must be rooted");
            var relativePath = Path.GetRelativePath(basepath, subpath);
            return IsRelativeASubpath(relativePath);
        }

        public static bool IsRelativeASubpath(ReadOnlySpan<char> relativePath)
        {
            if (Path.IsPathRooted(relativePath)) throw new ArgumentException("relativePath");
            return !relativePath.StartsWith("..") && relativePath != ".";
        }

        public static ReadOnlySpan<char> RemoveExtension(ReadOnlySpan<char> path)
        {
            return path.Slice(0, path.LastIndexOf("."));
        }

        public static void UpdateFolder(string sourceFolder, string destinationFolder, string[]? excludePatterns = null)
        {
            try
            {
                sourceFolder = Path.GetFullPath(sourceFolder);
                destinationFolder = Path.GetFullPath(destinationFolder);

                if(!sourceFolder.EndsWith('\\')) sourceFolder += "\\";
                if(!destinationFolder.EndsWith('\\')) destinationFolder += "\\";

                foreach (string srcFile in DirectoryHelper.EnumerateFiles(sourceFolder, excludePatterns))
                {
                    var destFile = Path.Combine(destinationFolder, srcFile.StripPrefix(sourceFolder));
                    var destDir = Path.GetDirectoryName(destFile) ?? throw new NullReferenceException();

                    if (!Directory.Exists(destDir))
                    {
                        Directory.CreateDirectory(destDir);
                    }

                    if (!File.Exists(destFile) || File.GetLastWriteTime(srcFile) > File.GetLastWriteTime(destFile))
                    {
                        File.Copy(srcFile, destFile, true);
                    }
                }

                foreach (string desFile in DirectoryHelper.EnumerateFiles(destinationFolder, excludePatterns))
                {
                    var sourceFile = Path.Combine(sourceFolder, desFile.StripPrefix(destinationFolder));
                    if (!File.Exists(sourceFile))
                    {
                        File.Delete(desFile);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}