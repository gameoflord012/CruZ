using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CruZ_Engine.Serialization
{
    interface ISerializable
    {
        public ISerializable CreateDefault();
        public void ReadJson(JsonReader reader, JsonSerializer serializer);
        public void WriteJson(JsonWriter writer, JsonSerializer serializer);

    }
}