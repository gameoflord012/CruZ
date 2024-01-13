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

namespace CruZ.Editor
{
    internal class WorldViewControl : MonoGame.Forms.NET.Controls.MonoGameControl, IECSContextProvider, IApplicationContextProvider, IInputContextProvider
    {
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

            Pen pen = new Pen(System.Drawing.Color.FromArgb(255, 0, 0, 0));
        }

        Stopwatch _timer;
        TimeSpan _elapsed;

        public event Action<GameTime>   DrawEvent;
        public event Action<GameTime>   UpdateEvent;
        public event Action             InitializeSystemEvent;
        public event Action<GameTime>   UpdateInputEvent { add { UpdateEvent += value; } remove { UpdateEvent -= value; } }

        public GraphicsDevice GraphicsDevice    => Editor.GraphicsDevice;
        public ContentManager Content           => Editor.Content;
    }
}
