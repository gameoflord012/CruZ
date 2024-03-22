﻿using System;

using CruZ.Common;
using CruZ.Common.ECS;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.AnimalGang.DesktopGL
{
    public class FlameRendererComponent : RendererComponent
    {
        public override Type ComponentType => typeof(RendererComponent);

        public FlameRendererComponent()
        {
            _fx = GameContext.GameResource.Load<Effect>("shaders\\flame-shader.fx");
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            _fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);

            spriteBatch.Begin(effect: _fx);
            spriteBatch.Draw(_tex, AttachedEntity.Position, Color.White);
            spriteBatch.End();
        }

        Effect _fx;
        Texture2D _tex;
    }
}
