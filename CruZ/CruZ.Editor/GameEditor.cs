using CruZ.Editor.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CruZ.Editor.UI;
using CruZ.Editor.Scene;
using Microsoft.Xna.Framework;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine;
using CruZ.Editor.Winform.Utility;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Resource;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;
using CruZ.GameEngine.GameSystem.Physic;


namespace CruZ.Editor.Controls
{
    /// <summary>
    /// Handle editing operations on <see cref="GameApplication"/>
    /// </summary>
    public partial class GameEditor
    {
        public event Action<TransformEntity?>? SelectingEntityChanged;

        public GameEditor(EditorForm form)
        {
            _editorForm = form;
            _cacheService = new CacheService(Path.Combine(EditorContext.UserProfileDir, "caches"));

            InputManager.MouseScrolled += Input_MouseScroll;
            InputManager.MouseMoved += Input_MouseMove;
            InputManager.MouseStateChanged += Input_MouseStateChanged;
            InputManager.KeyStateChanged += Input_KeyStateChanged;

            ECSManager.InstanceChanged += ECSManager_InstanceChanged;
        }

        private void RegisterGameAppEvents()
        {
            GameApplication.Initialized += Game_Intialized;
            GameApplication.Exiting += Game_Exiting;
            _gameApp!.Window.AllowUserResizing = true;
            //_gameApp.BeforeDraw += GameApp_EarlyDraw;
        }

        private void ECSManager_InstanceChanged(ECSManager? oldECS, ECSManager newECS)
        {
            if(oldECS != null)
            {
                oldECS.World.EntityAdded -= World_EntityAdded;
                oldECS.World.EntityRemoved -= World_EntityRemoved;
            }

            newECS.World.EntityAdded += World_EntityAdded;
            newECS.World.EntityRemoved += World_EntityRemoved;
        }

        /// <summary>
        /// Initialize editor UIControls and reading cache
        /// </summary>
        /// 
        public void LoadLastSessionCaches()
        {
            _cacheService.ReadCache(this, "LoadedScene");
        }

        public TransformEntity? SelectedEntity
        {
            get => _currentSelectEntity;
            set
            {
                if (_currentSelectEntity == value) return;

                if (_currentSelectEntity != null)
                    _fromEntityToControl[_currentSelectEntity].CanInteract = false;

                _currentSelectEntity = value;

                if (_currentSelectEntity != null)
                    _fromEntityToControl[_currentSelectEntity].CanInteract = true;

                LogManager.SetMsg(_currentSelectEntity != null ? _currentSelectEntity.ToString() : "");
                SelectingEntityChanged?.Invoke(value);
            }
        }

        public void LoadRuntimeScene(string sceneName)
        {
            if(LoadedGameScene != null) throw new InvalidOperationException("Can load only one scene");

            WaitGameInitialized();

            try
            {
                _gameApp!.MarshalInvoke(() => LoadedGameScene = SceneManager.GetRuntimeScene(sceneName));

            }
            catch (RuntimeSceneLoadException e)
            {
                DialogHelper.ShowExceptionDialog(e);
                throw;
            }
        }
        
        private void WaitGameInitialized()
        {
            if (_gameAppThread != null && _gameAppThread.IsAlive) return;

            _appInitalized_Reset.Reset();

            _gameAppThread = new Thread(StartNewAppSession);
            _gameAppThread.Name = "Editor Session";
            _gameAppThread.Start();

            _appInitalized_Reset.WaitOne(); // wait until game initialize
        }

        private void StartNewAppSession()
        {
            CleanAppSession();

            _gameApp = GameApplication.CreateContext(
                new GameWrapper(), 
                EditorContext.UserResourceDir);

            RegisterGameAppEvents();

            _gameApp.Run();
        }

        private void World_EntityAdded(TransformEntity e)
        {
            AddEntityControl(e);
        }

        private void World_EntityRemoved(TransformEntity e)
        {
            RemoveEntityControl(e);
        }

        private void AddEntityControl(TransformEntity e)
        {
            if (_fromEntityToControl.ContainsKey(e)) return;

            if (_entityControlPool.Count == 0)
            {
                var newControl = new EntityControl();
                newControl.IsActive = false;
                _editorUIBranch.AddChild(newControl);
                _entityControlPool.Push(newControl);
            }

            var entityControl = _entityControlPool.Pop();
            entityControl.IsActive = true;
            entityControl.AttachEntity = e;
            _fromEntityToControl[e] = entityControl;
        }

        private void RemoveEntityControl(TransformEntity e)
        {
            if (!_fromEntityToControl.ContainsKey(e)) return;

            var eControl = _fromEntityToControl[e];
            _fromEntityToControl.Remove(e);

            eControl.AttachEntity = null;
            eControl.CanInteract = false;
            eControl.IsActive = false;
            _entityControlPool.Push(eControl);
        }

        private void InitUIControls()
        {
            // orders of ui gettin added is effect which is drawing first
            _editorUIBranch = UIManager.Root.AddBranch("Editor");

            _boardGrid = new BoardGrid();
            _editorUIBranch.AddChild(_boardGrid);

            _infoWindow = new LoggingWindow();
            _editorUIBranch.AddChild(_infoWindow);

            _entityControlPool = [];
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

        private void OnApplicationBeforeClosing()
        {
            _cacheService.WriteCache(this, "LoadedScene");
            _cacheService.WriteCache(this, "Camera");
        }

        private void Game_Intialized()
        {
            _cacheService.ReadCache(this, "Camera");
            InitUIControls();
            _appInitalized_Reset.Set();
        }

        private void Game_Exiting()
        {
            _editorForm.SafeInvoke(CleanAppSession);
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
            if (info.IsKeyJustDown(Keys.OemTilde))
            {
                PhysicSystem.Instance.ShowDebug = !PhysicSystem.Instance.ShowDebug;
            }
        }

        public GameScene? LoadedGameScene { get; private set; }

        Stack<EntityControl> _entityControlPool = [];
        Dictionary<TransformEntity, EntityControl> _fromEntityToControl = [];

        BoardGrid _boardGrid = null!;
        LoggingWindow _infoWindow = null!;

        UIControl _editorUIBranch = null!;

        bool _isMouseDraggingCamera;
        Vector2 _cameraStartDragCoord;
        Point _mouseStartDragPoint;

        TransformEntity? _currentSelectEntity;
        GameApplication? _gameApp;

        Thread? _gameAppThread;
        ManualResetEvent _appInitalized_Reset = new(false); // wait handler for game initialize event

        EditorForm _editorForm;

        CacheService _cacheService;
    }
}
