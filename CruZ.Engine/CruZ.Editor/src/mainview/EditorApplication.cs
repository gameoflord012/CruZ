using CruZ.Components;
using CruZ.Editor.UI;
using CruZ.Resource;
using CruZ.Systems;
using CruZ.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public partial class EditorApplication
    {
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

        #region PUBLIC_FUNCS
        public void UnloadCurrentScene()
        {
            if (_currentScene == null) return;

            _currentScene.SetActive(false);
            //_currentScene.Dispose();
        }

        public void SelectEntity(TransformEntity e)
        {
            if (_currentSelect != null &&
                e == _currentSelect.AttachEntity) 
            {
                _currentSelect.SelectEntity(false);
                return;
            }

            if (_currentSelect != null)
            {
                _currentSelect.SelectEntity(false);
            }

            _currentSelect = GetEntityControl(e);
            _currentSelect.SelectEntity(true);

            OnSelectedEntityChanged?.Invoke(this, e);
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

        private void EntityControl_Selecting(EntityControl control)
        {
            SelectEntity(control.AttachEntity);
        }
        #endregion

        #region PRIVATE

        private EntityControl? GetEntityControl(TransformEntity e)
        {
            foreach (var control in _eControls)
            {
                if (control.AttachEntity == e)
                {
                    return control;
                }
            }

            return null;
        }

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

            _eControls.Clear();

            foreach (var e in _currentScene.Entities)
            {
                var eControl = new EntityControl(e);
                UIManager.Root.AddChild(eControl);
                eControl.Selecting += EntityControl_Selecting;

                _eControls.Add(eControl);
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

        #endregion

        bool                _isMouseDraggingCamera;
        Vector3             _cameraStartDragCoord;
        XNA.Point           _mouseStartDragPoint;

        GameScene?          _currentScene;
        EntityControl?      _currentSelect;

        GameApplication?    _gameApp;
        Thread?             _gameAppThread;

        Camera?             _mainCamera;

        ManualResetEvent    _appInitalized_Reset = new(false);
        
        List<EntityControl> _eControls = [];
    }
}
