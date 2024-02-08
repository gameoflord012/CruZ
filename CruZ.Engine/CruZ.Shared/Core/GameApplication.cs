using CruZ.Systems;
using CruZ.UI;
using CruZ.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace CruZ
{
    public partial class GameApplication :
        IECSContextProvider, IInputContextProvider, UIContext, IDisposable
    {
        public event Action<GameTime>   DrawUI;
        public event Action<GameTime>   UpdateUI;
        public event Action             InitializeUI;

        public event Action<GameTime> ECSDraw;
        public event Action<GameTime> ECSUpdate;
        public event Action InitializeECSSystem;

        public event Action<GameTime>       InputUpdate;
        public event Action<Viewport>       WindowResize;
        public event Action                 ExitEvent;
        public event Action<DrawEventArgs>  EarlyDraw;
        public event Action<GameTime>       Draw;
        public event Action<DrawEventArgs>  LateDraw;

        public event Action Initializing;
        public event Action Initialized;

        public ContentManager Content { get => _core.Content; }
        public GraphicsDevice GraphicsDevice { get => _core.GraphicsDevice; }
        public GameWindow Window => _core.Window;
        public bool ExitCalled { get => _exitCalled; }
        public bool IsInitialized { get; private set; } = false;
        public int FpsResult { get => _fpsResult; }

        private GameApplication()
        {
            _core = new();

            _core.Initializing += InternalInitializing;
            _core.UpdateEvent += InternalUpdate;
            _core.DrawEvent += InternalDraw;
            _core.ExitEvent += InternalOnExit;

            _core.LoadContentEvent += OnLoadContent;
            _core.LateDrawEvent += OnLateDraw;
            //_core.EndRunEvent       += OnEndRun;

            _core.Window.ClientSizeChanged += Window_ClientSizeChanged;

            ECS.CreateContext(this);
            Input.CreateContext(this);
            UIManager.CreateContext(this);

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
            CalculateFps(gameTime);

            var drawArgs = new DrawEventArgs(_spriteBatch, gameTime);
            EarlyDraw?.Invoke(drawArgs);
            Draw?.Invoke(gameTime);
            ECSDraw?.Invoke(gameTime);
            LateDraw?.Invoke(drawArgs);
            DrawUI?.Invoke(gameTime);
            OnDraw(gameTime);

        }

        private void InternalInitializing()
        {
            IsInitialized = true;

            _spriteBatch = new(GraphicsDevice);
            Camera.Main = new Camera(GraphicsDevice.Viewport);

            InitializeECSSystem?.Invoke();
            InitializeUI?.Invoke();
            Initializing?.Invoke();

            OnInitialize();
            Initialized?.Invoke();
        }

        private void InternalOnExit(object? sender, EventArgs e)
        {
            _exitCalled = true;
            ExitEvent?.Invoke();
            OnExit();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }
        protected virtual void OnLateDraw(GameTime gameTime) { }
        protected virtual void OnExit() { }
        protected virtual void OnLoadContent() { }

        private void CalculateFps(GameTime gameTime)
        {
            _fpsTimer += gameTime.GetElapsedSeconds();
            _frameCount++;

            int seconds = 0;
            while(_fpsTimer > 1)
            { 
                seconds++;
                _fpsTimer -= 1;
            }

            if(seconds > 0)
            {
                _fpsResult = _frameCount / seconds;
                _frameCount = 0;

                Logging.SetMsg($"Fps: {_fpsResult}", "Fps");
            }
        }
        
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                _core.Dispose();
                _spriteBatch.Dispose();
            }
        }

        //protected virtual void  OnEndRun() { }

        private GameCore _core;
        private SpriteBatch _spriteBatch;
        bool disposed = false;
        bool _exitCalled = false;

        int _fpsResult = 0;
        int _frameCount = 0;
        float _fpsTimer = 0;
    }

    public partial class GameApplication
    {
        public static Viewport Viewport => _instance.GraphicsDevice.Viewport;

        public static void RegisterWindowResize(Action<Viewport> windowResize)
        {
            _instance.WindowResize += windowResize;
        }

        public static void RegisterDraw(Action<GameTime> draw)
        {
            _instance.Draw += draw;
        }

        public static void UnregisterDraw(Action<GameTime> draw)
        {
            _instance.Draw -= draw;
        }

        public static bool IsActive()
        {
            return _instance._core.IsActive;
        }

        public static GameApplication CreateContext()
        {
            return _instance = new GameApplication();
        }

        public static GameScene CreateScene()
        {
            return GameScene.Create(_instance);
        }

        public static SpriteBatch GetSpriteBatch()
        {
            return _instance._spriteBatch;
        }

        public static ContentManager GetContent()
        {
            return _instance.Content;
        }

        public static int GetFpsResult() => _instance.FpsResult;

        private static GameApplication? _instance;
    }

    public class DrawEventArgs
    {
        public SpriteBatch SpriteBatch;

        public DrawEventArgs(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteBatch = spriteBatch;
            GameTime = gameTime;
        }

        public GameTime GameTime;

    }
}
