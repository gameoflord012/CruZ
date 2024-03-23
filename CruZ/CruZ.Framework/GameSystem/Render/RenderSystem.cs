using System.Linq;

using CruZ.Common;
using CruZ.Framework.GameSystem.ECS;

using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.GameSystem.Render
{
    internal class RenderSystem : EntitySystem
    {
        public override void Initialize()
        {
            _spriteBatch = GameApplication.GetSpriteBatch();
        }

        protected override void OnDraw(EntitySystemEventArgs args)
        {
            var renderers = args.Entity.GetAllComponents()
                .Where(e => e is RendererComponent)
                .Select(e => (RendererComponent)e)
                .ToList();

            renderers.Sort();

            foreach (var renderer in renderers)
                renderer.Render(
                    args.GameTime, _spriteBatch, Camera.Main.ViewProjectionMatrix());
        }

        SpriteBatch _spriteBatch;
    }
}
