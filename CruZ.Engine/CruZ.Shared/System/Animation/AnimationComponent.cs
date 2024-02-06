using CruZ.Resource;
using CruZ.Serialization;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        }

        public void Load(SpriteComponent sprite)
        {
            if(_sprite == sprite) return;

            UnLoad();
            _sprite = sprite;

            _sprite.DrawLoopBegin += Sprite_DrawLoopBegin;
            _sprite.DrawLoopEnd   += Sprite_DrawLoopEnd;
        }

        public void UnLoad()
        {
            if(_sprite != null)
            {
                _sprite.DrawLoopBegin -= Sprite_DrawLoopBegin;
                _sprite.DrawLoopEnd   -= Sprite_DrawLoopEnd;
            }

            _sprite = null;
        }

        private void Sprite_DrawLoopBegin(object? sender, DrawLoopBeginEventArgs e)
        {
            e.Texture = _animatedSprite.TextureRegion.Texture;
            e.SourceRectangle = _animatedSprite.TextureRegion.Bounds;
            //e.Origin = 
            //    new(
            //    _animatedSprite.OriginNormalized.X * e.Origin.X, 
            //    _animatedSprite.OriginNormalized.Y * e.Origin.Y);
        }
        
        private void Sprite_DrawLoopEnd(object? sender, DrawLoopEndEventArgs e)
        {
            
        }

        AnimatedSprite _animatedSprite;
        SpriteSheet _spriteSheet;
        SpriteComponent? _sprite;
    }

    public class AnimationComponent : IComponent, IComponentCallback, ISerializable
    {
        [Browsable(false)]
        public Type ComponentType => typeof(AnimationComponent);

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
            if(_currentAnimationPlayer == _getAnimationPlayer[key]) 
                return _currentAnimationPlayer;

            _currentAnimationPlayer?.UnLoad();
            _currentAnimationPlayer = _getAnimationPlayer[key];
            _currentAnimationPlayer.Load(_sprite);

            return _currentAnimationPlayer;
        }

        public void OnAttached(TransformEntity entity)
        {
            _e = entity;
            _e.OnComponentAdded += Entity_OnComponentAdded;
        }

        private void Entity_OnComponentAdded(object? sender, IComponent e)
        {
            _e.TryGetComponent(ref _sprite);
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
        SpriteComponent?                    _sprite;
        TransformEntity?                    _e;

        List<KeyValuePair<string, string>> _loadedResources = [];
    }
}