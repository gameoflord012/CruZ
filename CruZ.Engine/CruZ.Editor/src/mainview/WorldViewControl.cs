using CruZ.Components;
using CruZ.Systems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public partial class WorldViewControl
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

        public event EventHandler<GameScene>        SceneLoadEvent;
        public event EventHandler<TransformEntity>  OnSelectedEntityChanged;
        
        public GameScene? CurrentGameScene => _currentScene;

        public WorldViewControl()
        {
            //ECS                 .SetContext(this);
            //ApplicationContext  .SetContext(this);
            //Input               .SetContext(this);
            //UIManager           .SetContext(this);

            _gameApp = new GameApplication();
            _gameApp.WindowResize += Game_WindowResize;
            _gameApp.Initialize += Game_Initialize;
            _gameApp.Window.AllowUserResizing = true;

            Input.MouseScroll   += Input_MouseScroll;
            Input.MouseMove     += Input_MouseMove;
            Input.MouseDown     += Input_MouseDown;
            Input.MouseUp       += Input_MouseUp;

            CacheService.RegisterCacheControl(this);
            CanReadCache?.Invoke(this, false);
        }

        private void Game_Initialize()
        {
            CanReadCache?.Invoke(this, true);

            _mainCamera = new Camera(_gameApp.GraphicsDevice.Viewport);
            Camera.Main = _mainCamera;
        }

        public void Run()
        {
            _gameApp.Run();
        }

        #region COMMENT
        //protected void Initialize()
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

        //private void Update(object? sender, ElapsedEventArgs e)
        //{
        //    //GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _updateElapsed);
        //    //_updateElapsed = _gameLoopTimer.Elapsed;

        //    //Editor.FPSCounter.OnUpdate(gameTime);

        //    //InputUpdate?   .Invoke(gameTime);
        //    //ECSUpdate?        .Invoke(gameTime);
        //    //UpdateUI            .Invoke(gameTime);

        //    //Invalidate();
        //}

        //protected void Draw()
        //{
        //    //GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _drawElapsed);
        //    //_drawElapsed = _gameLoopTimer.Elapsed;

        //    ////Editor.FPSCounter.UpdateFrameCounter();

        //    //ECSDraw?.Invoke(gameTime);
        //    //DrawUI?.Invoke(gameTime);

        //    //DrawAxis();
        //}
        #endregion

        public void LoadScene(GameScene scene)
        {
            UnloadCurrentScene();

            _currentScene = scene;
            _currentScene.SetActive(true);
            SceneLoadEvent.Invoke(this, _currentScene);

            InitEntityControl();
        }

        public void UnloadCurrentScene()
        {
            if (_currentScene == null) return;

            _currentScene.SetActive(false);
            _currentScene.Dispose();
        }

        public void SelectEntity(TransformEntity e)
        {
            if (e == _currentSelectedEntity) return;

            _currentSelectedEntity = e;
            OnSelectedEntityChanged?.Invoke(this, _currentSelectedEntity);
        }
        
        private void Game_WindowResize(Viewport viewport)
        {
            Camera.Main.ViewPortWidth = viewport.Width;
            Camera.Main.ViewPortHeight = viewport.Height;
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
            if (info.CurMouse.MiddleButton == XNA.Input.ButtonState.Pressed)
            {
                _isMouseDraggingCamera = false;
            }
        }

        private void InitEntityControl()
        {
            //if (_currentScene == null) return;
            //foreach (var e in _currentScene.Entities)
            //{
            //    var btn = new EntityButton(e);

            //    _entityBtns.Add(btn);
            //    btn.MouseDown += EntityBtn_MouseDown;
            //    Controls.Add(btn);
            //    //Controls.Add(new EntityControl(e));
            //    UIManager.Controls.Add(new EntityControl(e));
            //}
        }

        //private void EntityBtn_MouseDown(object? sender, EventArgs e)
        //{
        //    SelectEntity(((EntityButton)sender).AttachedEntity);
        //}

        readonly MouseButtons _cameraMouseDragButton = MouseButtons.Middle;

        //Stopwatch   _gameLoopTimer;
        //TimeSpan    _drawElapsed;
        //TimeSpan    _updateElapsed;
        //System.Timers.Timer _updateTimer;
        //List<Button> _entityBtns = new();

        bool        _isMouseDraggingCamera;
        Vector3     _cameraStartDragCoord;
        XNA.Point   _mouseStartDragPoint;

        GameScene?          _currentScene;
        TransformEntity     _currentSelectedEntity;
        GameApplication     _gameApp;

        Camera _mainCamera;
    }
}
