using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem.Render;
using CruZ.GameEngine;

using Microsoft.Xna.Framework.Graphics;

namespace CruZ.GameEngine.Utility
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

        public static Effect PostProcessing
        {
            get
            {
                if (_postprocessing == null || _postprocessing.IsDisposed)
                {
                    _postprocessing = GameContext.GameResource.Load<Effect>(".internal\\postprocessing.fx");
                    GameApplication.Disposables.Add(_postprocessing);
                }
                return _postprocessing;
            }
        }

        static Effect? _guassianBloom;
        static Effect? _normalSpriteRenderer;
        static Effect? _texutreLight;
        static Effect? _bloom;
        static Effect? _postprocessing;
    }
}
