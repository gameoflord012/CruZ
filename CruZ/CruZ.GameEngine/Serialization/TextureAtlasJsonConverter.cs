//using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//using CruZ.GameEngine.GameSystem;
//using CruZ.GameEngine.Resource;

//using Microsoft.Xna.Framework.Graphics;

//namespace CruZ.GameEngine.Serialization
//{
//    internal class TextureAtlasJsonConverter : JsonConverter<TextureAtlas>
//    {
//        internal class InlineTextureAtlas
//        {
//            public string Texture { get; set; }
//            public int RegionWidth { get; set; }
//            public int RegionHeight { get; set; }

//            [JsonIgnore]
//            public Guid TextureGuid => new Guid(_textureGuid);

//            [JsonInclude, JsonPropertyName("textureGuid")]
//            private string _textureGuid { get; set; }
//        }

//        public TextureAtlasJsonConverter(ResourceManager resourceManager)
//        {
//            _resource = resourceManager;
//        }

//        public override void Write(Utf8JsonWriter writer, TextureAtlas value, JsonSerializerOptions options)
//        {
//            throw new NotImplementedException();
//        }

//        public override TextureAtlas? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            var helperConverter = (JsonConverter<InlineTextureAtlas>)options.GetConverter(typeof(InlineTextureAtlas));

//            var inlineAtlas = helperConverter.Read(ref reader, typeToConvert, options);
//            var texture = _resource.Load<Texture2D>(inlineAtlas.TextureGuid);
//            return TextureAtlas.Create(texture, inlineAtlas.RegionWidth, inlineAtlas.RegionHeight);
//        }

//        ResourceManager _resource;
//    }
//}
