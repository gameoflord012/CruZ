using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    internal class WorldViewControl : MonoGame.Forms.NET.Controls.InvalidationControl, IECSContextProvider, IApplicationContextProvider, IInputContextProvider
    {
        public event Action<GameTime>   DrawEvent;
        public event Action<GameTime>   UpdateEvent;
        public event Action             InitializeSystemEvent;
        public event Action<GameTime>   UpdateInputEvent { add { UpdateEvent += value; } remove { UpdateEvent -= value; } }

        public GraphicsDevice GraphicsDevice => Editor.GraphicsDevice;
        public ContentManager Content => Editor.Content;
        public GameScene? CurrentGameScene => _currentScene;

        public WorldViewControl()
        {
            ECS.CreateContext(this);
            ApplicationContext.CreateContext(this);
            Input.CreateContext(this);
            Camera.Main = new Camera(Width, Height);
        }

        protected override void Initialize()
        {
            Editor.Content.RootDirectory = ".";
            InitializeSystemEvent.Invoke();

            _gameLoopTimer = new();
            _gameLoopTimer.Start();

            _drawElapsed = _gameLoopTimer.Elapsed;
            _updateElapsed = _gameLoopTimer.Elapsed;

            Application.Idle -= Update;
            Application.Idle += Update;
        }

        private void Update(object? sender, EventArgs e)
        {
            GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _updateElapsed);
            _updateElapsed = _gameLoopTimer.Elapsed;

            UpdateEvent.Invoke(gameTime);
            Invalidate();
        }

        protected override void Draw()
        {
            GameTime gameTime = new(_gameLoopTimer.Elapsed, _gameLoopTimer.Elapsed - _drawElapsed);
            _drawElapsed = _gameLoopTimer.Elapsed;

            DrawEvent?.Invoke(gameTime);
            DrawAxis();
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

            Camera.Main.Zoom.X -= e.Delta * 0.001f * Camera.Main.Zoom.X;
            Debug.WriteLine(Camera.Main.Zoom.X);
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if(_isMouseDragging)
            {
                var scale = Camera.Main.ScreenToSpaceScale();
                var delt = new Vector3(
                    (e.X - _mouseStartDragPoint.X) * scale.X,
                    (e.Y - _mouseStartDragPoint.Y) * scale.Y);

                Camera.Main.Position = _cameraStartDragCoord + delt;
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if(e.Button == _cameraMouseDragButton && !_isMouseDragging)
            {
                _isMouseDragging = true;
                _mouseStartDragPoint = e.Location;
                _cameraStartDragCoord = Camera.Main.Position;
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if(e.Button == _cameraMouseDragButton)
            {
                _isMouseDragging = false;
            }
        }

        private void InitEntityControl()
        {
            if (_currentScene == null) return;
            foreach(var e in _currentScene.Entities)
            {
                Controls.Add(new EntityButton(e));
            }
        }

        private void DrawAxis()
        {
            Editor.spriteBatch.Begin();
            var yTop = Camera.Main.CoordinateToPoint(new Vector3(0, -5000));
            var yBot = Camera.Main.CoordinateToPoint(new Vector3(0, 5000));
            Editor.spriteBatch.DrawLine(yTop.X, yTop.Y, yBot.X, yBot.Y, Microsoft.Xna.Framework.Color.Black);


            var xTop = Camera.Main.CoordinateToPoint(new Vector3(-5000, 0));
            var xBot = Camera.Main.CoordinateToPoint(new Vector3(5000, 0));
            Editor.spriteBatch.DrawLine(xTop.X, xTop.Y, xBot.X, xBot.Y, Microsoft.Xna.Framework.Color.Black);

            var center = new PointF(Width / 2f, Height / 2f);
            Editor.spriteBatch.DrawLine(center.X, center.Y - 10, center.X, center.Y + 10, Microsoft.Xna.Framework.Color.Black);
            Editor.spriteBatch.DrawLine(center.X - 10, center.Y, center.X + 10, center.Y, Microsoft.Xna.Framework.Color.Black);

            Editor.spriteBatch.End();
        }

        readonly MouseButtons _cameraMouseDragButton = MouseButtons.Middle;

        Stopwatch _gameLoopTimer;
        TimeSpan _drawElapsed;
        TimeSpan _updateElapsed;

        bool _isMouseDragging;
        Vector3 _cameraStartDragCoord;
        System.Drawing.Point _mouseStartDragPoint;

        GameScene? _currentScene;

        List<Button> _entityBtns = new();
    }
}
