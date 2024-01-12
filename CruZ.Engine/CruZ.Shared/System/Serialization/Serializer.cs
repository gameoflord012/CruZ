using CruZ.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CruZ.Serialization
{
    public class Serializer
    {
        public IList<JsonConverter> Converters { get => _settings.Converters; set => _settings.Converters = value; }

        public Serializer()
        {
            _settings = new();
            _settings.Formatting = Formatting.Indented;
            _settings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
        }

        public void SerializeToFile(object o, string filePath)
        {
            var json = JsonConvert.SerializeObject(o, _settings);
            using (var writer = Helper.CreateOrOpenFilePath(filePath, false))
            {
                writer.WriteLine(json);
                writer.Flush();
            }
        }

        public T DeserializeFromFile<T>(string filePath) where T : class
        {
            return (T)DeserializeFromFile(filePath, typeof(T));
        }

        public object DeserializeFromFile(string filePath, Type ty)
        {

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("deserialize file {0} not exist", filePath));
            }

            string json;

            using (var reader = new StreamReader(filePath))
            {
                json = reader.ReadToEnd();
            }

            return Deserialize(json, ty);
        }

        public object Deserialize(string json, Type ty)
        {
            object o;

            try
            {
                o = JsonConvert.DeserializeObject(json, ty, _settings);
            }
            catch (JsonSerializationException e)
            {
                throw new JsonSerializationException(string.Format("can't deserialize data \"{0}\" to type {1}", json, ty), e);
            }
            catch (JsonReaderException e)
            {
                throw new JsonReaderException(string.Format("can't deserialize data \"{0}\" to type {1}", json, ty), e);
            }

            if (o == null)
            {
                throw new(string.Format("Problem to deserialize \"{0}\" to type {1}", json, ty));
            }

            return o;
        }

        JsonSerializerSettings _settings;
    }
}
