using System;

using CruZ.Framework;
using CruZ.Framework.GameSystem.Render;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.AnimalGang.DesktopGL
{
    public class FlameRendererComponent : RendererComponent
    {
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
