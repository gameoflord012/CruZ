using System;

using Microsoft.Xna.Framework;

namespace CruZ.Common
{
    /// <summary>
    /// Provides a wrapper for <see cref="Game"/>.
    /// </summary>
    public partial class GameWrapper : Game
    {
        public event Action Initialized;
        public event Action<GameTime>? BeforeUpdate;
        public event Action<GameTime>? AfterDraw;

        //public GraphicsDeviceManager GraphicsDeviceManager => _gdManager;

        public GameWrapper()
        {
            _gdManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = ".";
            IsMouseVisible = true;
        }

        protected sealed override void Initialize()
        {
            base.Initialize();

            OnInitialize();
            Initialized?.Invoke();
        }

        protected sealed override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            BeforeUpdate?.Invoke(gameTime);
            OnUpdate(gameTime);
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            OnDraw(gameTime);
            AfterDraw?.Invoke(gameTime);
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnUpdate(GameTime gameTime) { }

        protected virtual void OnDraw(GameTime gameTime) { }

        private GraphicsDeviceManager _gdManager;
    }
}
