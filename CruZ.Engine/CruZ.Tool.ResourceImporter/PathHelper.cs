using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CruZ.Tools.ResourceImporter
{
    internal class PathHelper
    {
        public static string GetRelativePath(string relativeFolder, string destinationFile)
        {
            Uri folder = new Uri(Path.GetFullPath(relativeFolder).TrimEnd('\\') + "\\");
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
    }
}
