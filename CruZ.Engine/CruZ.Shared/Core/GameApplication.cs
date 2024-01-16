using CruZ.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CruZ
{
    public abstract class GameApplication: IECSContextProvider, IInputContextProvider, IApplicationContextProvider
    {
        public ContentManager       Content         { get => _core.Content; }
        public GraphicsDevice       GraphicsDevice  { get => _core.GraphicsDevice; }

        public GameApplication()
        {
            _core = new();

            _core.InitializeEvent   += InternalInitialize;
            _core.UpdateEvent       += Update;
            _core.LoadContentEvent  += LoadContent;
            _core.EndRunEvent       += EndRun;
            _core.ExitEvent         += Exit;
            _core.DrawEvent         += Draw;
            _core.LateDrawEvent     += LateDraw;

            ApplicationContext  .CreateContext(this);
            ECS                 .CreateContext(this);
            Input               .CreateContext(this);

            _core.Run();
        }

        private void InternalInitialize()
        {
            Camera.Main = new Camera(GraphicsDevice.Viewport);
            Initialize();
        }

        protected virtual void  Initialize() { }
        protected virtual void  Update(GameTime gameTime) { }
        protected virtual void  Draw(GameTime gameTime) { }
        protected virtual void  LateDraw(GameTime gameTime) { }
        protected virtual void  Exit(object sender, EventArgs args) { }
        protected virtual void  EndRun() { }
        protected virtual void  LoadContent() { }

        private GameCore _core;

        event Action<GameTime>  IECSContextProvider.DrawEvent               { add { _core.DrawEvent             += value; } remove { _core.DrawEvent            -= value; } }
        event Action<GameTime>  IECSContextProvider.UpdateEvent             { add { _core.UpdateEvent           += value; } remove { _core.UpdateEvent          -= value; } }
        event Action            IECSContextProvider.InitializeSystemEvent   { add { _core.InitializeSystemEvent += value; } remove { _core.InitializeSystemEvent-= value; } }
        event Action<GameTime>  IInputContextProvider.UpdateInputEvent      { add { _core.UpdateEvent           += value; } remove { _core.UpdateEvent          -= value; } }
    }
}
