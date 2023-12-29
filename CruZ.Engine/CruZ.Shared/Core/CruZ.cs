using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace CruZ_Engine
{
    public delegate void CruZ_UpdateDelegate(GameTime gameTime);
    public delegate void ActionDelegate();
    public delegate void OnExitingDelegate(object sender, EventArgs args);

    public partial class CruZ : Game
    {
        private CruZ()
        {
            Content.RootDirectory = CONTENT_ROOT;
            IsMouseVisible = true;

            _input = new CruZ_Input(this);
            _ecs = new CruZ_ECS(this);
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void EndRun()
        {
            base.EndRun();
            OnEndRun?.Invoke();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            OnExit.Invoke(sender, args);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            OnLoadContent.Invoke();
        }

        protected override void Initialize()
        {
            base.Initialize();
            OnInitialize?.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            OnUpdate?.Invoke(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Gray);
            OnDraw?.Invoke(gameTime);
        }

        public event ActionDelegate OnInitialize;
        public event ActionDelegate OnLoadContent;
        public event ActionDelegate OnEndRun;
        public event OnExitingDelegate OnExit;
        public event CruZ_UpdateDelegate OnUpdate;
        public event CruZ_UpdateDelegate OnDraw;

        public ViewportAdapter ViewportAdapter { get => _viewportAdapter; set => _viewportAdapter = value; }
        public CruZ_Input Input { get => _input; }
        public World World { get => _ecs.World; }

        private CruZ_ECS _ecs;
        private CruZ_Input _input;
        private GraphicsDeviceManager _graphics;
        private ViewportAdapter _viewportAdapter;
    }
}
