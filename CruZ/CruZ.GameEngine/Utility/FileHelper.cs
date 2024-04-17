using CruZ.GameEngine.Serialization;

using System.IO;

namespace CruZ.GameEngine.Utility
{
    public static class FileHelper
    {
        //public static Serializer Serializer { get => _serializer; }

        //static FileHelper()
        //{
        //    _serializer = new Serializer();
        //    _serializer.Converters.Add(new SerializableJsonConverter());
        //}

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

        //public static string ConvertToBinPath(string projectPath)
        //{
        //    var binDir = Environment.CurrentDirectory;
        //    var projectDir = Directory.GetParent(binDir).Parent.Parent.FullName;

        //    var relative = Path.GetRelativePath(projectDir, projectPath);
        //    return Path.GetFullPath(relative);
        //}

        private static Serializer _serializer;
    }
}