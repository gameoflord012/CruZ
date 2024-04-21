//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace CruZ.GameEngine.GameSystem.Animation
//{
//    public class AnimatedSprite
//    {
//        public AnimatedSprite(Texture2D texture, int width, int height, float frameDuration = 0.2f)
//        {
//            _atlas = TextureAtlas.Create(texture, width, height);
//            _frameDuration = frameDuration;
//        }

//        public void Update(GameTime gameTime)
//        {
//            _currentTime = gameTime.GetElapsedSeconds();
//        }

//        public TextureRegion2D GetCurrentFrame()
//        {
//            int frameIndex = GetCurrentFrameIndex();
//            return _atlas.GetRegion(frameIndex % _atlas.RegionCount);
//        }

//        private int GetCurrentFrameIndex()
//        {
//            return (int)(_currentTime / _frameDuration);
//        }

//        TextureAtlas _atlas;
//        float _frameDuration; // in seconds
//        float _currentTime; // in seconds
//    }
//}
