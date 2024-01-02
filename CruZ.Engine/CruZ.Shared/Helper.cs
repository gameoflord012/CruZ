using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace CruZ.Utility
{
    public class Helper
    {
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
    }
}