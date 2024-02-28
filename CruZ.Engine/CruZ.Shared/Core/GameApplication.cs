using CruZ.Service;
using CruZ.Systems;
using CruZ.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

using System;
using System.Collections.Generic;

namespace CruZ
{
    public partial class GameApplication :
        IECSContextProvider, IInputContextProvider, UIContext, IDisposable
    {
        #region Events
        public event Action<GameTime> DrawUI;
        public event Action<GameTime> UpdateUI;
        public event Action InitializeUI;

        public event Action<GameTime> ECSDraw;
        public event Action<GameTime> ECSUpdate;
        public event Action InitializeECSSystem;

        public event Action<GameTime> InputUpdate;
        public event Action<Viewport> WindowResize;
        public event Action ExitEvent;
        public event Action<DrawEventArgs> EarlyDraw;
        public event Action<GameTime> Draw;
        public event Action<DrawEventArgs> LateDraw;

        public event Action Initializing;
        public event Action Initialized;
        #endregion

        #region Properties
        public ContentManager Content { get => _core.Content; }
        public GraphicsDevice GraphicsDevice { get => _core.GraphicsDevice; }
        public GameWindow Window => _core.Window;
        public bool ExitCalled { get => _exitCalled; }
        public bool IsInitialized { get; private set; } = false;
        public int FpsResult { get => _fpsResult; } 
        #endregion

        private GameApplication()
        {
            _core = new();

            _core.Content.RootDirectory = ".";
            _core.IsMouseVisible = true;

            _core.Initializing += InternalInitializing;
            _core.UpdateEvent += InternalUpdate;
            _core.DrawEvent += InternalDraw;
            _core.ExitEvent += InternalExit;
            _core.Window.ClientSizeChanged += Window_ClientSizeChanged;

            ECS.CreateContext(this);
            Input.CreateContext(this);
            UIManager.CreateContext(this);
        }

        public void Run()
        {
            _core.Run();
        }

        public void Exit()
        {
            lock (this)
            {
                if (!_exitCalled)
                    _core.Exit();
            }
        }

        private void Window_ClientSizeChanged(object? sender, EventArgs e)
        {
            WindowResize?.Invoke(_core.GraphicsDevice.Viewport);
        }
        private void ProcessMarshalRequests()
        {
            foreach (var invoke in _marshalRequests)
            {
                invoke.Invoke();
            }

            _marshalRequests.Clear();
        }

        #region Internals
        private void InternalUpdate(GameTime gameTime)
        {
            ProcessMarshalRequests();

            InputUpdate?.Invoke(gameTime);
            ECSUpdate?.Invoke(gameTime);
            UpdateUI?.Invoke(gameTime);
        }

        private void InternalDraw(GameTime gameTime)
        {
            CalculateFps(gameTime);

            GraphicsDevice.Clear(Color.Beige);

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

        private void InternalExit(object? sender, EventArgs e)
        {
            _exitCalled = true;

            ExitEvent?.Invoke();
            Dispose();
        } 
        #endregion

        protected virtual void OnInitialize() { }
        protected virtual void OnDraw(GameTime gameTime) { }

        private void CalculateFps(GameTime gameTime)
        {
            _fpsTimer += gameTime.GetElapsedSeconds();
            _frameCount++;

            int seconds = 0;
            while (_fpsTimer > 1)
            {
                seconds++;
                _fpsTimer -= 1;
            }

            if (seconds > 0)
            {
                _fpsResult = _frameCount / seconds;
                _frameCount = 0;

                LogService.SetMsg($"Fps: {_fpsResult}", "Fps");
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

        #region Privates
        private GameWrapper _core;
        private SpriteBatch _spriteBatch;
        bool disposed = false;
        bool _exitCalled = false;

        int _fpsResult = 0;
        int _frameCount = 0;
        float _fpsTimer = 0;

        List<Action> _marshalRequests = []; 
        #endregion
    }

    // Static members
    public partial class GameApplication
    {
        public static Viewport Viewport => _instance.GraphicsDevice.Viewport;

        public static GraphicsDevice GetGraphicsDevice() => _instance.GraphicsDevice;

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

        /// <summary>
        /// Whether the Game Window is active
        /// </summary>
        /// <returns></returns>
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

        public static void MarshalInvoke(Action action)
        {
            lock (_instance)
            {
                _instance._marshalRequests.Add(action);
            }
        }

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
