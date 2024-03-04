using CruZ.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using System.Collections.Generic;
using System.Linq;

namespace CruZ.Systems
{
    internal class RenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        public RenderSystem() : base(Aspect.One(
            typeof(SpriteComponent), typeof(LightComponent)))
        {
            GameApplication.RegisterWindowResize(GameApp_WindowResize);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<SpriteComponent>();
            _lightMapper = mapperService.GetMapper<LightComponent>();
            _spriteBatch = GameApplication.GetSpriteBatch();
            _gd = GameApplication.GetGraphicsDevice();
            _lightEffect = GameContext.GameResource.Load<Effect>("lightshader.fx");
        }

        public void Draw(GameTime gameTime)
        {
            List<SpriteComponent> sprites = GetSortedSpriteList();
            List<LightComponent> lights = _lightMapper.Components.ToList();
            List<int> sortingLayers = [];

            // process sprites
            int i = 0;
            while(i < sprites.Count)
            {
                var sortingLayer = sprites[i].SortingLayer;
                sortingLayers.Add(sortingLayer);

                var renderTarget = GetRenderTarget(sortingLayer);

                _gd.SetRenderTarget(renderTarget);
                _gd.Clear(Color.Transparent);

                #region Process Sprites
                // render sprite
                _spriteBatch.Begin(
                            sortMode: SpriteSortMode.FrontToBack,
                            transformMatrix: Camera.Main.ViewMatrix(),
                            samplerState: SamplerState.PointClamp);
                do
                {
                    sprites[i].InternalDraw(_spriteBatch, Camera.Main.ViewMatrix());
                    i++;
                } while (
                    i < sprites.Count &&
                    sprites[i].SortingLayer == sprites[i - 1].SortingLayer);

                _spriteBatch.End();
                #endregion

                // render lights
                _spriteBatch.Begin(effect: _lightEffect);
                foreach (var light in lights
                    .Where(e => e.SortingLayers.Contains(sortingLayer)))
                {
                    light.InternalDraw(gameTime);
                }
                _spriteBatch.End();
            }

            // render all renderTargets to back buffer
            _gd.SetRenderTarget(null);
            _gd.Clear(GameConstants.DEFAULT_BACKGROUND_COLOR);
            _spriteBatch.Begin();
            foreach (var sortingLayer in sortingLayers)
            {
                var renderTarget = GetRenderTarget(sortingLayer);
                _spriteBatch.Draw(renderTarget, new Vector2(0, 0), Color.White);
            }
            _spriteBatch.End();
        }

        private void GameApp_WindowResize(Viewport viewport)
        {
            foreach (var renderTarget in _renderTargets.Values) 
                renderTarget.Dispose();
            _renderTargets.Clear();
        }

        private RenderTarget2D GetRenderTarget(int sortingLayer)
        {
            if(!_renderTargets.ContainsKey(sortingLayer))
                _renderTargets[sortingLayer] = new RenderTarget2D(_gd, _gd.Viewport.Width, _gd.Viewport.Height);
            return _renderTargets[sortingLayer];
        }

        private List<SpriteComponent> GetSortedSpriteList()
        {
            List<SpriteComponent> sprites = this.GetAllComponents(_spriteMapper).ToList();
            sprites.Sort((s1, s2) => { return s1.SortingLayer.CompareTo(s2.SortingLayer); });
            return sprites;
        }

        public virtual void Update(GameTime gameTime) { }

        SpriteBatch _spriteBatch;
        ComponentMapper<LightComponent> _lightMapper;
        ComponentMapper<SpriteComponent> _spriteMapper;
        Dictionary<int, RenderTarget2D> _renderTargets = [];
        GraphicsDevice _gd;
        Effect _lightEffect;
    }
}
