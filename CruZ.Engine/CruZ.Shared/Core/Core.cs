using CruZ.Resource;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace CruZ
{
    public delegate void CruZ_UpdateDelegate(GameTime gameTime);
    public delegate void ActionDelegate();
    public delegate void OnExitingDelegate(object sender, EventArgs args);

    public partial class Core : Game
    {
        private Core()
        {
            Content.RootDirectory = ResourceManager.CONTENT_ROOT;
            IsMouseVisible = true;

            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void EndRun()
        {
            base.EndRun();
            OnEndRun?.Invoke();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            OnExit?.Invoke(sender, args);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            OnLoadContent?.Invoke();
        }

        protected override void Initialize()
        {
            base.Initialize();
            OnInitialize?.Invoke();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            OnUpdate?.Invoke(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            InternalDraw(gameTime);
        }

        private void InternalDraw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            OnDraw?.Invoke(gameTime);
            OnLateDraw?.Invoke(gameTime);
        }

        private GraphicsDeviceManager _graphics;
    }
}
