//using CruZ.Common;
//using CruZ.Common.UI;
//using CruZ.GameEngine;
//using CruZ.GameEngine.GameSystem;
//using CruZ.GameEngine.GameSystem.UI;

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace CruZ.Experiment
//{
//    class CameraExperiment : GameWrapper
//    {
//        public CameraExperiment() : base()
//        {
//            IsMouseVisible = true;
//            Window.AllowUserResizing = true;
//            Content.RootDirectory = ".\\Content\\bin";
//            GameApplication.CreateContext(this);
//        }

//        protected override void LoadContent()
//        {
//            base.LoadContent();

//            _texture = Content.Load<Texture2D>("homelander");
//            _lightEffect = Content.Load<Effect>("shaders\\7dac5bbe-d4ad-493f-a2d7-7f3a00c95863");
//            _normalFx = Content.Load<Effect>("shaders\\normal-shader");
//        }

//        protected override void OnInitialize()
//        {
//            _spriteBatch = new(GraphicsDevice);
//            _renderTarget = new(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
//            _camera = new(Window);
//            _vp = GraphicsDevice.Viewport;

//            _camera.CameraOffset = new(0, 0);
//            _camera.Zoom = 1;

//            _uiControl = new UIControl();
//            UIManager.Root.AddChild(_uiControl);
//        }

//        protected override void OnUpdate(GameTime gameTime)
//        {
//            Vector2 dir = new(0, 0);

//            if (Keyboard.GetState().IsKeyDown(Keys.A))
//            {
//                dir = new(-1, 0);
//            }
//            if (Keyboard.GetState().IsKeyDown(Keys.D))
//            {
//                dir = new(1, 0);
//            }
//            if (Keyboard.GetState().IsKeyDown(Keys.W))
//            {
//                dir = new(0, -1);
//            }
//            if (Keyboard.GetState().IsKeyDown(Keys.S))
//            {
//                dir = new(0, 1);
//            }

//            _position += dir * (float)gameTime.ElapsedGameTime.TotalSeconds;
//        }

//        protected override void OnDraw(GameTime gameTime)
//        {
//            GraphicsDevice.Clear(Color.Pink);
//            var proj = _camera.ProjectionMatrix();
//            var view = _camera.ViewMatrix();

//            Point point = _camera.CoordinateToPoint(new(50, 50));

//            _spriteBatch.Begin();
//            _spriteBatch.DrawLine(0, 0, point.X, point.Y, Color.White);
//            _spriteBatch.End();

//            _uiControl.Location = point;
//            _uiControl.Width = 100;
//            _uiControl.Height = 100;

//            _normalFx.Parameters["view_projection"].SetValue(view * proj);

//            _spriteBatch.Begin(effect: _normalFx);
//            _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
//            _spriteBatch.End();
//        }

//        private void LightFxDemo()
//        {
//            GraphicsDevice.SetRenderTarget(_renderTarget);
//            GraphicsDevice.Clear(Color.Transparent);

//            int zoom = 2;
//            int width = GraphicsDevice.Viewport.Width;
//            int height = GraphicsDevice.Viewport.Height;
//            Matrix view = Matrix.Identity;
//            Matrix projection = Matrix.CreateOrthographicOffCenter(0, width / zoom, height / zoom, 0, 0, 1);

//            _lightEffect.Parameters["view_projection"]?.SetValue(view * projection);
//            _lightEffect.Parameters["LightPosition"]?.SetValue(_position);
//            _lightEffect.Parameters["LightRadius"]?.SetValue(1f);
//            _lightEffect.Parameters["LightColor"]?.SetValue(new Vector4(0, 0, 1, 1));

//            _spriteBatch.Begin(effect: _lightEffect);
//            _spriteBatch.Draw(_texture, new Vector2(0, 0), Color.Red);
//            _spriteBatch.End();

//            GraphicsDevice.SetRenderTarget(null);

//            GraphicsDevice.Clear(Color.RoyalBlue);
//            _spriteBatch.Begin();
//            _spriteBatch.Draw(_renderTarget, new Vector2(0, 0), Color.White);
//            _spriteBatch.End();
//        }

//        Texture2D _texture;
//        SpriteBatch _spriteBatch;
//        Effect _lightEffect;
//        Effect _normalFx;
//        GraphicsDeviceManager _gdManager;
//        Camera _camera;
//        RenderTarget2D _renderTarget;
//        UIControl _uiControl;
//        Vector2 _position;
//        Viewport _vp;
//    }
//}
