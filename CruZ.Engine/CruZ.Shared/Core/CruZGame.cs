using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using System;

namespace CruZ
{
    public abstract class CruZGame
    {
        public CruZGame()
        {
            _core = CruZ.Instance();

            _core.OnInitialize += Initialize;
            _core.OnUpdate += Update;
            _core.OnLoadContent += LoadContent;
            _core.OnEndRun += EndRun;
            _core.OnExit += OnExit;
            _core.OnDraw += Draw;

            _core.Run();
        }

        protected virtual void Draw(GameTime gameTime) { }
        protected virtual void OnExit(object sender, EventArgs args) { }
        protected virtual void EndRun() { }
        protected virtual void LoadContent() { }
        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }

        public CruZ Core { get => _core; }
        public World World { get => _core.World; }
        public ContentManager Content { get => _core.Content; }
        public GraphicsDevice GraphicsDevice { get => _core.GraphicsDevice; }

        CruZ _core;
    }
}
