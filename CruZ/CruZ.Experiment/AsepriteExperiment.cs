using System.Diagnostics;

using AsepriteDotNet.Aseprite;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Render;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Aseprite;

namespace CruZ.Experiment
{
    internal class AsepriteExperiment : GameWrapper
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();

            _sp = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var aseFile = GameApplication.Resource.Load<AsepriteFile>("NinjaAnim.aseprite");

            _spriteSheet = aseFile.CreateSpriteSheet(GraphicsDevice);
            _idle = _spriteSheet.CreateAnimatedSprite("walk-front");
        }

        protected override void Update(GameTime gameTime)
        {
            _idle.Update(gameTime);

            _lastState = _currentState;
            _currentState = Keyboard.GetState();

            if (_currentState.IsKeyDown(Keys.Space) && _lastState.IsKeyUp(Keys.Space))
            {
                _idle.Stop();
                _idle.Play(1);
                _isPlaying = true;
            }

            _idle.OnAnimationEnd = AnimationEndHandler;

            Debug.WriteLine($"_isPlaying {_isPlaying}");
        }

        private void AnimationEndHandler(AnimatedSprite sprite)
        {
            _isPlaying = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            _sp.Begin(samplerState: SamplerState.PointClamp);
            SpriteDrawArgs sprite = new();
            sprite.Apply(_idle);
            sprite.Scale = Vector2.One * 10;
            _sp.Draw(sprite);
            _sp.End();

        }

        SpriteBatch _sp;
        SpriteSheet _spriteSheet;

        KeyboardState _lastState;
        KeyboardState _currentState;

        AnimatedSprite _idle;

        bool _isPlaying = false;
    }
}
