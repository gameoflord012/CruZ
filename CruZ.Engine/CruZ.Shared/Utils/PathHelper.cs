using System.IO;

namespace CruZ.Utility
{
    class PathHelper
    {
        public static bool IsSubPath(string basepath, string subpath)
        {
            var rel = Path.GetRelativePath(basepath, subpath);
            rel.Replace("//", "\\");
            return !rel.StartsWith("..\\") && rel != ".";
        }
    }
}