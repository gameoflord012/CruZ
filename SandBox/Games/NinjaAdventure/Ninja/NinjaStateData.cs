using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;

using Genbox.VelcroPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Ninja
{
    internal class NinjaStateData : StateData
    {
        public PhysicBodyComponent Physic;
        public NinjaCharacter NinjaCharacter;
        public AnimationComponent Animation;
        public SpriteRendererComponent SpriteRenderer;
        public HealthComponent Health;

        public int MonsterCount;
        public Body LastMonsterBody;
        public Vector2 LastInputMovement;

        public string GetFacingString()
        {
            return AnimationHelper.GetFacingDirectionString(LastInputMovement);
        }
    }
}
