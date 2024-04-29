using CruZ.GameEngine.Serialization;

using System.IO;

namespace CruZ.GameEngine.Utility
{
    public static class FileHelper
    {
        public static StreamWriter OpenWrite(string file, bool append = true)
        {
            var dir = Path.GetDirectoryName(file);

            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return new StreamWriter(file, append);
        }

        public static void WriteToFile(string file, string content, bool append = true)
        {
            using (var writer = OpenWrite(file, append))
            {
                writer.Write(content);
            }
        }
    }
}