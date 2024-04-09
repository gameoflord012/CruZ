using CruZ.Common.ECS;
using CruZ.Editor.Global;
using CruZ.Editor.Service;
using CruZ.Common.Scene;
using CruZ.Common.UI;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CruZ.Editor.UI;
using CruZ.Common;
using CruZ.Editor.Scene;
using Microsoft.Xna.Framework;
using CruZ.Framework.Service;
using CruZ.Framework.Input;
using CruZ.Framework.Resource;
using CruZ.Framework.GameSystem.ECS;
using CruZ.Framework.Exceptions;
using CruZ.Framework;
using CruZ.Editor.Winform.Utility;


namespace CruZ.Editor.Controls
{
    /// <summary>
    /// Handle editing on <see cref="GameApplication"/>, 
    /// also a wrapper of it.
    /// </summary>
    public partial class GameEditor
    {
        public event Action<GameScene?>? CurrentSceneChanged;
        public event Action<TransformEntity?>? SelectingEntityChanged;

        public GameScene? CurrentGameScene => _currentScene;

        public GameEditor(EditorForm form)
        {
            _editorForm = form;
            _thisThreadId = Thread.CurrentThread.ManagedThreadId;
            _cacheService = new CacheService(Path.Combine(EditorContext.UserProfileDir, "caches\\"));
            _userResource = EditorContext.UserResource;

            InputManager.MouseScrolled += Input_MouseScroll;
            InputManager.MouseMoved += Input_MouseMove;
            InputManager.MouseStateChanged += Input_MouseStateChanged;
            InputManager.KeyStateChanged += Input_KeyStateChanged;
            UIManager.MouseClick += UI_MouseClick;
        }

        public void Init()
        {
            _cacheService.Register(this);
            CacheRead?.Invoke(this, "LoadedScene");
        }

        #region Public_Functions
        public void UnloadCurrentScene()
        {
            SelectedEntity = null;

            if (_currentScene == null) return;

            // Check whether the resource is load from ResourceManager
            if (_currentScene.ResourceInfo != null)
                EditorContext.UserResource.Save(_currentScene);

            _currentScene.Dispose();
            _currentScene = null;

            CurrentSceneChanged?.Invoke(null);
        }

        public TransformEntity? SelectedEntity
        {
            get => _currentSelect != null ? _currentSelect : null;
            set
            {
                lock (this)
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

                    LogService.SetMsg(value != null ? value.ToString() : "");
                    SelectingEntityChanged?.Invoke(value);
                }
            }
        }

        public void LoadSceneFromFile(string file)
        {
            CheckAppInitialized();

            var scene = _userResource.Load<GameScene>(file);
            scene.Name = Path.GetRelativePath(_userResource.ResourceRoot, file);

            LoadScene(scene);
        }

        public void LoadRuntimeScene(string sceneName)
        {
            CheckAppInitialized();

            try
            {
                LoadScene(SceneManager.GetRuntimeScene(sceneName));
            }
            catch (RuntimeSceneLoadException e)
            {
                DialogHelper.ShowExceptionDialog(e);
                throw;
            }
        }

        /// <summary>
        /// Should be called from winform thread
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CleanAppSession()
        {
            if (_gameApp == null) return;
            Trace.Assert(_gameAppThread != null);

            OnApplicationBeforeClosing();

            _gameApp.Exit();

            if (!_gameAppThread.Join(5000))
                throw new Exception("Can't exit editor app");

            _gameApp?.Dispose();
            _gameApp = null;
            _gameAppThread = null;
        }

        public TransformEntity CreateNewEntity()
        {
            if (_currentScene == null)
                throw new InvalidOperationException("Can't create new entity when Scene is not loaded");

            var newEntity = _currentScene.CreateEntity();

            UpdateEntityControls();

            return newEntity;
        }

        public void RemoveEntity(TransformEntity e)
        {
            _currentScene.RemoveEntity(e);
            e.Dispose();
        }
        #endregion

