using CruZ.Resource;
using CruZ.Serialization;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CruZ.Components
{
    public class AnimationComponent : IComponent, IComponentCallback, ISerializable
    {
        public Type ComponentType   => typeof(AnimationComponent);

        public void LoadSpriteSheet(URI uri)
        {
            _spriteSheet = ResourceManager.LoadResource<SpriteSheet>(uri);
            _animatedSprite = new AnimatedSprite(_spriteSheet);
            _spriteSheetURI = uri.ToString();
        }

        public void Play(string animationName)
        {
            try
            {
                _animatedSprite.Play(animationName);
            }
            catch (KeyNotFoundException e)
            {
                throw new(string.Format("Cant found animation with key {0}", animationName), e);
            }
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
            string? uri = jObject["sprite-sheet-uri"].Value<string>();

            if (string.IsNullOrEmpty(uri)) return;

            LoadSpriteSheet(new(uri));
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