//using CruZ.GameEngine.GameSystem.Render;

//using Microsoft.Xna.Framework;

//using System.Collections.Generic;

//namespace CruZ.GameEngine.GameSystem.Animation
//{
//    /// <summary>
//    /// Play an animations
//    /// </summary>
//    public class AnimationPlayer
//    {
//        public AnimationPlayer(AnimatedSprite spriteSheet)
//        {
//            //_animatedSprite = AnimatedSprite(spriteSheet);
//        }

//        public void Play(string animationName)
//        {
//            try
//            {
//                //_animatedSprite.Play(animationName);
//            }
//            catch (KeyNotFoundException e)
//            {
//                throw new(string.Format("Cant found animation with key {0}", animationName), e);
//            }
//        }

//        internal void Update(GameTime gameTime)
//        {
//            //_animatedSprite.Update(gameTime);
//        }

//        internal void DoRequest(DefaultDrawArgs e)
//        {
//            //e.Texture = _animatedSprite.TextureRegion.Texture;
//            //e.SourceRectangle = _animatedSprite.TextureRegion.Bounds;
//            //e.NormalizedOrigin =
//            //    new(
//            //    _animatedSprite.OriginNormalized.X - 0.5f + e.NormalizedOrigin.X,
//            //    _animatedSprite.OriginNormalized.Y - 0.5f + e.NormalizedOrigin.Y);
//        }

//        //MonoGame.Extended.Sprites.AnimatedSprite _animatedSprite;
//    }
//}
