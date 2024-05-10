using System;
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
            _marshalRequests = [];

            _wrapper = core;
            _wrapper.AfterInitialize += Wrapper_Initialized;
            _wrapper.BeforeUpdate += Wrapper_BeforeUpdate;
            _wrapper.AfterDraw += Wrapper_AfterDraw;
            _wrapper.Exiting += Wrapper_Exiting;
            _wrapper.Window.ClientSizeChanged += Wrapper_WindowResized;

            _ecs = ECSManager.CreateContext();
            _input = GameInput.CreateContext();

            _gameResourceDir = gameResourceDir;
            _gameResource = ResourceManager.From(_gameResourceDir);
            _internalResource = ResourceManager.From(Path.Combine(_gameResourceDir, ".internal"));

            InitInternalResource();
        }

        private void InitInternalResource()
        {
            _gameResource!.CopyResourceFolder(
                ResourceManager.From(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\Internal")),
                ".internal");
        }

        public void Run()
        {
            _wrapper.Run();
        }

        public void Exit()
        {
            _wrapper.Exit();
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

        private void Wrapper_WindowResized(object? sender, EventArgs e)
        {
            WindowResized?.Invoke(_wrapper.GraphicsDevice.Viewport);
        }

        private void Wrapper_BeforeUpdate(GameTime gameTime)
        {
            ProcessMarshalRequests();

            _input.Update(gameTime);

            if(!Pause)
            {
                _ecs.Update(gameTime);
            }
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

        public ContentManager Content
        { get => _wrapper.Content; }

        public GraphicsDevice GraphicsDevice
        { get => _wrapper.GraphicsDevice; }

        public GameWindow Window
        {
            get => _wrapper.Window;
        }

        public int FpsResult
        { get; private set; }

        public bool Pause
        {
            get;
            set;
        }

        private ECSManager _ecs;
        private GameInput _input;
        private GameWrapper _wrapper;
        private SpriteBatch _spriteBatch;
        private int _frameCount;
        private float _fpsTimer;
        private string? _gameResourceDir;
        private ResourceManager? _gameResource;
        private ResourceManager? _internalResource;
        private List<MarshalRequest> _marshalRequests;
        private bool _isDisposed;

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _isDisposed = true;
                _wrapper.Dispose();
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
    }

    public partial class GameApplication
    {
        public static event Action<Viewport>? WindowResized;
        public static event Action? Initialized;
        public static event Action? Exiting;
        public static event Action? AfterDraw;

        public static GameApplication CreateContext(GameWrapper core, string gameResourceDir)
        {
            if(_instance != null && !_instance._isDisposed)
                throw new InvalidOperationException("Dispose needed before creating new context");

            return _instance = new GameApplication(core, gameResourceDir);
        }

        public static GraphicsDevice GetGraphicsDevice()
        {
            return Instance.GraphicsDevice;
        }

        internal static AutoResizeRenderTarget CreateRenderTarget()
        {
            var rt = new AutoResizeRenderTarget(Instance.GraphicsDevice, Instance.Window);
            Disposables.Add(rt);
            return rt;
        }

        private static T CheckNull<T>(T? value)
        {
            return value ?? throw new InvalidOperationException("Set value first");
        }

        public static bool IsActive()
        {
            return Instance._wrapper.IsActive;
        }

        public static ContentManager GetContentManager()
        {
            return Instance.Content;
        }

        private static GameApplication Instance
        {
            get => CheckNull(_instance);
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

    }
}
