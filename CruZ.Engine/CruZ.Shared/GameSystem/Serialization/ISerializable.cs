using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CruZ.Serialization
{
    public interface ICustomSerializable
    {
        public object ReadJson(JsonReader reader, JsonSerializer serializer);
        public void WriteJson(JsonWriter writer, JsonSerializer serializer);
    }
}