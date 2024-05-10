using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

using CruZ.Editor.Scene;
using CruZ.Editor.Service;
using CruZ.Editor.UI;
using CruZ.Editor.Winform.Utility;
using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.UI;
using CruZ.GameEngine.Input;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

using Keys = Microsoft.Xna.Framework.Input.Keys;


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
            _entityControlPool = [];
            _fromEntityToControl = [];
            _gameInitalizeWaitHandle = new(false);
            _editorForm = form;
            _cacheService = new CacheService(Path.Combine(EditorContext.UserProfileDir, "caches"));

            GameInput.MouseScrolled += Input_MouseScroll;
            GameInput.MouseMoved += Input_MouseMove;
            GameInput.MouseStateChanged += Input_MouseStateChanged;
            GameInput.KeyStateChanged += Input_KeyStateChanged;

            ECSManager.InstanceChanged += ECSManager_InstanceChanged;
        }
        private void RegisterGameAppEvents()
        {
            GameApplication.Initialized += Game_Intialized;
            GameApplication.Exiting += Game_Exiting;
            _gameApp!.Window.AllowUserResizing = true;
            //_gameApp.BeforeDraw += GameApp_EarlyDraw;
        }

        /// <summary>
        /// Initialize editor UIControls and reading cache
        /// </summary>
        /// 
        public void LoadLastSessionCaches()
        {
            _cacheService.ReadCache(this, "LoadedScene");
        }

        public void LoadRuntimeScene(string sceneName)
        {
            if(LoadedGameScene != null) throw new InvalidOperationException("Can load only one scene");

            WaitGameInitialized();

            try
            {
                _gameApp!.MarshalInvoke(() => LoadedGameScene = SceneManager.GetRuntimeScene(sceneName));

            }
            catch(RuntimeSceneLoadException e)
            {
                DialogHelper.ShowExceptionDialog(e);
                throw;
            }
        }

        private void WaitGameInitialized()
        {
            if(_gameAppThread != null && _gameAppThread.IsAlive) return;

            _gameInitalizeWaitHandle.Reset();

            _gameAppThread = new Thread(StartNewAppSession);
            _gameAppThread.Name = "Editor Session";
            _gameAppThread.Start();

            _gameInitalizeWaitHandle.WaitOne(); // wait until game initialize
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

        private void AddEntityControl(TransformEntity e)
        {
            if(_fromEntityToControl.ContainsKey(e)) return;

            if(_entityControlPool.Count == 0)
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
            if(!_fromEntityToControl.ContainsKey(e)) return;

            var eControl = _fromEntityToControl[e];
            _fromEntityToControl.Remove(e);

            eControl.AttachEntity = null;
            eControl.Active = false;
            eControl.IsActive = false;
            _entityControlPool.Push(eControl);
        }

        /// <summary>
        /// Should be called from winform thread
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CleanAppSession()
        {
            if(_gameApp == null) return;
            Trace.Assert(_gameAppThread != null);

            OnApplicationBeforeClosing();

            _gameApp.Exit();

            if(!_gameAppThread.Join(5000))
                throw new Exception("Can't exit editor app");

            _gameApp?.Dispose();
            _gameApp = null;
            _gameAppThread = null;
        }

        private void InitUIControls()
        {
            // orders of ui gettin added is effect which is drawing first
            _editorUIBranch = UISystem.Root.AddBranch("Editor");

            _boardGrid = new BoardGrid();
            _editorUIBranch.AddChild(_boardGrid);

            _infoWindow = new LoggingWindow();
            _editorUIBranch.AddChild(_infoWindow);

            _entityControlPool = [];
        }

        private void OnApplicationBeforeClosing()
        {
            _cacheService.WriteCache(this, "LoadedScene");
            _cacheService.WriteCache(this, "Camera");
        }
        private void OnDebugModeChanged(bool shouldDisplayDebug)
        {
            if(shouldDisplayDebug)
            {
                PhysicSystem.Instance.ShowDebug = true;
            }
            else
            {
                PhysicSystem.Instance.ShowDebug = false;
            }
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

        private void Game_Intialized()
        {
            _cacheService.ReadCache(this, "Camera");
            _debugMode = true;
            InitUIControls();
            _gameInitalizeWaitHandle.Set();
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
            if(_isMouseDraggingCamera)
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
            if(info.IsMouseJustDown(MouseKey.Middle)
                && !_isMouseDraggingCamera)
            {
                _isMouseDraggingCamera = true;
                _mouseStartDragPoint = info.CurMouse.Position;
                _cameraStartDragCoord = Camera.Main.CameraOffset;
            }

            if(info.IsMouseJustUp(MouseKey.Middle))
            {
                _isMouseDraggingCamera = false;
            }
        }
        private void Input_KeyStateChanged(IInputInfo info)
        {
            if(info.IsKeyJustDown(Keys.OemTilde))
            {
                _debugMode = !_debugMode;
                OnDebugModeChanged(_debugMode);
            }

            if(info.IsKeyHeldDown(Keys.LeftControl) && info.IsKeyJustDown(Keys.P))
            {
                _pause = !_pause;
                OnPauseValueChanged(_pause);
            }
        }

        private void OnPauseValueChanged(bool pause)
        {
            Trace.Assert(_gameApp != null);

            if(pause)
            {
                _gameApp.Pause = true;
            }
            else
            {
                _gameApp.Pause = false;
            }
        }

        private void World_EntityAdded(TransformEntity e)
        {
            AddEntityControl(e);
        }
        private void World_EntityRemoved(TransformEntity e)
        {
            RemoveEntityControl(e);
        }

        public GameScene? LoadedGameScene
        {
            get;
            private set;
        }
        public TransformEntity? SelectedEntity
        {
            get => _currentSelectEntity;
            set
            {
                if(_currentSelectEntity == value) return;

                if(_currentSelectEntity != null)
                    _fromEntityToControl[_currentSelectEntity].Active = false;

                _currentSelectEntity = value;

                if(_currentSelectEntity != null)
                    _fromEntityToControl[_currentSelectEntity].Active = true;

                LogManager.SetMsg(_currentSelectEntity != null ? _currentSelectEntity.ToString() : "");
                SelectingEntityChanged?.Invoke(value);
            }
        }

        private Stack<EntityControl> _entityControlPool;
        private Dictionary<TransformEntity, EntityControl> _fromEntityToControl;
        private BoardGrid _boardGrid;
        private LoggingWindow _infoWindow;
        private UIControl _editorUIBranch;
        private bool _isMouseDraggingCamera;
        private Vector2 _cameraStartDragCoord;
        private Point _mouseStartDragPoint;
        private TransformEntity? _currentSelectEntity;
        private GameApplication? _gameApp;
        private Thread? _gameAppThread;
        private ManualResetEvent _gameInitalizeWaitHandle; // wait handler for game initialize event
        private EditorForm _editorForm;
        private CacheService _cacheService;
        private bool _debugMode;
        private bool _pause;
    }
}
