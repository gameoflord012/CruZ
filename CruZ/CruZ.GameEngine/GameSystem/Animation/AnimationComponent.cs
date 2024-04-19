using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.Resource;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CruZ.GameEngine.GameSystem.Animation
{
    public class AnimationComponent : Component, IJsonOnDeserialized
    {
        public AnimationComponent()
        {
            _resource = GameContext.GameResource;
        }

        public void LoadAnimationPlayer(string resourcePath, string animationPlayerKey)
        {
            var spriteSheet = _resource.Load<AnimatedSprite>(resourcePath);

            _getAnimationPlayer[animationPlayerKey] = new AnimationPlayer(spriteSheet);
            _loadedResources.Add(new(resourcePath, animationPlayerKey));
        }

        public AnimationPlayer SelectPlayer(string key)
        {
            if (_currentAnimationPlayer != GetPlayer(key))
                return _currentAnimationPlayer = GetPlayer(key);

            return _currentAnimationPlayer;
        }

        private AnimationPlayer GetPlayer(string key)
        {
            if (!_getAnimationPlayer.ContainsKey(key))
                throw new ArgumentException($"No animation with key {key}");

            return _getAnimationPlayer[key];
        }

        protected override void OnComponentChanged(ComponentCollection comps)
        {
            if (_renderer != null)
                _renderer.DrawLoopBegin -= SpriteRenderer_DrawLoopBegin;

            comps.TryGetComponent(out _renderer);

            if (_renderer != null)
                _renderer.DrawLoopBegin += SpriteRenderer_DrawLoopBegin;
        }

        internal void Update(GameTime gameTime)
        {
            _currentAnimationPlayer?.Update(gameTime);
        }

        private void SpriteRenderer_DrawLoopBegin(object? sender, DrawArgs e)
        {
            _currentAnimationPlayer?.Draw(e);
        }

        void IJsonOnDeserialized.OnDeserialized()
        {
            foreach (var resource in _loadedResources)
            {
                LoadAnimationPlayer(resource.Value, resource.Key);
            }
        }

        AnimationPlayer? _currentAnimationPlayer;
        SpriteRendererComponent? _renderer;
        ResourceManager _resource;
        Dictionary<string, AnimationPlayer> _getAnimationPlayer = new();

        /// <summary>
        /// store list of loaded animation
        /// Key: resource path of animation
        /// Value: animation key
        /// </summary>
        [JsonInclude]
        List<KeyValuePair<string, string>> _loadedResources = [];
    }
}