using CruZ.Resource;
using CruZ.Scene;
using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CruZ.Editor
{
    internal class WorldViewControl : MonoGame.Forms.NET.Controls.MonoGameControl, IECSContextProvider, IApplicationContextProvider, IInputContextProvider
    {
        public event Action<GameTime>   DrawEvent;
        public event Action<GameTime>   UpdateEvent;
        public event Action             InitializeSystemEvent;
        public event Action<GameTime>   UpdateInputEvent { add { UpdateEvent += value; } remove { UpdateEvent -= value; } }

        public GraphicsDevice GraphicsDevice => Editor.GraphicsDevice;
        public ContentManager Content => Editor.Content;

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

            _timer = new();
            _timer.Start();
            _elapsed = _timer.Elapsed;

            var scene = SceneManager.SceneAssets.Values.First();
            ResourceManager.InitResource("scenes\\scene1.scene", scene, true);
            scene.Dispose();

            scene = ResourceManager.LoadResource<GameScene>("scenes\\scene1.scene");
            scene.SetActive(true);
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateEvent.Invoke(gameTime);
        }

        protected override void Draw()
        {
            GameTime gameTime = new(_timer.Elapsed, _timer.Elapsed - _elapsed);
            _elapsed = _timer.Elapsed;

            DrawEvent?.Invoke(gameTime);
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
            DrawAxis(e);
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
                var mouseCurrentCoord = Camera.Main.PointToCoordinate(new(e.X, e.Y));
                Camera.Main.Position = _cameraStartDragCoord + mouseCurrentCoord - _mouseStartDragCoord;
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if(e.Button == _cameraMouseDragButton && !_isMouseDragging)
            {
                _isMouseDragging = true;
                _mouseStartDragCoord = Camera.Main.PointToCoordinate(new(e.X, e.Y));
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

        private void DrawAxis(System.Windows.Forms.PaintEventArgs e)
        {
            Pen pen = new Pen(System.Drawing.Color.FromArgb(100, 0, 0, 0));
            var yTop = Camera.Main.CoordinateToPoint(new Vector3(0, -5000));
            var yBot = Camera.Main.CoordinateToPoint(new Vector3(0, 5000));
            e.Graphics.DrawLine(pen, yTop.X, yTop.Y, yBot.X, yBot.Y);

            var xTop = Camera.Main.CoordinateToPoint(new Vector3(-5000, 0));
            var xBot = Camera.Main.CoordinateToPoint(new Vector3(5000, 0));
            e.Graphics.DrawLine(pen, xTop.X, xTop.Y, xBot.X, xBot.Y);

            var center = new PointF(Width / 2f, Height / 2f);
            e.Graphics.DrawLine(pen, center.X, center.Y - 10, center.X, center.Y + 10);
            e.Graphics.DrawLine(pen, center.X - 10, center.Y, center.X + 10, center.Y);
        }

        readonly MouseButtons _cameraMouseDragButton = MouseButtons.Middle;

        Stopwatch _timer;
        TimeSpan _elapsed;

        bool _isMouseDragging;
        Vector3 _mouseStartDragCoord;
        Vector3 _cameraStartDragCoord;
    }
}
