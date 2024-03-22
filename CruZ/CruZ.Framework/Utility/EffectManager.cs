using CruZ.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.Utility
{
    class EffectManager
    {
        public static Effect NormalSpriteRenderer
        {
            get
            {
                if (_normalSpriteRenderer == null || _normalSpriteRenderer.IsDisposed)
                    _normalSpriteRenderer = GameContext.GameResource.Load<Effect>("internal\\normal-sprite.fx");
                return _normalSpriteRenderer;
            }
        }
        public static Effect TextureLight
        {
            get
            {
                if (_texutreLight == null || _texutreLight.IsDisposed)
                    _texutreLight = GameContext.GameResource.Load<Effect>("internal\\texture-light.fx");
                return _texutreLight;
            }
        }
        static Effect? _normalSpriteRenderer;
        static Effect? _texutreLight;
    }
}
