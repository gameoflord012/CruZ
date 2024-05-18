using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Scene;
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
        private record MarshalRequest(Action Action, ManualResetEvent ResetEvent, bool ForwardException);

        private GameApplication(GameWrapper core, string gameResourceDir)
        {
            _threadId = Environment.CurrentManagedThreadId;
            _marshalRequests = [];
            _disposables = [];

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
            foreach(var request in _marshalRequests.ToImmutableArray())
            {
                _marshalRequests.Remove(request);

                try
                {
                    request.Action.Invoke();
                    _marshalResult = new MarshalInvokeResult(null);
                }
                catch(Exception e)
                {
                    if(request.ForwardException)
                    {
                        _marshalResult = new MarshalInvokeResult(e);
                    }
                    else
                    {
                        _marshalResult = null;
                        throw;
                    }
                }

                request.ResetEvent.Set();
            }
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

            _mainCamera = new Camera(Window);
            Camera.Current = _mainCamera;
            Camera.Current.PreserveScreenRatio = true;

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

        private void GenerateMarshalRequest(Action action, bool forwardException, out MarshalInvokeResult invokeResult)
        {
            ManualResetEvent resetEvent;

            lock(this)
            {
                resetEvent = new ManualResetEvent(false);
                _marshalRequests.Add(new MarshalRequest(action, resetEvent, forwardException));
            }

            resetEvent.WaitOne();

            Trace.Assert(_marshalResult != null);
            invokeResult = _marshalResult;
            _marshalResult = null;
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

        private List<IDisposable> _disposables;
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
        private MarshalInvokeResult? _marshalResult;
        private bool _isDisposed;
        private Camera _mainCamera;
        private int _threadId;

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _isDisposed = true;
                _wrapper.Dispose();
                _spriteBatch.Dispose();
                _ecs.Dispose();
                _disposables.ForEach(e => e.Dispose());
                _disposables.Clear();

                WindowResized = null;
                Initialized = null;
                Exiting = null;
                AfterDraw = null;
            }
        }
    }
}
