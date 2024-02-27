using Microsoft.Xna.Framework.Graphics;
using System;

namespace CruZ
{
    using Microsoft.Xna.Framework;

    public partial class GameWrapper : XNA.Game
    {
        public event Action?                    Initializing;
        public event Action?                    InitializeSystemEvent;
        public event Action?                    LoadContentEvent;
        public event Action?                    EndRunEvent;
        public event Action<object, EventArgs>? ExitEvent;
        public event Action<GameTime>?          UpdateEvent;
        public event Action<GameTime>?          DrawEvent;
        public event Action<GameTime>?          LateDrawEvent;

        public GameWrapper()
        {
            Content.RootDirectory = ".";
            IsMouseVisible = true;

            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void EndRun()
        {
            base.EndRun();
            EndRunEvent?.Invoke();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            ExitEvent?.Invoke(sender, args);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            LoadContentEvent?.Invoke();
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitalizeSystem();
            Initializing?.Invoke();
        }

        private void InitalizeSystem()
        {
            InitializeSystemEvent?.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateEvent?.Invoke(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            InternalDraw(gameTime);
            base.Draw(gameTime);
        }

        private void InternalDraw(GameTime gameTime)
        {
            DrawEvent?.Invoke(gameTime);
            LateDrawEvent?.Invoke(gameTime);
        }

        public void ChangeWindowSize(int width, int height)
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        private GraphicsDeviceManager _graphics;
    }
}
