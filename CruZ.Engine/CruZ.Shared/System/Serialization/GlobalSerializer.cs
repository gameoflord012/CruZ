using CruZ.Serialization;
using CruZ.Utility;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CurZ.Serialization
{
    public static class GlobalSerializer
    {
        static GlobalSerializer()
        {
            _settings = new();
            _settings.Formatting = Formatting.Indented;
            _settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

            _settings.Converters.Add(new SerializableJsonConverter());
        }

        public static void SerializeToFile(object o, string filePath)
        {
            var json = JsonConvert.SerializeObject(o, _settings);
            var writer = Helper.CreateOrOpenFilePath(filePath);

            writer.WriteLine(json);
            writer.Flush();
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