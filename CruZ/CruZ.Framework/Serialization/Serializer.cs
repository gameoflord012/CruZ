using CruZ.Framework.Utility;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CruZ.Framework.Serialization
{
    public class Serializer
    {
        public IList<JsonConverter> Converters { get => _settings.Converters; set => _settings.Converters = value; }

        public Serializer()
        {
            _settings = new();
            _settings.Formatting = Formatting.Indented;
            _settings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
            _settings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            _settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public void SerializeToFile(object o, string filePath)
        {
            var json = JsonConvert.SerializeObject(o, _settings);
            using (var writer = FileHelper.OpenWrite(filePath, false))
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
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Problem to deserialize \"{0}\" to type {1}", json, ty), e);
            }

            Trace.Assert(o != null);

            return o;
        }

        JsonSerializerSettings _settings;
    }
}
