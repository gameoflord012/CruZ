using System.Globalization;
using System.IO;

namespace CruZ.Utility
{
    public class Helper
    {
        public static StreamWriter CreateOrOpenFilePath(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            var file = Path.GetFileName(filePath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(file);
            }

            var fileStream = File.Open(filePath, FileMode.OpenOrCreate);
            StreamWriter writer = new(fileStream);

            return writer;
        }
    }
}