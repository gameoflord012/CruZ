using CruZ.Components;
using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    internal partial class WorldViewControl : MonoGame.Forms.NET.Controls.InvalidationControl, IECSContextProvider, IApplicationContextProvider, IInputContextProvider
    {
        public event Action<GameTime> DrawEvent;
        public event Action<GameTime> UpdateEvent;
        public event Action InitializeSystemEvent;
        public event Action<GameTime> UpdateInputEvent { add { UpdateEvent += value; } remove { UpdateEvent -= value; } }
        public event EventHandler<TransformEntity> OnSelectedEntityChanged;

        public GraphicsDevice GraphicsDevice => Editor.GraphicsDevice;
        public ContentManager Content => Editor.Content;
        public GameScene? CurrentGameScene => _currentScene;

        public WorldViewControl()
        {
            ECS.CreateContext(this);
            ApplicationContext.CreateContext(this);
            Input.CreateContext(this);

            _camera = new Camera(Width, Height);
            Camera.Main = _camera;

            CacheService.RegisterCacheControl(this);
            CanReadCacheChanged?.Invoke(this, false);
        }

        protected override void Initialize()
        {
            Editor.Content.RootDirectory = ".";
            InitializeSystemEvent.Invoke();

            Editor.FPSCounter.Enabled = true;

            _gameLoopTimer = new();
            _gameLoopTimer.Start();

            _drawElapsed = _gameLoopTimer.Elapsed;
            _updateElapsed = _gameLoopTimer.Elapsed;

            Application.Idle -= Update;
            Application.Idle += Update;

            CanReadCacheChanged?.Invoke(this, true);
        }

        private void Update(object? sender, EventArgs e)
        {
            GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _updateElapsed);
            _updateElapsed = _gameLoopTimer.Elapsed;

            Editor.FPSCounter.Update(gameTime);
            UpdateEvent.Invoke(gameTime);

            Invalidate();
        }

        protected override void Draw()
        {
            GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _drawElapsed);
            _drawElapsed = _gameLoopTimer.Elapsed;

            Editor.FPSCounter.UpdateFrameCounter();

            DrawEvent?.Invoke(gameTime);
            DrawAxis();

            Update();
        }

        public void LoadScene(GameScene scene)
        {
            UnloadCurrentScene();

            _currentScene = scene;
            _currentScene.SetActive(true);

            InitEntityControl();
        }

        public void UnloadCurrentScene()
        {
            if (_currentScene == null) return;

            _currentScene.SetActive(false);
            _currentScene.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Camera.Main.ViewPortWidth = Width;
            Camera.Main.ViewPortHeight = Height;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Camera.Main.Zoom = new(Camera.Main.Zoom.X - e.Delta * 0.001f * Camera.Main.Zoom.X, Camera.Main.Zoom.Y);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isMouseDragging)
            {
                var scale = Camera.Main.ScreenToWorldScale();
                var delt = new Vector3(
                    (e.X - _mouseStartDragPoint.X) * scale.X,
                    (e.Y - _mouseStartDragPoint.Y) * scale.Y);

                Camera.Main.Position = _cameraStartDragCoord - delt;
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == _cameraMouseDragButton && !_isMouseDragging)
            {
                _isMouseDragging = true;
                _mouseStartDragPoint = e.Location;
                _cameraStartDragCoord = Camera.Main.Position;
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == _cameraMouseDragButton)
            {
                _isMouseDragging = false;
            }
        }

        private void InitEntityControl()
        {
            if (_currentScene == null) return;
            foreach (var e in _currentScene.Entities)
            {
                var btn = new EntityButton(e);

                _entityBtns.Add(btn);
                btn.MouseDown += EntityBtn_MouseDown;
                Controls.Add(btn);
            }
        }

        private void SelectEntity(TransformEntity e)
        {
            if (e == _currentSelectedEntity) return;

            _currentSelectedEntity = e;
            OnSelectedEntityChanged?.Invoke(this, _currentSelectedEntity);
        }

        private void EntityBtn_MouseDown(object? sender, EventArgs e)
        {
            SelectEntity(((EntityButton)sender).AttachedEntity);
        }

        readonly MouseButtons _cameraMouseDragButton = MouseButtons.Middle;

        Stopwatch   _gameLoopTimer;
        TimeSpan    _drawElapsed;
        TimeSpan    _updateElapsed;

        bool                    _isMouseDragging;
        Vector3                 _cameraStartDragCoord;
        System.Drawing.Point    _mouseStartDragPoint;

        GameScene?      _currentScene;
        TransformEntity _currentSelectedEntity;

        List<Button> _entityBtns = new();

        Camera _camera;
    }
}
