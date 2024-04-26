using AsepriteDotNet.Aseprite;

using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.Resource;

using Microsoft.Xna.Framework;

using MonoGame.Aseprite;

using SharpDX.MediaFoundation;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CruZ.GameEngine.GameSystem.Animation
{
    public class AnimationComponent : Component
    {
        /// <summary>
        /// If true, the rendering sprite will only 1 unit in world coordinate
        /// </summary>
        public bool FitToWorldUnit { get; set; }

        public AnimationComponent()
        {
            _resource = GameContext.GameResource;
            _file = null!;
        }

        public void LoadAnimationFile(string asepriteFile)
        {
            LoadAsepriteFile(_resource.Load<AsepriteFile>(asepriteFile));
        }

        private void LoadAsepriteFile(AsepriteFile file)
        {
            _file = file;
            _animations.Clear();

            var spriteSheet = _file.CreateSpriteSheet(GameApplication.GetGraphicsDevice());

            foreach (var tag in file.Tags)
            {
                var animation = spriteSheet.CreateAnimatedSprite(tag.Name);

                // Setting
                animation.OriginX = animation.TextureRegion.Bounds.Width / 2f;
                animation.OriginY = animation.TextureRegion.Bounds.Height / 2f;

                _animations.Add(tag.Name.ToLower(), animation);
            }
        }

        //void IJsonOnDeserialized.OnDeserialized()
        //{
        //    LoadAsepriteFile(_resource.Load<AsepriteFile>(_asepriteResourceInfo));
        //}

        public void PlayAnimation(string animationTag, int loopCount = 0)
        {
            animationTag = animationTag.ToLower();

            if (_currentAnimation != null && animationTag == _currentAnimation.Name)
                return;

            _currentAnimation?.Stop();
            _currentAnimation = GetAnimation(animationTag);
            _currentAnimation.Play(loopCount);
        }

        private AnimatedSprite GetAnimation(string tag)
        {
            tag = tag.ToLower();

            if (!_animations.TryGetValue(tag, out AnimatedSprite? value))
                throw new ArgumentException(tag);

            return value;
        }

        internal void Update(GameTime gameTime)
        {
            _currentAnimation?.Update(gameTime);
        }

        private void SpriteRenderer_FetchingDrawRequests(FetchingDrawRequestsEventArgs args)
        {
            if (_currentAnimation == null) return;
            var defaultArgs = args.DefaultDrawArgs;
            defaultArgs.Apply(_currentAnimation);
            defaultArgs.Apply(AttachedEntity);

            if(FitToWorldUnit)
            {
                defaultArgs.Scale =
                new Vector2(
                    1f / (_currentAnimation.TextureRegion.Bounds.Width),
                    1f / _currentAnimation.TextureRegion.Bounds.Height);
            }
            else
            {
                defaultArgs.Scale = Vector2.One;
            }

            args.DrawRequests.Add(defaultArgs);
        }

        public SpriteRendererComponent Renderer
        {
            get => _renderer ?? throw new NullReferenceException();
            set
            {
                if (_renderer != null)
                    _renderer.DrawRequestsFetching -= SpriteRenderer_FetchingDrawRequests;

                _renderer = value;

                if (_renderer != null)
                    _renderer.DrawRequestsFetching += SpriteRenderer_FetchingDrawRequests;
            }
        }

        public AnimatedSprite? CurrentAnimation
        {
            get => _currentAnimation; 
            private set => _currentAnimation = value;
        }

        AnimatedSprite? _currentAnimation;
        
        SpriteRendererComponent? _renderer;

        ResourceManager _resource;
        Dictionary<string, AnimatedSprite> _animations = [];
        AsepriteFile _file;
    }
}