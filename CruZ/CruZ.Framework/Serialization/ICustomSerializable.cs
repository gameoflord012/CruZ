namespace CruZ.Framework.Serialization
{
    public interface ICustomSerializable
    {
        public object ReadJson(JsonReader reader, JsonSerializer serializer);
        public void WriteJson(JsonWriter writer, JsonSerializer serializer);
    }
}