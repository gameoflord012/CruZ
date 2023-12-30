using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using System;

namespace CruZ
{
    public abstract class GameApplication
    {
        public GameApplication()
        {
            Core.OnInitialize   += Initialize;
            Core.OnUpdate       += Update;
            Core.OnLoadContent  += LoadContent;
            Core.OnEndRun       += EndRun;
            Core.OnExit         += Exit;
            Core.OnDraw         += Draw;

            Core.Instance.Run();
        }

        protected virtual void  Draw(GameTime gameTime) { }
        protected virtual void  Exit(object sender, EventArgs args) { }
        protected virtual void  EndRun() { }
        protected virtual void  LoadContent() { }
        public virtual void     Initialize() { }
        public virtual void     Update(GameTime gameTime) { }

        public ContentManager Content { get => Core.Instance.Content; }
        public GraphicsDevice GraphicsDevice { get => Core.Instance.GraphicsDevice; }
    }
}
