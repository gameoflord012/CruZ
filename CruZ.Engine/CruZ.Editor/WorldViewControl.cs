using CruZ.Resource;
using CruZ.Scene;
using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace CruZ.Editor
{
    internal class WorldViewControl : MonoGame.Forms.NET.Controls.MonoGameControl, IECSContextProvider, IApplicationContextProvider
    {
        public WorldViewControl()
        {
            ECS.CreateContext(this);
            ApplicationContext.CreateContext(this);
        }

        protected override void Initialize()
        {
            _timer = new();
            _timer.Start();
            _elapsed = _timer.Elapsed;

            var scene = SceneManager.SceneAssets.Values.First();
            ResourceManager.InitResource("scenes\\scene1.scene", scene, true);
            scene.Dispose();

            scene = ResourceManager.LoadResource<GameScene>("scenes\\scene1.scene");
            scene.SetActive(true);

            InitializeSystemEvent.Invoke();
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

        Stopwatch _timer;
        TimeSpan _elapsed;

        public event Action<GameTime>   DrawEvent;
        public event Action<GameTime>   UpdateEvent;
        public event Action             InitializeSystemEvent;

        public GraphicsDevice GraphicsDevice    => Editor.GraphicsDevice;
        public ContentManager Content           => Editor.Content;
    }
}
