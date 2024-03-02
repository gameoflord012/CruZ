using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CruZ.Tools.ResourceImporter
{
    internal class PathHelper
    {
        public static string GetRelativePath(string rootFolder, string destinationFile)
        {
            Uri folder = new Uri(Path.GetFullPath(rootFolder).TrimEnd('\\') + "\\");
            Uri file = new Uri(Path.GetFullPath(destinationFile));

            return Uri.UnescapeDataString(
                folder.MakeRelativeUri(file)
                    .ToString()
                    .Replace('/', Path.DirectorySeparatorChar)
                );
        }

        public static string GetPathWithoutExtension(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            var file = Path.GetFileNameWithoutExtension(filePath);
            return Path.Combine(dir, file);

        }

        public static bool IsPathSubpath(string parentPath, string subPath)
        {
            if(!Path.IsPathRooted(parentPath) || !Path.IsPathRooted(subPath))
                throw new ArgumentException("Path must be rooted");
            
            var parentDirInfo = new DirectoryInfo(parentPath);
            var subpathDirInfo = new DirectoryInfo(subPath);

            bool isSubpath = false;
            while(subpathDirInfo.Parent != null)
            {
                if(subpathDirInfo.Parent.FullName == parentDirInfo.FullName)
                {
                    isSubpath = true;
                    break;
                }
                else
                    subpathDirInfo = subpathDirInfo.Parent;
            }

            return isSubpath;
        }
    }
}
