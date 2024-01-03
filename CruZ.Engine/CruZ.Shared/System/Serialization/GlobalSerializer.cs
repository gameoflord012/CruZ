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
            using (var writer = Helper.CreateOrOpenFilePath(filePath, false))
            {
                writer.WriteLine(json);
                writer.Flush();
            }
        }

        public static T DeserializeFromFile<T>(string uri) where T : class
        {
            return (T)DeserializeFromFile(uri, typeof(T));
        }

        public static object DeserializeFromFile(string uri, Type ty)
        {

            if (!File.Exists(uri))
            {
                throw new(string.Format("deserialize file {0} not exist", uri));
            }

            string json;

            using (var reader = new StreamReader(uri))
            {
                json = reader.ReadToEnd();
            }

            return Deserialize(json, ty);

        }

        public static object Deserialize(string json, Type ty)
        {
            return JsonConvert.DeserializeObject(json, ty, _settings);
        }

        static JsonSerializerSettings _settings;
    }
}