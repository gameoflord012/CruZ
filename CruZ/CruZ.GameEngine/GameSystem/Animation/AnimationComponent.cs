using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;

using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.Resource;

using Microsoft.Xna.Framework;

using MonoGame.Aseprite;

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

        public void LoadAnimationFile(string asepriteFile)
        {
             LoadAsepriteFile(_resource.Load<AsepriteFile>(asepriteFile, out _asepriteResourceInfo));
        }

        private void LoadAsepriteFile(AsepriteFile file)
        {
            _file = file;
            _animations.Clear();

            var spriteSheet = _file.CreateSpriteSheet(GameApplication.GetGraphicsDevice());

            foreach (var tag in file.Tags)
            {
                var animation = spriteSheet.CreateAnimatedSprite(tag.Name);
                _animations.Add(tag.Name, animation);
            }
        }

        void IJsonOnDeserialized.OnDeserialized()
        {
            LoadAsepriteFile(_resource.Load<AsepriteFile>(_asepriteResourceInfo));
        }

        public void PlayAnimation(string animationTag)
        {
            _currentAnimation?.Stop();
            _currentAnimation = GetAnimation(animationTag);
            _currentAnimation.Play();
        }

        private AnimatedSprite GetAnimation(string tag)
        {
            if(!_animations.ContainsKey(tag)) throw new ArgumentException();
            return _animations[tag];
        }

        internal void Update(GameTime gameTime)
        {
            _currentAnimation?.Update(gameTime);
        }

        private void SpriteRenderer_DrawLoopBegin(object? sender, DrawSpriteArgs e)
        {
            if(_currentAnimation == null) return;

            e.Texture = _currentAnimation.TextureRegion.Texture;
            e.SourceRectangle = _currentAnimation.TextureRegion.Bounds;
        }

        protected override void OnComponentChanged(ComponentCollection comps)
        {
            if (_renderer != null)
                _renderer.DrawLoopBegin -= SpriteRenderer_DrawLoopBegin;

            comps.TryGetComponent(out _renderer);

            if (_renderer != null)
                _renderer.DrawLoopBegin += SpriteRenderer_DrawLoopBegin;
        }

        AnimatedSprite? _currentAnimation;
        SpriteRendererComponent? _renderer;
        ResourceManager _resource;
        Dictionary<string, AnimatedSprite> _animations = [];
        AsepriteFile _file;

        /// <summary>
        /// store list of loaded animation
        /// Key: resource path of animation
        /// Value: animation key
        /// </summary>
        [JsonInclude]
        ResourceInfo _asepriteResourceInfo;
    }
}