using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Factories;

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
                _animation.Transform = new();
                _animation.LoadAnimationFile("art\\Larva\\Larva.aseprite");
            }
            Entity.AddComponent(_animation);

            var scriptComponent = new ScriptComponent();
            {
                scriptComponent.Updating += ScriptComponent_Updating;
            }
            Entity.AddComponent(scriptComponent);

            var physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.5f, 1, physic.Body);
                physic.Body.IsSensor = true;
            }
            Entity.AddComponent(physic);
        }

        private void ScriptComponent_Updating(GameTime gameTime)
        {
            Vector2 followDir = Follow != null ? 
                Follow.Position - Entity.Transform.Position : 
                Vector2.Zero;

            var facingDir =  
                Vector2.Rotate(Vector2.UnitY, Entity.Transform.Rotation);

            if(followDir.Length() > 0.01) followDir.Normalize();
            facingDir.Normalize();

            var rotationDir = MathF.Sign(FunMath.GetAngleBetween(facingDir, followDir));

            Entity.Transform.Rotation += rotationDir * _rotationSpeed * gameTime.GetElapsedSeconds();
            Entity.Transform.Position += facingDir * _speed * gameTime.GetElapsedSeconds();
            //
            // animation    
            //
            _facingString ??= "front";
            _facingString = AnimationHelper.GetFacingDirectionString(facingDir, _facingString);
            _animation.PlayAnimation($"walk-{_facingString}");
            _animation.Transform.Position = Entity.Transform.Position;
        }

        public TransformEntity Entity { get; private set; }
       
        AnimationComponent _animation;

        public Transform? Follow { get; set; }

        float _speed = 1;
        float _rotationSpeed = 3.14f;
        private string? _facingString;
    }
}
