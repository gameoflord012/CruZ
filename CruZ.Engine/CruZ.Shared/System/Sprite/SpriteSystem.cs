using CruZ.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Collections.Generic;
using System.Linq;

namespace CruZ.Systems
{
    internal class SpriteSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        public SpriteSystem() : base(Aspect.All(typeof(SpriteComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteRendererMapper = mapperService.GetMapper<SpriteComponent>();
            _spriteBatch = new SpriteBatch(ApplicationContext.GraphicsDevice);
        }

        public void Draw(GameTime gameTime)
        {
            List<SpriteComponent> sprites = GetSortedSpriteList();

            int i = 0;
            while(i < sprites.Count)
            {
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
    }
}
