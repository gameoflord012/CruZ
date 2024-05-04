using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine
{
    /// <summary>
    /// Provides a wrapper for <see cref="Game"/>.
    /// </summary>
    public partial class GameWrapper : Game
    {
        public event Action? AfterInitialize;
        public event Action<GameTime>? BeforeUpdate;
        public event Action<GameTime>? AfterDraw;

        public GameWrapper()
        {
            _gdManager = new GraphicsDeviceManager(this);
            _gdManager.GraphicsProfile = GraphicsProfile.HiDef;
            IsMouseVisible = true;
        }

        protected sealed override void Initialize()
        {
            base.Initialize();
            OnInitialize();

            AfterInitialize?.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BeforeUpdate?.Invoke(gameTime);
            OnUpdated(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            OnDrawing(gameTime);
            AfterDraw?.Invoke(gameTime);
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnUpdated(GameTime gameTime) { }

        protected virtual void OnDrawing(GameTime gameTime) { }

        private GraphicsDeviceManager _gdManager;
    }
}
