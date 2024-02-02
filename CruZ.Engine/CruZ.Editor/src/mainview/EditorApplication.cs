using CruZ.Components;
using CruZ.Editor.UI;
using CruZ.Resource;
using CruZ.Systems;
using CruZ.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public partial class EditorApplication
    {
        //public event Action<GameTime>   ECSDraw;
        //public event Action<GameTime>   ECSUpdate;

        //public event Action<GameTime>   UpdateUI;
        //public event Action<GameTime>   DrawUI;

        //public event Action<GameTime>   InputUpdate;
        //public event Action             InitializeECSSystem;


        //public GraphicsDevice   GraphicsDevice      => Editor.GraphicsDevice;
        //public ContentManager   Content             => Editor.Content;
        //public SpriteBatch      SpriteBatch         => Editor.spriteBatch;

        public event EventHandler<GameScene> SceneLoadEvent;
        public event EventHandler<TransformEntity> OnSelectedEntityChanged;

        public GameScene? CurrentGameScene => _currentScene;

        public EditorApplication()
        {
            //ECS                 .CreateContext(this);
            //ApplicationContext  .CreateContext(this);
            //Input               .CreateContext(this);
            //UIManager           .CreateContext(this);

            Input.MouseScroll += Input_MouseScroll;
            Input.MouseMove += Input_MouseMove;
            Input.MouseDown += Input_MouseDown;
            Input.MouseUp += Input_MouseUp;

            CacheService.Register(this);
            UpdateCache?.Invoke(this);
        }

        #region COMMENT
        //protected void Initializing()
        //{
        //    //_gameLoopTimer = new();
        //    //_gameLoopTimer.Start();

        //    //_drawElapsed = _gameLoopTimer.Elapsed;
        //    //_updateElapsed = _gameLoopTimer.Elapsed;

        //    //_updateTimer = new();
        //    //_updateTimer.SynchronizingObject = this;
        //    //_updateTimer.Interval = 1f / GlobalVariables.TARGET_FPS * 1000;
        //    //_updateTimer.Enabled = true;
        //    //_updateTimer.Elapsed += OnUpdate;

        //}

        //private void OnUpdate(object? sender, ElapsedEventArgs e)
        //{
        //    //GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _updateElapsed);
        //    //_updateElapsed = _gameLoopTimer.Elapsed;

        //    //Editor.FPSCounter.OnUpdate(gameTime);

        //    //InputUpdate?   .Invoke(gameTime);
        //    //ECSUpdate?        .Invoke(gameTime);
        //    //UpdateUI            .Invoke(gameTime);

        //    //Invalidate();
        //}

        //protected void OnDraw()
        //{
        //    //GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _drawElapsed);
        //    //_drawElapsed = _gameLoopTimer.Elapsed;

        //    ////Editor.FPSCounter.UpdateFrameCounter();

        //    //ECSDraw?.Invoke(gameTime);
        //    //DrawUI?.Invoke(gameTime);

        //    //DrawAxis();
        //}
        #endregion

        #region PUBLIC_FUNCS
        public void UnloadCurrentScene()
        {
            if (_currentScene == null) return;

            _currentScene.SetActive(false);
            //_currentScene.Dispose();
        }

        public void SelectEntity(TransformEntity e)
        {
            if (e == _currentSelectedEntity) return;

            _currentSelectedEntity = e;
            OnSelectedEntityChanged?.Invoke(this, _currentSelectedEntity);
        }

        public void LoadSceneFromFile(string file)
        {
            Check_AppInitialized();

            var scene = ResourceManager.LoadResource<GameScene>(file);
            LoadScene(scene);
        }

        public void ExitApp()
        {
            if (_gameApp != null)
            {
                if (!_gameApp.ExitCalled)
                    _gameApp.Exit();
            }

            if (_gameAppThread != null)
                if (!_gameAppThread.Join(5000))
                    throw new System.Exception("Can't exit editor app");

            CleanSession();
        }

        #endregion

        #region EVENT_HANDLER

        private void Game_WindowResize(Viewport viewport)
        {
            GetMainCamera().ViewPortWidth = viewport.Width;
            GetMainCamera().ViewPortHeight = viewport.Height;
        }

        private void Input_MouseScroll(InputInfo info)
        {
            Camera.Main.Zoom = new(Camera.Main.Zoom.X - info.SrollDelta * 0.001f * Camera.Main.Zoom.X, Camera.Main.Zoom.Y);
        }

        private void Input_MouseMove(InputInfo info)
        {
            if (_isMouseDraggingCamera)
            {
                var scale = Camera.Main.ScreenToWorldScale();
                var delt = new Vector3(
                    (info.CurMouse.Position.X - _mouseStartDragPoint.X) * scale.X,
                    (info.CurMouse.Position.Y - _mouseStartDragPoint.Y) * scale.Y);

                Camera.Main.Position = _cameraStartDragCoord - delt;
            }
        }

        private void Input_MouseDown(InputInfo info)
        {
            if (info.CurMouse.MiddleButton == XNA.Input.ButtonState.Pressed
                && !_isMouseDraggingCamera)
            {
                _isMouseDraggingCamera = true;
                _mouseStartDragPoint = info.CurMouse.Position;
                _cameraStartDragCoord = Camera.Main.Position;
            }
        }

        private void Input_MouseUp(InputInfo info)
        {
            if (info.CurMouse.MiddleButton == XNA.Input.ButtonState.Released)
            {
                _isMouseDraggingCamera = false;
            }
        }

        private void GameApp_Intialized()
        {
            Camera.Main = GetMainCamera();

            _appInitalized_Reset.Set();
        }

        private void GameApp_Exit()
        {
            UnloadCurrentScene();
            CleanSession();
        }

        #endregion

        private void CleanSession()
        {
            _gameApp?.Dispose();

            _gameApp = null;
            _gameAppThread = null;
        }

        private void Check_AppInitialized()
        {
            if (_gameAppThread != null && _gameAppThread.IsAlive) return;

            _appInitalized_Reset.Reset();
            var newSession = new Thread(StartNewAppSession);
            newSession.Name = "EditorApp session";
            newSession.Start();
            _appInitalized_Reset.WaitOne();

            _gameAppThread = newSession;
        }

        private void StartNewAppSession()
        {
            ExitApp();

            _gameApp = GameApplication.CreateContext();

            _gameApp.WindowResize += Game_WindowResize;
            _gameApp.Initializing += GameApp_Intialized;
            _gameApp.Window.AllowUserResizing = true;
            _gameApp.ExitEvent += GameApp_Exit;

            _gameApp.Run();
        }

        private void InitEntityControl()
        {
            if (_currentScene == null) return;

            foreach (var e in _currentScene.Entities)
            {
                //var btn = new EntityButton(e);

                //_entityBtns.Add(btn);
                //btn.MouseDown += EntityBtn_MouseDown;
                //Controls.Add(btn);
                //Controls.Add(new EntityControl(e));

                //TODO: DONOT CREATE ENTITYCONTROL ON DIFFERENT THREAD
                UIManager.Root.AddChild(new EntityControl(e));
            }
        }

        private void LoadScene(GameScene scene)
        {
            UnloadCurrentScene();

            _currentScene = scene;
            _currentScene.SetActive(true);
            SceneLoadEvent?.Invoke(this, _currentScene);

            InitEntityControl();
        }

        private Camera GetMainCamera()
        {
            return _mainCamera ??= new Camera(_gameApp.GraphicsDevice.Viewport);
        }

        //private void EntityBtn_MouseDown(object? sender, EventArgs e)
        //{
        //    SelectEntity(((EntityButton)sender).AttachedEntity);
        //}

        //Stopwatch   _gameLoopTimer;
        //TimeSpan    _drawElapsed;
        //TimeSpan    _updateElapsed;
        //System.Timers.Timer _updateTimer;
        //List<Button> _entityBtns = new();

        bool _isMouseDraggingCamera;
        Vector3 _cameraStartDragCoord;
        XNA.Point _mouseStartDragPoint;

        GameScene? _currentScene;
        TransformEntity _currentSelectedEntity;

        GameApplication? _gameApp;
        Thread? _gameAppThread;

        Camera? _mainCamera;

        ManualResetEvent _appInitalized_Reset = new(false);
    }
}
