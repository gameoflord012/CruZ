using CruZ.Framework;
using CruZ.Framework.GameSystem.Render;

using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Framework.Utility
{
    public class EffectManager
    {
        public static Effect NormalSpriteRenderer
        {
            get
            {
                if (_normalSpriteRenderer == null || _normalSpriteRenderer.IsDisposed)
                {
                    _normalSpriteRenderer = GameContext.GameResource.Load<Effect>(".internal\\normal-sprite.fx");
                    GameApplication.Disposables.Add(_normalSpriteRenderer);
                }
                return _normalSpriteRenderer;
            }
        }

        public static Effect TextureLight
        {
            get
            {
                if (_texutreLight == null || _texutreLight.IsDisposed)
                {
                    _texutreLight = GameContext.GameResource.Load<Effect>(".internal\\texture-light.fx");
                    GameApplication.Disposables.Add(_texutreLight);
                }
                return _texutreLight;
            }
        }

        public static Effect Bloom
        {
            get
            {
                if (_bloom == null || _bloom.IsDisposed)
                {
                    _bloom = GameContext.GameResource.Load<Effect>(".internal\\bloom.fx");
                    GameApplication.Disposables.Add(_bloom);
                }
                return _bloom;
            }
        }

        public static Effect ReinhardTonemap
        {
            get
            {
                if (_reinhardTonemap == null || _reinhardTonemap.IsDisposed)
                {
                    _reinhardTonemap = GameContext.GameResource.Load<Effect>(".internal\\reinhard-tonemap.fx");
                    GameApplication.Disposables.Add(_reinhardTonemap);
                }
                return _reinhardTonemap;
            }
        }

        static Effect? _guassianBloom;
        static Effect? _normalSpriteRenderer;
        static Effect? _texutreLight;
        static Effect? _bloom;
        static Effect? _reinhardTonemap;
    }
}
