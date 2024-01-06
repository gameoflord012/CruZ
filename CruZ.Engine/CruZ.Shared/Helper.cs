using CruZ.Serialization;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CruZ.Utility
{
    public static class Helper
    {
        public static Serializer Serializer { get => _serializer; }

        static Helper()
        {
            _serializer = new Serializer();
            _serializer.Converters.Add(new SerializableJsonConverter());
        }

        public static StreamWriter CreateOrOpenFilePath(string filePath, bool append = true)
        {
            var dir = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.Open(filePath, FileMode.OpenOrCreate).Close();
            var writer = new StreamWriter(filePath, append);

            return writer;
        }

        public static T GetUnitializeObject<T>()
        {
            return (T)GetUnitializeObject(typeof(T));
        }

        public static object GetUnitializeObject(Type type)
        {
            return RuntimeHelpers.GetUninitializedObject(type);
        }

        public static string ConvertToBinPath(string projectPath)
        {
            var binDir = Environment.CurrentDirectory;
            var projectDir = Directory.GetParent(binDir).Parent.Parent.FullName;

            var relative = Path.GetRelativePath(projectDir, projectPath);
            return Path.GetFullPath(relative);
        }

        private static Serializer _serializer;
    }
}