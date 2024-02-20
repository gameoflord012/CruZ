﻿using CruZ.Components;
using CruZ.Editor.UI;
using CruZ.Editor.Utility;
using CruZ.Exception;
using CruZ.Resource;
using CruZ.Scene;
using CruZ.Systems;
using CruZ.UI;
using CruZ.Utility;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace CruZ.Editor.Controls
{
    /// <summary>
    /// Handle editing operations on game, also manage GameApplication
    /// </summary>
    public partial class GameEditor
    {
        public event Action<GameScene?> CurrentSceneChanged;
        public event Action<TransformEntity?> SelectingEntityChanged;

        public GameScene? CurrentGameScene => _currentScene;

        public GameEditor(EditorForm form)
        {
            _editorForm = form;

            Input.MouseScrolled     += Input_MouseScroll;
            Input.MouseMoved        += Input_MouseMove;
            Input.MouseStateChanged += Input_MouseStateChanged;
            Input.KeyStateChanged   += Input_KeyStateChanged;

            _editorForm.FormClosing += EditorForm_Closing;
            UIManager.MouseClick += UI_MouseClick;

            _thisThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Init()
        {
            CacheService.Register(this);
            CacheRead?.Invoke(this, "LoadedScene");
        }

        #region Public_Functions
        public void UnloadCurrentScene()
        {
            SelectedEntity = null;

            if (_currentScene == null) return;

            if(_currentScene.ResourceInfo != null && !_currentScene.ResourceInfo.IsRuntime)
                ResourceManager.SaveResource(_currentScene);

            _currentScene.Dispose();
            _currentScene = null;

            CurrentSceneChanged?.Invoke(null);
        }

        public TransformEntity? SelectedEntity 
        { 
            get => _currentSelect != null ? _currentSelect : null; 
            set 
            {
                lock(this)
                {
                    if (_currentSelect != null && value == _currentSelect)
                        return;

                    // Disable previous EntityControl
                    if (_currentSelect != null)
                        GetEntityControl(_currentSelect).SelectEntity(false);

                    if (value != null)
                    {
                        _currentSelect = value;
                        GetEntityControl(_currentSelect).SelectEntity(true);
                    }
                    else
                    {
                        _currentSelect = null;
                    }

                    Logging.SetMsg(value != null ? value.ToString() : "");
                    SelectingEntityChanged?.Invoke(value);
                }
            } 
        }

        public void LoadSceneFromFile(string file)
        {
            Check_AppInitialized();

            var scene = ResourceManager.LoadResource<GameScene>(file);
            scene.Name = Path.GetRelativePath(ResourceManager.ResourceRoot, file);

            LoadScene(scene);
        }

        public void LoadRuntimeScene(string sceneName)
        {
            Check_AppInitialized();

            try
            {
                LoadScene(SceneManager.GetRuntimeScene(sceneName));
            }
            catch(RuntimeSceneLoadException e)
            {
                DialogHelper.ShowExceptionDialog(e);
                throw;
            }
        }

        public void CleanAppSession()
        {
            if(_gameApp == null || _gameApp.ExitCalled) return;
            Trace.Assert(_gameAppThread != null);

            CacheWrite?.Invoke(this, "Camera");

            _gameApp.Exit();

            if (!_gameAppThread.Join(5000))
                throw new System.Exception("Can't exit editor app");

            _gameApp = null;
            _gameAppThread = null;
        }
        #endregion

        #region Event_Handlers
        private void EditorForm_Closing(object? sender, FormClosingEventArgs args)
        {
            CacheWrite?.Invoke(this, "LoadedScene");
        }

        private void GameApp_WindowResize(Viewport viewport)
        {
            GetMainCamera().ViewPortWidth = viewport.Width;
            GetMainCamera().ViewPortHeight = viewport.Height;
        }
        
        private void GameApp_Intialized()
        {
            Camera.Main = GetMainCamera();
            CacheRead?.Invoke(this, "Camera");

            _appInitalized_Reset.Set();
        }

        private void GameApp_Exit()
        {
            UnloadCurrentScene();
            _editorForm.SafeInvoke(CleanAppSession);
        }

        //private void GameApp_EarlyDraw(DrawEventArgs args)
        //{
        //    DrawAxis(args.SpriteBatch);
        //}

        private void Input_MouseScroll(IInputInfo info)
        {
            Camera.Main.Zoom = new(
                Camera.Main.Zoom.X - info.SrollDelta * 0.001f * Camera.Main.Zoom.X, 
                Camera.Main.Zoom.Y);
        }

        private void Input_MouseMove(IInputInfo info)
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

        private void Input_MouseStateChanged(IInputInfo info)
        {
            if (info.IsMouseJustDown(MouseKey.Middle)
                && !_isMouseDraggingCamera)
            {
                _isMouseDraggingCamera = true;
                _mouseStartDragPoint = info.CurMouse.Position;
                _cameraStartDragCoord = Camera.Main.Position;
            }

            if (info.IsMouseJustUp(MouseKey.Middle))
            {
                _isMouseDraggingCamera = false;
            }
        }

        private void Input_KeyStateChanged(IInputInfo info)
        {
            if( info.Keyboard.IsKeyDown(XNA.Input.Keys.LeftControl) &&
                info.IsKeyJustDown(XNA.Input.Keys.Z))
            {
                Debug.WriteLine("Undo");
            }
        }

        private void UI_MouseClick(UIInfo info)
        {
            FindEntityToSelect(info);
        }
        #endregion
        
        #region Private_Functions
        private void FindEntityToSelect(UIInfo info)
        {
            var contains = UIManager.GetContains(info.MousePos().X, info.MousePos().Y);

            var eControl = contains
                .Where(e => e is EntityControl)
                .Select(e => (EntityControl)e).ToList();

            eControl.Sort((e1, e2) =>
            {
                SpriteComponent? sp1 = null, sp2 = null;
                e1.AttachEntity.TryGetComponent(ref sp1);
                e2.AttachEntity.TryGetComponent(ref sp2);

                if (sp1 == sp2) return 0;
                if (sp1 == null) return -1;
                if (sp2 == null) return 1;

                return sp1.CompareLayer(sp2);
            });

            if (eControl.Count() == 0)
            {
                SelectedEntity = null;
                return;
            }

            int idx = 0;

            if(_currentSelect  != null)
            {
                for (int i = 0; i < eControl.Count(); i++)
                {
                    if (eControl[i] == GetEntityControl(_currentSelect))
                    {
                        idx = i;
                        break;
                    }
                }
            }

            idx = (idx + 1) % eControl.Count();
            SelectedEntity = eControl[idx].AttachEntity;
        }

        private EntityControl GetEntityControl(TransformEntity e)
        {
            foreach (var control in _eControls)
            {
                if (control.AttachEntity == e)
                {
                    return control;
                }
            }

            throw new ArgumentException($"There is no EntityControl for entity {e}");
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
            CleanAppSession();

            _gameApp = GameApplication.CreateContext();
            RegisterGameAppEvents();

            _gameApp.Run();
        }

        private void RegisterGameAppEvents()
        {
            _gameApp.WindowResize += GameApp_WindowResize;
            _gameApp.Initializing += GameApp_Intialized;
            _gameApp.Window.AllowUserResizing = true;
            _gameApp.ExitEvent += GameApp_Exit;
            //_gameApp.EarlyDraw += GameApp_EarlyDraw;
        }

        private void InitUIControls()
        {
            // orders of ui gettin added is effect which is drawing first

            UIManager.Root.AddChild(new BoardGrid());

            #region EntityControl
            if (_currentScene == null) return;

            _eControls.Clear();

            foreach (var e in _currentScene.Entities)
            {
                var eControl = new EntityControl(e);
                UIManager.Root.AddChild(eControl);
                _eControls.Add(eControl);
            } 
            #endregion

            #region InfoTextWindow
            _infoTextWindow = new LoggingWindow();
            UIManager.Root.AddChild(_infoTextWindow);
            #endregion
    }

        private void LoadScene(GameScene scene)
        {
            UnloadCurrentScene();

            _currentScene = scene;
            _currentScene.SetActive(true);
            CurrentSceneChanged?.Invoke(_currentScene);

            Logging.SetMsg(_currentScene.ToString(), "Scene");

            InitUIControls();
        }

        private Camera GetMainCamera()
        {
            return _mainCamera ??= new Camera(_gameApp.GraphicsDevice.Viewport);
        }
        #endregion

        #region Private_Variables
        bool _isMouseDraggingCamera;
        Vector3 _cameraStartDragCoord;
        XNA.Point _mouseStartDragPoint;

        GameScene? _currentScene;
        TransformEntity? _currentSelect;

        GameApplication? _gameApp;
        Thread? _gameAppThread;
        int _thisThreadId;

        Camera? _mainCamera;

        ManualResetEvent _appInitalized_Reset = new(false);

        List<EntityControl> _eControls = [];
        LoggingWindow _infoTextWindow;

        EditorForm _editorForm;
        #endregion
    }
}