using CruZ.Components;
using CruZ.Global;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CruZ.Systems
{
    internal class RenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        public RenderSystem() : base(Aspect.All(typeof(SpriteComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteRendererMapper = mapperService.GetMapper<SpriteComponent>();
            _spriteBatch = GameApplication.GetSpriteBatch();
            _gd = GameApplication.GetGraphicsDevice();
        }

        public void Draw(GameTime gameTime)
        {
            List<SpriteComponent> sprites = GetSortedSpriteList();
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

                _spriteBatch.Begin(
                    sortMode: SpriteSortMode.FrontToBack,
                    transformMatrix: Camera.Main.ViewMatrix(),
                    samplerState: SamplerState.PointClamp);
                do
                {
                    sprites[i].Draw(_spriteBatch, Camera.Main.ViewMatrix());
                    i++;
                } while (
                    i < sprites.Count &&
                    sprites[i].SortingLayer == sprites[i - 1].SortingLayer);

                _spriteBatch.End();
            }

            // render all renderTargets to back buffer
            _gd.SetRenderTarget(null);
            _gd.Clear(Variables.DEFAULT_BACKGROUND_COLOR);
            _spriteBatch.Begin();
            foreach (var sortingLayer in sortingLayers)
            {
                var renderTarget = GetRenderTarget(sortingLayer);
                _spriteBatch.Draw(renderTarget, new Vector2(0, 0), Color.White);
            }
            _spriteBatch.End();
        }

        private RenderTarget2D GetRenderTarget(int sortingLayer)
        {
            if(!_renderTargets.ContainsKey(sortingLayer))
                _renderTargets[sortingLayer] = new RenderTarget2D(_gd, _gd.Viewport.Width, _gd.Viewport.Height);
        
            return _renderTargets[sortingLayer];
        }

        private List<SpriteComponent> GetSortedSpriteList()
        {
            List<SpriteComponent> sprites = this.GetAllComponents(_spriteRendererMapper).ToList();
            sprites.Sort((s1, s2) => { return s1.SortingLayer.CompareTo(s2.SortingLayer); });
            return sprites;
        }

        public virtual void Update(GameTime gameTime) { }

        SpriteBatch _spriteBatch;
        ComponentMapper<SpriteComponent> _spriteRendererMapper;
        Dictionary<int, RenderTarget2D> _renderTargets = [];
        GraphicsDevice _gd;
    }
}
