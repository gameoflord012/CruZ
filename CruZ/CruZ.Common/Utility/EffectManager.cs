using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.Utility
{
    class EffectManager
    {
        public static Effect NormalSpriteRenderer => _normalSpriteRenderer ??= GameContext.GameResource.Load<Effect>("internal\\normal-sprite.fx");
        public static Effect TextureLight => _texutreLight ??= GameContext.GameResource.Load<Effect>("internal\\texture-light.fx");

        static Effect? _normalSpriteRenderer;
        static Effect? _texutreLight;
    }
}
