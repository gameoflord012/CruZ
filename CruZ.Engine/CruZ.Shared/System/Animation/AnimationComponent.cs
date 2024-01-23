using CruZ.Resource;
using CruZ.Serialization;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CruZ.Components
{
    public class AnimationPlayer
    {
        public AnimationPlayer(SpriteSheet spriteSheet)
        {
            _spriteSheet = spriteSheet;
            _animatedSprite = new AnimatedSprite(spriteSheet);
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

        public void Update(GameTime gameTime, SpriteComponent sprite)
        {
            _animatedSprite.Update(gameTime);

            sprite.Texture = _animatedSprite.TextureRegion.Texture;
            sprite.SourceRectangle = _animatedSprite.TextureRegion.Bounds;
            sprite.Origin = _animatedSprite.Origin;
        }

        AnimatedSprite _animatedSprite;
        SpriteSheet _spriteSheet;
    }

    public class AnimationComponent : IComponent, IComponentCallback, ISerializable
    {
        public Type ComponentType   => typeof(AnimationComponent);

        public void LoadSpriteSheet(string resourcePath, string animationPlayerKey)
        {
            var spriteSheet = ResourceManager.LoadResource<SpriteSheet>(resourcePath);

            _getAnimationPlayer[animationPlayerKey] = new AnimationPlayer(spriteSheet);
            _loadedResources.Add(new(resourcePath, animationPlayerKey));
        }

        public void Update(GameTime gameTime)
        {
            _currentAnimationPlayer?.Update(gameTime, _sprite);
        }

        public AnimationPlayer SelectPlayer(string key)
        {
            _currentAnimationPlayer = _getAnimationPlayer[key];
            return _currentAnimationPlayer;
        }

        public void OnComponentAdded(TransformEntity entity)
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

            foreach (var player in jObject["animation-players"])
            {
                string? uri = player["resource-uri"].Value<string>();
                string? playerKey = player["animation-player-key"].Value<string>();

                if (string.IsNullOrEmpty(uri)) continue;
                Trace.Assert(playerKey != null);

                LoadSpriteSheet(uri, playerKey);
            }
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("animation-players");
            writer.WriteStartArray();
            foreach (var resource in _loadedResources)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("resource-uri");
                writer.WriteValue(resource.Key);
                writer.WritePropertyName("animation-player-key");
                writer.WriteValue(resource.Value);
                writer.WriteEndObject();
            }
            writer.WriteEnd();
            writer.WriteEnd();
        }

        AnimationPlayer?                    _currentAnimationPlayer;
        Dictionary<string, AnimationPlayer> _getAnimationPlayer = new();
        SpriteComponent                     _sprite;
        TransformEntity?                    _e;

        List<KeyValuePair<string, string>> _loadedResources = [];
    }
}