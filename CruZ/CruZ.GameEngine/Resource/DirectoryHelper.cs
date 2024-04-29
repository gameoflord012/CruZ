using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine.Utility;

namespace CruZ.GameEngine.Resource
{
    internal class DirectoryHelper
    {
        public static IEnumerable<string> EnumerateFiles(string dir, string[]? excludeDirname = null)
        {
            excludeDirname ??= [];

            // enumerate only files in this directory (no sub directories)
            foreach (string file in Directory.EnumerateFiles(
                   dir, "*.*", SearchOption.TopDirectoryOnly))
            {
                yield return file;
            }

            // now recurse the subdirectories
            foreach (string subDir in Directory.GetDirectories(
                   dir, "*", SearchOption.TopDirectoryOnly))
            {
                var dirName = subDir.Substring(subDir.LastIndexOf("\\") + 1);
                if(dirName == "." || dirName == ".." || excludeDirname.Contains(dirName)) continue;
                foreach (var subDirFile in EnumerateFiles(subDir, excludeDirname))
                    yield return subDirFile;
            }
        }
    }
}
