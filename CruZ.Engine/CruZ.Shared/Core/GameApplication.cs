using CruZ.Systems;
using CruZ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CruZ
{
    public class GameApplication : 
        IECSContextProvider, IInputContextProvider, IApplicationContextProvider, UIContext
    {
        public event Action<GameTime>   DrawUI;
        public event Action<GameTime>   UpdateUI;
        public event Action<GameTime>   ECSDraw;
        public event Action<GameTime>   ECSUpdate;
        public event Action             InitializeECSSystem;
        public event Action<GameTime>   InputUpdate;
        public event Action<Viewport>   WindowResize;

        public event Action             Initialize;

        public ContentManager       Content         { get => _core.Content; }
        public GraphicsDevice       GraphicsDevice  { get => _core.GraphicsDevice; }
        public GameWindow           Window          => _core.Window;

        public GameApplication()
        {
            _core = new();

            _core.InitializeEvent   += InternalInitialize;
            _core.LoadContentEvent  += OnLoadContent;
            _core.UpdateEvent       += InternalUpdate;
            _core.DrawEvent         += InternalDraw;
            _core.LateDrawEvent     += OnLateDraw;
            _core.EndRunEvent       += OnEndRun;
            _core.ExitEvent         += OnExit;

            _core.Window.ClientSizeChanged += Window_ClientSizeChanged;

            ApplicationContext  .SetContext(this);
            ECS                 .SetContext(this);
            Input               .SetContext(this);

            //_core.Run();
        }

        public void Run()
        {
            _core.Run();
        }

        private void Window_ClientSizeChanged(object? sender, EventArgs e)
        {
            WindowResize?.Invoke(_core.GraphicsDevice.Viewport);
        }

        private void InternalUpdate(GameTime gameTime)
        {
            InputUpdate?.Invoke(gameTime);
            ECSUpdate?.Invoke(gameTime);
            UpdateUI?.Invoke(gameTime);

            OnUpdate(gameTime);
        }

        private void InternalDraw(GameTime gameTime)
        {
            ECSDraw?.Invoke(gameTime);
            DrawUI?.Invoke(gameTime);
            OnDraw(gameTime);
        }

        private void InternalInitialize()
        {
            Camera.Main = new Camera(GraphicsDevice.Viewport);
            InitializeECSSystem?.Invoke();

            Initialize?.Invoke();
            OnInitialize();
        }

        protected virtual void  OnInitialize() { }
        protected virtual void  OnUpdate(GameTime gameTime) { }
        protected virtual void  OnDraw(GameTime gameTime) { }
        protected virtual void  OnLateDraw(GameTime gameTime) { }
        protected virtual void  OnExit(object sender, EventArgs args) { }
        protected virtual void  OnEndRun() { }
        protected virtual void  OnLoadContent() { }

        private GameCore _core;
    }
}
