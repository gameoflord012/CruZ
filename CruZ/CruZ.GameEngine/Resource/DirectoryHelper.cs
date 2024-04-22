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
        public static IEnumerable<string> EnumerateFiles(string dir, string[] excludeDirs)
        {

            List<string> DirList = new List<string>();
            // enumerate only files in this directory (no sub directories)
            foreach (string file in Directory.EnumerateFiles(
                   dir, "*.*", SearchOption.TopDirectoryOnly))
            {
                yield return file;
            }

            // now recurse the subdirectories
            foreach (string subdir in Directory.GetDirectories(
                   dir, "*", SearchOption.TopDirectoryOnly).
                Where(sd => sd != "." && sd != ".." && !DirList.Contains(sd)))
            {
                EnumerateFiles(Path.Combine(dir, subdir), excludeDirs);
            }
        }
    }
}
