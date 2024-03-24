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
            _tex = GameContext.GameResource.Load<Texture2D>("imgs\\GAP\\Flame01.png");
        }

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Matrix viewProjectionMatrix)
        {
            _fx.Parameters["view_projection"].SetValue(viewProjectionMatrix);
            _fx.Parameters["intensity"].SetValue(0.5f);
            _fx.Parameters["flame_color"].SetValue(new Vector4(4, 1.25f, 0.6f, 0.8f) / 4f);

            DrawArgs drawArgs = new();
            drawArgs.Apply(AttachedEntity);
            drawArgs.Apply(_tex);

            spriteBatch.Begin(effect: _fx);
            spriteBatch.Draw(drawArgs);
            spriteBatch.End();

            //_fx.CurrentTechnique
        }

        Effect _fx;
        Texture2D _tex;
    }
}
