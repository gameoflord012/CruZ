using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace NinjaAdventure
{
    internal class LarvaMonster
    {
        public LarvaMonster(GameScene scene, SpriteRendererComponent spriteRenderer)
        {
            Entity = scene.CreateEntity();

            _animation = new AnimationComponent(spriteRenderer);
            {
                _animation.FitToWorldUnit = true;
                _animation.LoadAnimationFile("art\\Larva\\Larva.aseprite");
                _animation.PlayAnimation("walk-front");
            }
            Entity.AddComponent(_animation);

            var scriptComponent = new ScriptComponent();
            {
                scriptComponent.Updating += ScriptComponent_Updating;
            }
            Entity.AddComponent(scriptComponent);
        }

        private void ScriptComponent_Updating(GameTime gameTime)
        {
            Vector2 moveDir = Follow != null ? 
                Follow.Position - Entity.Transform.Position : 
                Vector2.Zero;

            if(moveDir.Length() > 0.01) moveDir.Normalize();

            Entity.Transform.Position += moveDir * _speed * gameTime.GetElapsedSeconds();
            Entity.Transform.Rotation = FunMath.GetRotation(-Vector2.UnitY, new Vector2(moveDir.X, -moveDir.Y));
        }

        public TransformEntity Entity { get; private set; }
       
        AnimationComponent _animation;
        public Transform? Follow { get; set; }

        float _speed = 1;
    }
}
