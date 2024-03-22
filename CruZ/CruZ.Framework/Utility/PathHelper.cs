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
    }
}