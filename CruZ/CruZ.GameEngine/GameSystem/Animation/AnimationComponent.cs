﻿using System;
using System.Collections.Generic;
using System.Text;

using AsepriteDotNet.Aseprite;

using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.Resource;

using Microsoft.Xna.Framework;

using MonoGame.Aseprite;

namespace CruZ.GameEngine.GameSystem.Animation
{
    public class AnimationComponent : Component
    {
        public bool FitToWorldUnit
        {
            get;
            set;
        }

        public AnimationComponent(SpriteRendererComponent spriteRenderer)
        {
            Scale = Vector2.One;
            Offset = Vector2.Zero;
            Color = Color.White;

            _animations = [];
            _resource = GameApplication.Resource;
            _renderer = spriteRenderer;
            _renderer.DrawRequestsFetching += SpriteRenderer_FetchingDrawRequests;
        }

        public void LoadAnimationFile(string resourcePath, string? prefix = default)
        {
            if(!_resource.TryGetCache(resourcePath, out SpriteSheet? spriteSheet))
            {
                var file = _resource.Load<AsepriteFile>(resourcePath, true);
                spriteSheet = file.CreateSpriteSheet(GameApplication.GetGraphicsDevice());

                _resource.Cache(resourcePath, spriteSheet);
            }

            LoadSpriteSheet(spriteSheet!, prefix);
        }

        private void LoadSpriteSheet(SpriteSheet spriteSheet, string? prefix)
        {
            foreach(var tag in spriteSheet.GetAnimationTagNames())
            {
                var animation = spriteSheet.CreateAnimatedSprite(tag);

                // Setting
                animation.OriginX = animation.TextureRegion.Bounds.Width / 2f;
                animation.OriginY = animation.TextureRegion.Bounds.Height / 2f;

                StringBuilder sb = new();
                if(!string.IsNullOrEmpty(prefix))
                {
                    sb.Append(prefix);
                    sb.Append('-');
                }
                sb.Append(tag);

                if(!_animations.TryAdd(sb.ToString().ToLower(), animation))
                    throw new InvalidOperationException("duplicate animation key");
            }
        }

        public void Play(string animationTag, int loopCount = 0,
            Action<AnimatedSprite>? animationEndCallback = default)
        {
            _animationEndCallback = animationEndCallback;

            if(_currentAnimation != null && _currentAnimation.IsAnimating)
            {
                // return if animations are same
                if(animationTag == _currentAnimation.Name)
                    return;

                // we will stop if animation still in play
                _currentAnimation.Stop();
            }

            _currentAnimation = GetAnimation(animationTag);
            _currentAnimation.OnAnimationEnd = OnAnimationEnd;

            _currentAnimation.Play(loopCount);
        }

        public void PlayEmpty()
        {
            _currentAnimation?.Stop();
            _currentAnimation = null;
        }

        private Action<AnimatedSprite>? _animationEndCallback;

        public string CurrentAnimationName()
        {
            if(_currentAnimation == null) throw new InvalidOperationException();
            return _currentAnimation.Name;
        }

        public void Stop()
        {
            _currentAnimation?.Stop();
        }

        private AnimatedSprite GetAnimation(string tag)
        {
            tag = tag.ToLower();

            if(!_animations.TryGetValue(tag, out AnimatedSprite? value))
                throw new ArgumentException(tag);

            return value;
        }

        internal void Update(GameTime gameTime)
        {
            _currentAnimation?.Update(gameTime);
        }

        private void OnAnimationEnd(AnimatedSprite animatedSprite)
        {
            _animationEndCallback?.Invoke(animatedSprite);
            _animationEndCallback = null;
            animatedSprite.OnAnimationEnd = null;
        }

        private void SpriteRenderer_FetchingDrawRequests(List<DrawRequestBase> drawRequests)
        {
            if(_currentAnimation == null) return;

            SpriteDrawArgs spriteArgs = new();
            {
                spriteArgs.Apply(_currentAnimation);

                spriteArgs.Scale = Scale * (FitToWorldUnit ?
                     new Vector2(
                         1f / (_currentAnimation.TextureRegion.Bounds.Width),
                         1f / _currentAnimation.TextureRegion.Bounds.Width) : Vector2.One);

                spriteArgs.Position = AttachedEntity.Position + Offset;
                spriteArgs.Color = Color;
            }

            drawRequests.Add(new SpriteDrawRequest(spriteArgs));
        }

        public Vector2 Scale
        {
            get;
            set;
        }

        public Vector2 Offset
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        private AnimatedSprite? _currentAnimation;
        private SpriteRendererComponent _renderer;
        private ResourceManager _resource;
        private Dictionary<string, AnimatedSprite> _animations;

        public override void Dispose()
        {
            base.Dispose();
            _renderer.DrawRequestsFetching -= SpriteRenderer_FetchingDrawRequests;
        }
    }
}
