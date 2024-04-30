using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;

namespace CruZ.GameEngine
{
    public partial class GameApplication : IDisposable
    {
        private GameApplication(GameWrapper core)
        {
            _core = core;
            _core.AfterInitialize += Wrapper_Initialized;
            _core.BeforeUpdate += Wrapper_BeforeUpdate;
            _core.AfterDraw += Wrapper_AfterDraw;
            _core.Exiting += Wrapper_Exiting;
            _core.Window.ClientSizeChanged += Wrapper_WindowResized;

            _ecs = ECSManager.CreateContext();
            _inputController = InputManager.CreateContext();
        }

        public void Run()
        {
            _core.Run();
        }

        public void Exit()
        {
            _core.Exit();
        }

        #region event handlers
        private void Wrapper_WindowResized(object? sender, EventArgs e)
        {
            WindowResized?.Invoke(_core.GraphicsDevice.Viewport);
        }

        private void Wrapper_BeforeUpdate(GameTime gameTime)
        {
            ProcessMarshalRequests();

            _inputController.Update(gameTime);
            _ecs.Update(gameTime);
        }

        private void Wrapper_AfterDraw(GameTime gameTime)
        {
            CalculateFps(gameTime);
            _ecs.Draw(gameTime);
            AfterDrawn?.Invoke();
        }

        private void Wrapper_Initialized()
        {
            _spriteBatch = new(GraphicsDevice);
            _core.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            Camera.Main = new Camera(Window);
            Camera.Main.PreserveScreenRatio = true;

            _ecs.Initialize();
            Initialized?.Invoke();
        }

        private void Wrapper_Exiting(object? sender, EventArgs e)
        {
            Exiting?.Invoke();
        }
        #endregion

        private void ProcessMarshalRequests()
        {
            foreach (var invoke in _marshalRequests)
            {
                invoke.Invoke();
            }

            _marshalRequests.Clear();
        }

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

                LogManager.SetMsg($"Fps: {_fpsResult}", "Fps");
            }
        }

        public void Dispose()
        {
            if (!_isDispose)
            {
                _isDispose = true;
                _core.Dispose();
                _spriteBatch.Dispose();
                _ecs.Dispose();
                Disposables.ForEach(e => e.Dispose());
                Disposables.Clear();

                WindowResized = null;
                Initialized = null;
                Exiting = null;
                AfterDrawn = null;
            }
        }

        #region variables
        public ContentManager Content { get => _core.Content; }
        public GraphicsDevice GraphicsDevice { get => _core.GraphicsDevice; }
        public GameWindow Window => _core.Window;
        public int FpsResult { get => _fpsResult; }

        ECSManager _ecs;
        IInputController _inputController;
        GameWrapper _core;
        SpriteBatch _spriteBatch;

        bool _isDispose = false;
        int _fpsResult = 0;
        int _frameCount = 0;
        float _fpsTimer = 0;

        List<Action> _marshalRequests = [];
        #endregion
    }

    // Static members
    public partial class GameApplication
    {
        // Remembers set events to null in _instance.Dispose()
        public static event Action<Viewport>? WindowResized;
        public static event Action? Initialized;
        public static event Action? Exiting;
        public static event Action? AfterDrawn;

        public static GraphicsDevice GetGraphicsDevice() => _instance.GraphicsDevice;

        internal static AutoResizeRenderTarget CreateRenderTarget()
        {
            var rt = new AutoResizeRenderTarget(_instance.GraphicsDevice, _instance.Window);
            Disposables.Add(rt);
            return rt;
        }

        /// <summary>
        /// Whether the Game Window is active
        /// </summary>
        /// <returns></returns>
        public static bool IsActive()
        {
            return _instance._core.IsActive;
        }

        public static GameApplication CreateContext(GameWrapper core)
        {
            if (_instance != null && !_instance._isDispose)
                throw new InvalidOperationException("Dispose needed before creating new context");

            return _instance = new GameApplication(core);
        }

        public static ContentManager GetContentManager()
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

        public static List<IDisposable> Disposables = [];
        private static GameApplication? _instance;
    }
}
