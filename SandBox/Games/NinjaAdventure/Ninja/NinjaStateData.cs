using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.StateMachine;

using Microsoft.Xna.Framework;

namespace NinjaAdventure.Ninja
{
    internal class NinjaStateData : StateData
    {
        public PhysicBodyComponent Physic;
        public NinjaCharacter Character;
        public AnimationComponent Animation;
        public SpriteRendererComponent SpriteRenderer;
        public HealthComponent Health;

        public Vector2 LastInputMovement;

        public void Reset()
        {
            LastInputMovement = Vector2.Zero;
        }

        public string GetFacingString()
        {
            return AnimationHelper.GetFacingDirectionString(LastInputMovement);
        }
    }
}
