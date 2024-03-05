using System;

namespace CruZ
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Redirect events or overriden function, this shouldn't include any logic other than that.
    /// </summary>
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

        //public GraphicsDeviceManager GraphicsDeviceManager => _gdManager;

        public GameWrapper()
        {
            _gdManager = new GraphicsDeviceManager(this);
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

        private GraphicsDeviceManager _gdManager;
    }
}
