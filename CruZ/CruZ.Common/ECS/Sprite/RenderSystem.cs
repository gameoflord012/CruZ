using CruZ.Common.ECS.Ultility;
using CruZ.Common.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Framework.Content.Pipeline.Builder;

using System.Collections.Generic;
using System.Linq;

namespace CruZ.Common.ECS
{
    internal class RenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        public RenderSystem() : base(Aspect.One(
            typeof(SpriteComponent), typeof(LightComponent)))
        {
            
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<SpriteComponent>();
            _lightMapper = mapperService.GetMapper<LightComponent>();
            _spriteBatch = GameApplication.GetSpriteBatch();
        }

        public void Draw(GameTime gameTime)
        {
            List<SpriteComponent> sprites = GetSortedSprites();
            List<LightComponent> lights = GetSortedLights();

            List<int> sortingLayers = [];
            sortingLayers.AddRangeUnique(sprites.Select(e => e.SortingLayer).ToList());
            sortingLayers.AddRangeUnique(lights.Select(e => e.SortingLayer).ToList());
            sortingLayers.Sort();

            foreach(var sortingLayer in sortingLayers)
            {
                var fx = EffectManager.NormalSpriteRenderer;
                fx.Parameters["view_projection"].SetValue(Camera.Main.ViewProjectionMatrix());

                // render sprites
                _spriteBatch.Begin(
                    effect: fx,
                    sortMode: SpriteSortMode.FrontToBack,
                    samplerState: SamplerState.PointClamp);

                while (sprites.Count > 0 && sprites[0].SortingLayer == sortingLayer)
                {
                    sprites[0].Render(gameTime, _spriteBatch, Camera.Main.ViewProjectionMatrix());
                    sprites.RemoveAt(0);
                }

                _spriteBatch.End();

                // render light
                while (lights.Count > 0 && lights[0].SortingLayer == sortingLayer)
                {
                    lights[0].Render(gameTime, _spriteBatch, Camera.Main.ViewProjectionMatrix());
                    lights.RemoveAt(0);
                }
            }
        }

        private List<SpriteComponent> GetSortedSprites()
        {
            List<SpriteComponent> sprites = this.GetAllComponents(_spriteMapper);
            sprites.Sort((s1, s2) => { return s1.SortingLayer.CompareTo(s2.SortingLayer); });
            return sprites;
        }

        private List<LightComponent> GetSortedLights()
        {
            var lights = this.GetAllComponents(_lightMapper);
            lights.Sort((s1, s2) => { return s1.SortingLayer.CompareTo(s2.SortingLayer); });
            return lights;
        }

        public virtual void Update(GameTime gameTime) { }

        SpriteBatch _spriteBatch;
        ComponentMapper<LightComponent> _lightMapper;
        ComponentMapper<SpriteComponent> _spriteMapper;
    }
}
