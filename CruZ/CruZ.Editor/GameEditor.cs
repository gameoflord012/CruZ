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
using CruZ.Framework.Service;
using CruZ.Framework.Input;
using CruZ.Framework.Resource;
using CruZ.Framework.GameSystem.ECS;
using CruZ.Framework;
using CruZ.Editor.Winform.Utility;
using CruZ.Framework.Scene;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using CruZ.Framework.GameSystem.UI;


namespace CruZ.Editor.Controls
{
    /// <summary>
    /// Handle editing operations on <see cref="GameApplication"/>
    /// </summary>
    public partial class GameEditor
    {
        public event Action<GameScene?>? CurrentSceneChanged;

        public event Action<TransformEntity?>? SelectingEntityChanged;

        public GameScene? CurrentGameScene => _currentScene;

        public GameEditor(EditorForm form)
        {
            _editorForm = form;
            _cacheService = new CacheService(Path.Combine(EditorContext.UserProfileDir, "caches\\"));
            _userResource = EditorContext.UserResource;

            InputManager.MouseScrolled += Input_MouseScroll;
            InputManager.MouseMoved += Input_MouseMove;
            InputManager.MouseStateChanged += Input_MouseStateChanged;
            InputManager.KeyStateChanged += Input_KeyStateChanged;
            UIManager.MouseClick += UI_MouseClick;
        }
        
        /// <summary>
        /// Initialize editor UIControls and reading cache
        /// </summary>
        /// 
        public void LoadLastSessionCaches()
        {
            _cacheService.ReadCache(this, "LoadedScene");
        }

        Stack<EntityControl> _entityControlPool = [];
        Dictionary<TransformEntity, EntityControl> _fromEntityToControl = [];
        BoardGrid _boardGrid = null!;
        LoggingWindow _infoWindow = null!;

        public void LoadSceneFromFile(string file)
        {
            WaitGameInitialized();

            var scene = _userResource.Load<GameScene>(file);
            scene.Name = Path.GetRelativePath(_userResource.ResourceRoot, file);

            LoadScene(scene);
        }

        public void LoadRuntimeScene(string sceneName)
        {
            WaitGameInitialized();

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

        private void LoadScene(GameScene scene)
        {
            if (scene == _currentScene) return;

            UnloadCurrentScene();

            _currentScene = scene;

            _currentScene.EntityAdded += Scene_EntityAdded;
            _currentScene.EntityRemoved += Scene_EntityRemoved;
            _currentScene.SetActive(true);

            OnSceneLoaded();
            CurrentSceneChanged?.Invoke(_currentScene);
        }

        public void UnloadCurrentScene()
        {
            OnSceneUnloading();

            if (_currentScene == null) return;

            _currentScene.EntityAdded -= Scene_EntityAdded;
            _currentScene.EntityRemoved -= Scene_EntityRemoved;

            _currentScene.SetActive(false);
            _currentScene.Dispose();
            _currentScene = null;

            CurrentSceneChanged?.Invoke(null);
        }

        /// <summary>
        /// Call before unloading scene, _currentScene is now still the old scene
        /// </summary>
        private void OnSceneUnloading()
        {
            SelectedEntity = null;

            if(_currentScene != null) 
                EditorContext.UserResource.TrySave(_currentScene);

            // Clear entity controls which belong to unloading scene
            foreach(var entity in _fromEntityToControl.Keys)
            {
                RemoveEntity(entity);
            }
        }

        private void OnSceneLoaded()
        {
            LogManager.SetMsg(_currentScene == null ? "<Empty>" : _currentScene.ToString(), "Scene");

            foreach (var entity in _currentScene!.Entities)
            {
                AddEntityControl(entity);
            }
        }

        private void Scene_EntityAdded(TransformEntity e)
        {
            AddEntityControl(e);
        }

        private void Scene_EntityRemoved(TransformEntity e)
        {
            RemoveEntityControl(e);
        }

        private void AddEntityControl(TransformEntity e)
        {
            if (_fromEntityToControl.ContainsKey(e)) return;

            if (_entityControlPool.Count == 0)
            {
                var newControl = new EntityControl();
                _editorUIBranch.AddChild(newControl);
                _entityControlPool.Push(newControl);
            }

            var entityControl = _entityControlPool.Pop();
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
            _entityControlPool.Push(eControl);
        } 

        public TransformEntity? SelectedEntity
        {
            get => _currentSelectEntity;
            set
            {
                if (_currentSelectEntity == value) return;

                if(_currentSelectEntity != null)
                    _fromEntityToControl[_currentSelectEntity].CanInteract = false;

                _currentSelectEntity = value;

                if (_currentSelectEntity != null)
                    _fromEntityToControl[_currentSelectEntity].CanInteract = true;

                LogManager.SetMsg(_currentSelectEntity != null ? _currentSelectEntity.ToString() : "");
                SelectingEntityChanged?.Invoke(value);
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

        public TransformEntity CreateNewEntity(TransformEntity? parent = null)
        {
            if (_currentScene == null)
                throw new InvalidOperationException("Can't create new entity when Scene is not loaded");

            var newEntity = _currentScene.CreateEntity(null, parent);
            return newEntity;
        }

        public void RemoveEntity(TransformEntity e)
        {
            _currentScene.RemoveEntity(e);
            e.Dispose();
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

        private void Game_Exiting()
        {
            UnloadCurrentScene();
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
            if (info.Keyboard.IsKeyDown(Keys.LeftControl) &&
                info.IsKeyJustDown(Keys.Z))
            {
                Debug.WriteLine("Undo");
            }
        }

        private void UI_MouseClick(UIInfo info)
        {
            FindEntityToSelect(info);
        }

        private void FindEntityToSelect(UIInfo info)
        {
            var contains =
                _editorUIBranch.GetRaycastControls(info.MousePos().X, info.MousePos().Y);

            var eControl = contains
                .Where(e => e is EntityControl).Cast<EntityControl>()
                .Where(e => _fromEntityToControl.ContainsValue(e))
                .ToList();

            eControl.Sort((e1, e2) =>
            {
                e1.AttachEntity!.TryGetComponent(out SpriteRendererComponent? sp1);
                e2.AttachEntity!.TryGetComponent(out SpriteRendererComponent? sp2);

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

            if (_currentSelectEntity != null)
            {
                for (int i = 0; i < eControl.Count(); i++)
                {
                    if (eControl[i] == _fromEntityToControl[_currentSelectEntity])
                    {
                        idx = i;
                        break;
                    }
                }
            }

            idx = (idx + 1) % eControl.Count();
            SelectedEntity = eControl[idx].AttachEntity;
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

            _gameApp = GameApplication.CreateContext(new GameWrapper());
            RegisterGameAppEvents();

            _gameApp.Run();
        }

        private void RegisterGameAppEvents()
        {
            GameApplication.Initialized += Game_Intialized;
            GameApplication.Exiting += Game_Exiting;
            _gameApp.Window.AllowUserResizing = true;
            //_gameApp.BeforeDraw += GameApp_EarlyDraw;
        }

        #region Private_Variables
        UIControl _editorUIBranch;

        bool _isMouseDraggingCamera;
        Vector2 _cameraStartDragCoord;
        Point _mouseStartDragPoint;

        GameScene? _currentScene;
        TransformEntity? _currentSelectEntity;
        GameApplication? _gameApp;

        Thread? _gameAppThread;
        ManualResetEvent _appInitalized_Reset = new(false); // wait handler for game initialize event

        EditorForm _editorForm;

        CacheService _cacheService;
        ResourceManager _userResource;
        #endregion
    }
}
