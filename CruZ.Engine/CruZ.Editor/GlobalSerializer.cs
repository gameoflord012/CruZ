using Newtonsoft.Json;
using System.IO;

namespace CurZ.Editor
{
    public static class GlobalSerializer
    {
        static GlobalSerializer()
        {
            _settings = new();
            _settings.Formatting = Formatting.Indented;
            _settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        }

        public static string SerializeToFile(object o, string filePath)
        {
            var json = JsonConvert.SerializeObject(o, _settings);
            var dir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(filePath, json);

            return json;
        }

        public static object? DeserializeFromFile(string filePath, Type ty)
        {

            if (!File.Exists(filePath)) return default;

            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject(json, ty, _settings);
            }
        }

        static JsonSerializerSettings _settings;
    }
}