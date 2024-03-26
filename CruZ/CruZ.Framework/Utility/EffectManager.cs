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
                    _bloom = GameContext.GameResource.Load<Effect>(".internal\\Bloom.fx");
                    GameApplication.Disposables.Add(_bloom);
                }
                return _bloom;
            }
        }

        public static Effect GaussianBloom
        {
            get
            {
                if (_guassianBloom == null || _guassianBloom.IsDisposed)
                {
                    _guassianBloom = GameContext.GameResource.Load<Effect>(".internal\\guassian-bloom.fx");
                    GameApplication.Disposables.Add(_guassianBloom);
                }
                return _guassianBloom;
            }
        }

        static Effect? _guassianBloom;
        static Effect? _normalSpriteRenderer;
        static Effect? _texutreLight;
        static Effect? _bloom;
    }
}
