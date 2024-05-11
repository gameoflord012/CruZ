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
                    _normalSpriteRenderer = GameApplication.Resource.Load<Effect>(".internal\\normal-sprite.fx");
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
                    _texutreLight = GameApplication.Resource.Load<Effect>(".internal\\texture-light.fx");
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
                    _bloom = GameApplication.Resource.Load<Effect>(".internal\\bloom.fx");
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
                    _postprocessing = GameApplication.Resource.Load<Effect>(".internal\\postprocessing.fx");
                }
                return _postprocessing;
            }
        }

        static Effect? _normalSpriteRenderer;
        static Effect? _texutreLight;
        static Effect? _bloom;
        static Effect? _postprocessing;
    }
}
