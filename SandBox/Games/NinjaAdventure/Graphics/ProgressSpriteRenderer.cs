using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Render;

using Microsoft.Xna.Framework.Graphics;

namespace NinjaAdventure.Graphics
{
    public class ProgressSpriteRenderer : SpriteRendererComponent
    {
        public ProgressSpriteRenderer()
        {
            _fx = GameApplication.Resource.Load<Effect>("effect\\normal-sprite.fx");
        }

        protected override Effect SetupEffect(RenderSystemEventArgs args)
        {
            _fx.Parameters["view_projection"].SetValue(args.ViewProjectionMatrix);
            return _fx;
        }

        Effect _fx;
    }
}
