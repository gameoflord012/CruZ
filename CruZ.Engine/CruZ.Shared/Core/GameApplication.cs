using CruZ.Systems;
using CruZ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CruZ
{
    public partial class GameApplication : 
        IECSContextProvider, IInputContextProvider, IApplicationContextProvider, UIContext, IDisposable
    {
        public event Action<GameTime>   DrawUI;
        public event Action<GameTime>   UpdateUI;
        public event Action<GameTime>   ECSDraw;
        public event Action<GameTime>   ECSUpdate;
        public event Action             InitializeECSSystem;
        public event Action<GameTime>   InputUpdate;
        public event Action<Viewport>   WindowResize;
        public event Action             ExitEvent;

        public event Action             Initializing;
        public event Action             Initialized;

        public ContentManager       Content         { get => _core.Content; }
        public GraphicsDevice       GraphicsDevice  { get => _core.GraphicsDevice; }
        public GameWindow           Window          => _core.Window;

        private GameApplication()
        {
            _core = new();

            _core.Initializing      += InternalInitializing;
            _core.UpdateEvent       += InternalUpdate;
            _core.DrawEvent         += InternalDraw;
            _core.ExitEvent         += InternalOnExit;

            _core.LoadContentEvent  += OnLoadContent;
            _core.LateDrawEvent     += OnLateDraw;
            //_core.EndRunEvent       += OnEndRun;

            _core.Window.ClientSizeChanged += Window_ClientSizeChanged;

            ApplicationContext  .CreateContext(this);
            ECS                 .CreateContext(this);
            Input               .CreateContext(this);
            UIManager           .CreateContext(this);

            //_core.Run();
        }

        public void Run()
        {
            _core.Run();
        }

        public void Exit()
        {
            _core.Exit();
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

        private void InternalInitializing()
        {
            Camera.Main = new Camera(GraphicsDevice.Viewport);
            InitializeECSSystem?.Invoke();

            Initializing?.Invoke();
            OnInitialize();

            Initialized?.Invoke();
        }

        private void InternalOnExit(object? sender, EventArgs e)
        {
            ExitEvent?.Invoke();
            OnExit();
        }

        protected virtual void  OnInitialize() { }
        protected virtual void  OnUpdate(GameTime gameTime) { }
        protected virtual void  OnDraw(GameTime gameTime) { }
        protected virtual void  OnLateDraw(GameTime gameTime) { }
        protected virtual void  OnExit() { }
        protected virtual void  OnLoadContent() { }

        public void Dispose()
        {
            _core.Dispose();
        }

        //protected virtual void  OnEndRun() { }

        private GameCore _core;
    }

    public partial class GameApplication
    {
        public static GameApplication CreateContext()
        {
            return _instance = new GameApplication();
        }

        private static GameApplication? _instance;

        public static GameScene CreateScene()
        {
            return GameScene.Create(_instance);
        }
    }
}
