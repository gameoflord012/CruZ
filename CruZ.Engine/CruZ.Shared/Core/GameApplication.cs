using CruZ.Systems;
using CruZ.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CruZ
{
    public class GameApplication: 
        IECSContextProvider, IInputContextProvider, IApplicationContextProvider, UIContext
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

            ApplicationContext  .SetContext(this);
            ECS                 .SetContext(this);
            Input               .SetContext(this);

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

        public event Action<GameTime> DrawUI;
        public event Action<GameTime> UpdateUI;

        event Action<GameTime>  IECSContextProvider.ECSDraw               { add { _core.DrawEvent             += value; } remove { _core.DrawEvent            -= value; } }
        event Action<GameTime>  IECSContextProvider.ECSUpdate             { add { _core.UpdateEvent           += value; } remove { _core.UpdateEvent          -= value; } }
        event Action            IECSContextProvider.InitializeECSSystem   { add { _core.InitializeSystemEvent += value; } remove { _core.InitializeSystemEvent-= value; } }
        event Action<GameTime>  IInputContextProvider.UpdateInputEvent      { add { _core.UpdateEvent           += value; } remove { _core.UpdateEvent          -= value; } }
    }
}
