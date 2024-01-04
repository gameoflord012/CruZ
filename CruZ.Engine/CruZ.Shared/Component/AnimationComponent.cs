using CruZ.Resource;
using CruZ.Serialization;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CruZ.Components
{
    public class AnimationComponent : IComponent, IComponentCallback, ISerializable
    {
        public Type ComponentType   => typeof(AnimationComponent);

        public void LoadSpriteSheet(URI uri)
        {
            _spriteSheet = ResourceManager.LoadContent<SpriteSheet>(uri);
            _animatedSprite = new AnimatedSprite(_spriteSheet);
            _spriteSheetURI = uri;
        }

        public void Play(string animationName)
        {
            _animatedSprite.Play(animationName);
        }

        public void Update(GameTime gameTime)
        {
            _animatedSprite.Update(gameTime);

            _sprite.Texture = _animatedSprite.TextureRegion.Texture;
            _sprite.SourceRectangle = _animatedSprite.TextureRegion.Bounds;
            _sprite.Origin = _animatedSprite.Origin;
        }

        public void OnEntityChanged(TransformEntity entity)
        {
            _e = entity;
            _sprite = _e.GetComponent<SpriteComponent>();
        }

        public ISerializable? CreateDefault()
        {
            return new AnimationComponent();
        }

        public void ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            string uri = jObject["sprite-sheet-uri"].Value<string>();
            LoadSpriteSheet(uri);
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("sprite-sheet-uri");
            writer.WriteValue(_spriteSheetURI);
            writer.WriteEnd();
        }

        string _spriteSheetURI;

        AnimatedSprite _animatedSprite;
        SpriteSheet _spriteSheet;
        SpriteComponent _sprite;
        TransformEntity? _e;
    }
}