using AsepriteDotNet.Aseprite;

using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine.Resource;

using Microsoft.Xna.Framework;

using MonoGame.Aseprite;

using System;
using System.Collections.Generic;
using System.Text;

namespace CruZ.GameEngine.GameSystem.Animation
{
    public class AnimationComponent : Component
    {
        /// <summary>
        /// If true, the rendering sprite will only 1 unit in world coordinate
        /// </summary>
        public bool FitToWorldUnit { get; set; }

        public Vector2 Scale = Vector2.One;

        public Vector2 Offset = Vector2.Zero;

        public Color Color = Color.White;

        public AnimationComponent(SpriteRendererComponent spriteRenderer)
        {
            _resource = GameApplication.Resource;
            _file = null!;

            _renderer = spriteRenderer;
            _renderer.DrawRequestsFetching += SpriteRenderer_FetchingDrawRequests;
        }

        public void LoadAnimationFile(string asepriteFile, string? prefix = default)
        {
            LoadAsepriteFile(_resource.Load<AsepriteFile>(asepriteFile), prefix);
        }

        private void LoadAsepriteFile(AsepriteFile file, string? prefix)
        {
            _file = file;

            var spriteSheet = _file.CreateSpriteSheet(GameApplication.GetGraphicsDevice());

            foreach (var tag in file.Tags)
            {
                var animation = spriteSheet.CreateAnimatedSprite(tag.Name);

                // Setting
                animation.OriginX = animation.TextureRegion.Bounds.Width / 2f;
                animation.OriginY = animation.TextureRegion.Bounds.Height / 2f;

                StringBuilder sb = new();
                if (!string.IsNullOrEmpty(prefix))
                {
                    sb.Append(prefix);
                    sb.Append('-');
                }
                sb.Append(tag.Name);

                if (!_animations.TryAdd(sb.ToString().ToLower(), animation))
                    throw new InvalidOperationException("duplicate animation key");
            }
        }

        public void Play(string animationTag, int loopCount = 0,
            Action<AnimatedSprite>? animationEndCallback = default)
        {
            _animationEndCallback = animationEndCallback;

            if (_currentAnimation != null && _currentAnimation.IsAnimating)
            {
                // return if animations are same
                if (animationTag == _currentAnimation.Name)
                    return;

                // we will stop if animation still in play
                _currentAnimation.Stop(); 
            }

            _currentAnimation = GetAnimation(animationTag);
            _currentAnimation.OnAnimationEnd = OnAnimationEndHandler;

            _currentAnimation.Play(loopCount);
        }

        private void OnAnimationEndHandler(AnimatedSprite animatedSprite)
        {
            _animationEndCallback?.Invoke(animatedSprite);
            _animationEndCallback = null;
            animatedSprite.OnAnimationEnd = null;
        }

        private Action<AnimatedSprite>? _animationEndCallback;

        public string CurrentAnimationName()
        {
            if (_currentAnimation == null) throw new InvalidOperationException();
            return _currentAnimation.Name;
        }

        public void Stop()
        {
            _currentAnimation?.Stop();
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

        private void SpriteRenderer_FetchingDrawRequests(List<DrawRequestBase> drawRequests)
        {
            if (_currentAnimation == null) return;

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

        AnimatedSprite? _currentAnimation;
        SpriteRendererComponent _renderer;

        ResourceManager _resource;
        Dictionary<string, AnimatedSprite> _animations = [];
        AsepriteFile _file;

        public override void Dispose()
        {
            base.Dispose();
            _renderer.DrawRequestsFetching -= SpriteRenderer_FetchingDrawRequests;
        }
    }
}