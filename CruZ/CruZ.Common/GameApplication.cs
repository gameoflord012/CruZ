using CruZ.Common.Service;

using CruZ.Common.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

using System;
using System.Collections.Generic;
using CruZ.Common.Scene;
using CruZ.Common.ECS;
using CruZ.Common.Input;

namespace CruZ.Common
{
    public partial class GameApplication : IDisposable
    {
        #region Properties
        public ContentManager Content { get => _core.Content; }
        public GraphicsDevice GraphicsDevice { get => _core.GraphicsDevice; }
        public GameWindow Window => _core.Window;
        public bool ExitCalled { get => _exitCalled; }
        public int FpsResult { get => _fpsResult; }
        #endregion

        private GameApplication(GameWrapper core)
        {
            _core = core;
            _core.Initialized += Wrapper_Initialized;
            _core.BeforeUpdate += Wrapper_BeforeUpdate;
            _core.AfterDraw += Wrapper_AfterDraw;
            _core.Window.ClientSizeChanged += Wrapper_WindowResized;

            _ecsController = ECSManager.CreateContext();
            _inputController = InputManager.CreateContext();
            _uiController = UIManager.CreateContext();
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

        #region Event Handlers
        private void Wrapper_WindowResized(object? sender, EventArgs e)
        {
            WindowResized?.Invoke(_core.GraphicsDevice.Viewport);
        }

        private void Wrapper_BeforeUpdate(GameTime gameTime)
        {
            ProcessMarshalRequests();

            _inputController.Update(gameTime);
            _ecsController.Update(gameTime);
            _uiController.Update(gameTime);
        }

        private void Wrapper_AfterDraw(GameTime gameTime)
        {
            CalculateFps(gameTime);
            _ecsController.Draw(gameTime);
            _uiController.Draw(gameTime, _spriteBatch);
        }

        private void Wrapper_Initialized()
        {
            _spriteBatch = new(GraphicsDevice);
            Camera.Main = new Camera(GraphicsDevice.Viewport);

            _ecsController.Initialize();
            _uiController.Initialize();
            Initialized?.Invoke();
        }

        private void Wrapper_Exit(object? sender, EventArgs e)
        {
            _exitCalled = true;
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

                WindowResized = null;
                Initialized = null;
                Exiting = null;
                Drawing = null;
            }
        }

        #region Private Variables
        IECSController _ecsController;
        IInputController _inputController;
        IUIController _uiController;
        GameWrapper _core;
        SpriteBatch _spriteBatch;

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
        // Remembers set events to null in _instance.Dispose()
        public static event Action<Viewport>? WindowResized;
        public static event Action? Initialized;
        public static event Action? Exiting;
        public static event Action? Drawing;

        public static GraphicsDevice GetGraphicsDevice() => _instance.GraphicsDevice;

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
            if(_instance != null) _instance.Dispose();
            return _instance = new GameApplication(core);
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
