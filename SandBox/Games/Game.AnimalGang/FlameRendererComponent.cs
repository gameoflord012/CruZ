using System;

using CruZ.Framework;
using CruZ.Framework.GameSystem.Render;
using CruZ.Framework.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.AnimalGang.DesktopGL
{
    public class FlameRendererComponent : RendererComponent
    {
        public FlameRendererComponent()
        {
            _fx = GameContext.GameResource.Load<Effect>("shaders\\flame-shader.fx");
            _tex = GameContext.GameResource.Load<Texture2D>("imgs\\GAP\\Flame01.png");
            _gd = GameApplication.GetGraphicsDevice();

            _bloom.BloomPreset = BloomFilter.BloomPresets.Wide;
            _bloom.BloomUseLuminance = false;
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            UpdateResolution();

            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);

            _gd.SetRenderTarget(_renderTarget);
            _gd.Clear(Color.Transparent);

            _fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            _fx.Parameters["flame_color"].SetValue(_flameColor);
            _fx.Parameters["exposure"].SetValue(Exposure);
            spriteBatch.Begin(effect: _fx, sortMode: SpriteSortMode.Immediate);
            drawArgs.Apply(_tex);
            spriteBatch.Draw(drawArgs);
            spriteBatch.End();

            if(!BloomDisable)
            {
                var filtered = _bloom.Draw(_renderTarget, _renderTarget.Width, _renderTarget.Height);
                spriteBatch.Begin(SpriteSortMode.Immediate, blendState: BlendState.Additive);
                spriteBatch.Draw(filtered, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
            
            _gd.SetRenderTarget(null);
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void UpdateResolution()
        {
            if(_renderTarget == null || _gd.Viewport.Width != _renderTarget.Width || _gd.Viewport.Height != _renderTarget.Height)
            _renderTarget = new RenderTarget2D(_gd, _gd.Viewport.Width, _gd.Viewport.Height,
                false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }

        public Vector4 FlameColor
        {
            get => _flameColor;
            set => _flameColor = value;
        }

        public float Exposure
        {
            get;
            set;
        }

        public float Threshold
        {
            get => _bloom.BloomThreshold;
            set => _bloom.BloomThreshold = MathHelper.Clamp(value, 0f, 1f);
        }

        public float StreakLength
        {
            get => _bloom.BloomStreakLength;
            set => _bloom.BloomStreakLength = value;
        }

        public float StrengthMultiplier
        {
            get => _bloom.BloomStrengthMultiplier;
            set => _bloom.BloomStrengthMultiplier = value;
        }

        public bool BloomDisable
        { 
            get;
            set;
        }

        public bool TexDisable
        {
            get;
            set;
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _renderTarget.Dispose();
            _fx.Dispose();
            _bloom.Dispose();
        }

        Effect _fx;
        Texture2D _tex;
        RenderTarget2D _renderTarget;
        GraphicsDevice _gd;

        Vector4 _flameColor = new(1, 1, 1, 1);
    }
}
