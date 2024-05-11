using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.GameSystem.Render
{
    public class SpriteDrawRequest : DrawRequestBase
    {
        public SpriteDrawRequest(SpriteDrawArgs spriteDrawArgs)
        {
            _spriteDrawArgs = spriteDrawArgs;
        }

        public override void DrawRequest(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawWorld(_spriteDrawArgs);
        }

        SpriteDrawArgs _spriteDrawArgs;

        public SpriteDrawArgs SpriteDrawArgs { get => _spriteDrawArgs; }
    }
}
