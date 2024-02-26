using Assimp;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CruZ.Experiment
{
    class MyGame : Microsoft.Xna.Framework.Game
    {
        public MyGame()
        {
            IsMouseVisible = true;
            _gdManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = ".\\Content\\bin";
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _lightTexture = Content.Load<Texture2D>("homelander");
            _lightEffect = Content.Load<Effect>("shaders\\light-shader");
        }

        protected override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new(GraphicsDevice);
            _renderTarget = new(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 dir = new(0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                dir = new(-1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                dir = new(1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                dir = new(0, -1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                dir = new(0, 1);
            }

            _position += dir * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            int zoom = 2;
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
            Matrix view = Matrix.Identity;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, width / zoom, height / zoom, 0, 0, 1);

            _lightEffect.Parameters["view_projection"]?.SetValue(view * projection);
            _lightEffect.Parameters["LightPosition"]?.SetValue(_position);
            _lightEffect.Parameters["LightRadius"]?.SetValue(1f);
            _lightEffect.Parameters["LightColor"]?.SetValue(new Vector4(0, 0, 1, 1));
             
            _spriteBatch.Begin(effect: _lightEffect);
            _spriteBatch.Draw(_lightTexture, new Vector2(0, 0), Color.Red);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.RoyalBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
        }
        
        Texture2D _lightTexture;
        SpriteBatch _spriteBatch;
        Effect _lightEffect;
        GraphicsDeviceManager _gdManager;
        RenderTarget2D _renderTarget;
        Vector2 _position;
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Microsoft.Xna.Framework.Game game = new MyGame();
            game.Run();
        }
    }
}