        #region Event_Handlers
        private void OnApplicationBeforeClosing()
        {
            CacheWrite?.Invoke(this, "LoadedScene");
            CacheWrite?.Invoke(this, "Camera");
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

        private void GameApp_Exiting()
        {
            UnloadCurrentScene();
            _editorForm.SafeInvoke(CleanAppSession);
        }

        private void Scene_EntityAdded(TransformEntity e)
        {
            AddEntityControl(e);
        }

        private void Scene_EntityRemoved(TransformEntity e)
        {
            RemoveEntityControl(e);
        }

        private void Input_MouseScroll(IInputInfo info)
        {
            Camera.Main.Zoom = Camera.Main.Zoom + info.SrollDelta * 0.001f * Camera.Main.Zoom;
        }

        private void Input_MouseMove(IInputInfo info)
        {
            if (_isMouseDraggingCamera)
            {
                var mousePoint = info.CurMouse.Position;
                var delt = new Vector2(
                    (_mouseStartDragPoint.X - mousePoint.X) / Camera.Main.ScreenToWorldRatio().X,
                    (_mouseStartDragPoint.Y - mousePoint.Y) / Camera.Main.ScreenToWorldRatio().Y
                );
                Camera.Main.CameraOffset = _cameraStartDragCoord + delt;
            }
        }

        private void Input_MouseStateChanged(IInputInfo info)
        {
            if (info.IsMouseJustDown(MouseKey.Middle)
                && !_isMouseDraggingCamera)
            {
                _isMouseDraggingCamera = true;
                _mouseStartDragPoint = info.CurMouse.Position;
                _cameraStartDragCoord = Camera.Main.CameraOffset;
            }

            if (info.IsMouseJustUp(MouseKey.Middle))
            {
                _isMouseDraggingCamera = false;
            }
        }

        private void Input_KeyStateChanged(IInputInfo info)
        {
            if (info.Keyboard.IsKeyDown(XNA.Input.Keys.LeftControl) &&
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
                e1.AttachEntity.TryGetComponent(out SpriteRendererComponent? sp1);
                e2.AttachEntity.TryGetComponent(out SpriteRendererComponent? sp2);

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

            if (_currentSelect != null)
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

        private void CheckAppInitialized()
        {
            if (_gameAppThread != null && _gameAppThread.IsAlive) return;

            _appInitalized_Reset.Reset();
            var newSession = new Thread(StartNewAppSession);
            newSession.Name = "Editor Session";
            newSession.Start();
            _appInitalized_Reset.WaitOne();

            _gameAppThread = newSession;
        }

        private void StartNewAppSession()
        {
            CleanAppSession();

            _gameApp = GameApplication.CreateContext(new GameWrapper());
            RegisterGameAppEvents();

            _gameApp.Run();
        }

        private void RegisterGameAppEvents()
        {
            GameApplication.WindowResized += GameApp_WindowResize;
            GameApplication.Initialized += GameApp_Intialized;
            GameApplication.Exiting += GameApp_Exiting;
            _gameApp.Window.AllowUserResizing = true;
            //_gameApp.BeforeDraw += GameApp_EarlyDraw;
        }

        private void InitUIControls()
        {
            // orders of ui gettin added is effect which is drawing first

            UIManager.Root.AddChild(new BoardGrid());

            UpdateEntityControls();

            #region InfoTextWindow
            _infoTextWindow = new LoggingWindow();
            UIManager.Root.AddChild(_infoTextWindow);
            #endregion
        }

        private void UpdateEntityControls()
        {
            _eControls.Clear();

            if (_currentScene == null) return;

            if (_lastScene != null)
            {
                _lastScene.EntityAdded -= Scene_EntityAdded; ;
                _lastScene.EntityRemoved -= Scene_EntityRemoved;
            }

            _currentScene.EntityAdded += Scene_EntityAdded;
            _currentScene.EntityRemoved += Scene_EntityRemoved;

            foreach (var e in _currentScene.Entities)
            {
                AddEntityControl(e);
            }
        }

        private void AddEntityControl(TransformEntity e)
        {
            if (GetEntityControl(e) != null) return;

            var eControl = new EntityControl(e);
            UIManager.Root.AddChild(eControl);
            _eControls.Add(eControl);
        }

        private void RemoveEntityControl(TransformEntity e)
        {
            GetEntityControl(e)?.Dispose();
        }

        private void LoadScene(GameScene scene)
        {
            if (scene == _currentScene) return;

            UnloadCurrentScene();

            _lastScene = _currentScene;
            _currentScene = scene;
            _currentScene.SetActive(true);
            CurrentSceneChanged?.Invoke(_currentScene);

            LogService.SetMsg(_currentScene.ToString(), "Scene");

            InitUIControls();
        }

        private Camera GetMainCamera()
        {
            return _mainCamera ??= new Camera(_gameApp.Window);
        }
        #endregion

        #region Private_Variables
        bool _isMouseDraggingCamera;
        Vector2 _cameraStartDragCoord;
        Point _mouseStartDragPoint;

        GameScene? _currentScene;
        GameScene? _lastScene;
        Camera? _mainCamera;
        TransformEntity? _currentSelect;
        GameApplication? _gameApp;

        Thread? _gameAppThread;
        ManualResetEvent _appInitalized_Reset = new(false);
        int _thisThreadId;

        List<EntityControl> _eControls = [];
        LoggingWindow _infoTextWindow;
        EditorForm _editorForm;

        CacheService _cacheService;
        ResourceManager _userResource;
        #endregion
    }
}
