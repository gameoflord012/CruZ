﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Input;
using CruZ.GameEngine.Resource;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine
{
    public partial class GameApplication : IDisposable
    {
        private GameApplication(GameWrapper core, string gameResourceDir)
        {
            _core = core;
            _core.AfterInitialize += Wrapper_Initialized;
            _core.BeforeUpdate += Wrapper_BeforeUpdate;
            _core.AfterDraw += Wrapper_AfterDraw;
            _core.Exiting += Wrapper_Exiting;
            _core.Window.ClientSizeChanged += Wrapper_WindowResized;

            _ecs = ECSManager.CreateContext();
            _inputController = InputManager.CreateContext();

            _gameResourceDir = gameResourceDir;
            _gameResource = ResourceManager.From(_gameResourceDir);
            _internalResource = ResourceManager.From(Path.Combine(_gameResourceDir, ".internal"));

            InitializeInternalResource();
        }
        private void InitializeInternalResource()
        {
            _gameResource!.CopyResourceFolder(
                ResourceManager.From(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\Internal")),
                ".internal");
        }

        public void Run()
        {
            _core.Run();
        }

        public void Exit()
        {
            _core.Exit();
        }

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
            AfterDraw?.Invoke();
        }

        private void Wrapper_Initialized()
        {
            _spriteBatch = new(GraphicsDevice);

            Camera.Main = new Camera(Window);
            Camera.Main.PreserveScreenRatio = true;

            _ecs.Initialize();
            Initialized?.Invoke();
        }

        private void Wrapper_Exiting(object? sender, EventArgs e)
        {
            Exiting?.Invoke();
        }

        private void ProcessMarshalRequests()
        {
            foreach(var request in _marshalRequests)
            {
                request.Action.Invoke();
                request.ResetEvent.Set();
            }

            _marshalRequests.Clear();
        }

        public void MarshalInvoke(Action action)
        {
            ManualResetEvent resetEvent;

            lock(this)
            {
                resetEvent = new ManualResetEvent(false);
                _marshalRequests.Add(new MarshalRequest(action, resetEvent));
            }

            resetEvent.WaitOne();
        }

        private record MarshalRequest(Action Action, ManualResetEvent ResetEvent);

        private readonly List<MarshalRequest> _marshalRequests = [];

        private void CalculateFps(GameTime gameTime)
        {
            _fpsTimer += gameTime.DeltaTime();
            _frameCount++;

            int seconds = 0;
            while(_fpsTimer > 1)
            {
                seconds++;
                _fpsTimer -= 1;
            }

            if(seconds > 0)
            {
                FpsResult = _frameCount / seconds;
                _frameCount = 0;

                LogManager.SetMsg($"Fps: {FpsResult}", "Fps");
            }
        }

        public void Dispose()
        {
            if(!_isDispose)
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
                AfterDraw = null;
            }
        }

        #region variables
        public ContentManager Content { get => _core.Content; }
        public GraphicsDevice GraphicsDevice { get => _core.GraphicsDevice; }
        public GameWindow Window => _core.Window;
        public int FpsResult { get; private set; } = 0;

        private readonly ECSManager _ecs;
        private readonly IInputController _inputController;
        private readonly GameWrapper _core;
        private SpriteBatch _spriteBatch = null!;
        private int _frameCount = 0;
        private float _fpsTimer = 0;
        private readonly string? _gameResourceDir;
        private readonly ResourceManager? _gameResource;
        private readonly ResourceManager? _internalResource;
        private bool _isDispose = false;
        #endregion
    }

    // Static members
    public partial class GameApplication
    {
        // Remembers set events to null in _instance.Dispose()
        public static event Action<Viewport>? WindowResized;
        public static event Action? Initialized;
        public static event Action? Exiting;
        public static event Action? AfterDraw;

        public static GraphicsDevice GetGraphicsDevice() => Instance.GraphicsDevice;

        internal static AutoResizeRenderTarget CreateRenderTarget()
        {
            var rt = new AutoResizeRenderTarget(Instance.GraphicsDevice, Instance.Window);
            Disposables.Add(rt);
            return rt;
        }

        /// <summary>
        /// Whether the Game Window is active
        /// </summary>
        /// <returns></returns>
        public static GameApplication CreateContext(GameWrapper core, string gameResourceDir)
        {
            if(_instance != null && !_instance._isDispose)
                throw new InvalidOperationException("Dispose needed before creating new context");

            return _instance = new GameApplication(core, gameResourceDir);
        }

        private static GameApplication Instance => CheckNull(_instance);

        public static bool IsActive()
        {
            return Instance._core.IsActive;
        }

        public static ContentManager GetContentManager()
        {
            return Instance.Content;
        }

        public static string GameResourceDir
        {
            get => CheckNull(Instance._gameResourceDir);
        }

        public static ResourceManager Resource
        {
            get => CheckNull(Instance._gameResource);
        }

        public static ResourceManager InternalResource
        {
            get => CheckNull(Instance._internalResource);
        }

        public static Func<AssemblyName, Assembly?> AssemblyResolver
        {
            get => (resolvingAss) => AppDomain.CurrentDomain.GetAssemblies()
                    .First(domainAss => domainAss.FullName == resolvingAss.FullName);
        }

        public static List<IDisposable> Disposables = [];
        private static GameApplication? _instance;

        private static T CheckNull<T>(T? value)
        {
            return value ?? throw new InvalidOperationException("Set value first");
        }
    }
}
