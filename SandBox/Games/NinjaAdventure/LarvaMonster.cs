using CruZ.GameEngine;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.Utility;

using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Collision.ContactSystem;

using Microsoft.Xna.Framework;
using System.Diagnostics;
using CruZ.GameEngine.GameSystem.Render;
using MonoGame.Extended.BitmapFonts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace NinjaAdventure
{
    internal class LarvaMonster : IDisposable
    {
        public LarvaMonster(GameScene scene, SpriteRendererComponent spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            
            _stunSoundFx = GameApplication.Resource.Load<SoundEffect>("sound\\larva-hit.mp3");

            Entity = scene.CreateEntity();

            _animation = new AnimationComponent(spriteRenderer);
            {
                _animation.FitToWorldUnit = true;
                _animation.Scale = new Vector2(0.7f, 0.7f);
                _animation.LoadAnimationFile("anim\\Larva\\Larva.aseprite");
            }
            Entity.AddComponent(_animation);

            var scriptComponent = new ScriptComponent();
            {
                scriptComponent.Updating += ScriptComponent_Updating;
            }
            Entity.AddComponent(scriptComponent);

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.3f, 1, _physic.Body);
                _physic.Body.UserData = this;
                _physic.BodyType = BodyType.Dynamic;
                _physic.IsSensor = true;
                _physic.OnCollision += Physic_OnCollision;
            }
            Entity.AddComponent(_physic);

            _health = new HealthComponent(30, spriteRenderer);
            {

            }
            Entity.AddComponent(_health);
        }

        private void ScriptComponent_Updating(GameTime gameTime)
        {
            if (_stunData.IsStunned)
            {
                UpdateStun(gameTime);
            }
            else
            {
                UpdateChasingTarget(gameTime);
            }

            UpdateAnimation();

            if(_isUseless) BecomeUseless?.Invoke(this);
        }

        private void UpdateStun(GameTime gameTime)
        {
            var stunDirection = _physic.Position - _stunData.HitPosition;

            if(stunDirection.SqrMagnitude() > 0.01f)
                stunDirection.Normalize();
            else
                stunDirection = Vector2.UnitX;

            _physic.LinearVelocity = stunDirection * _stunData.Speed;
            _stunData.Speed *= 0.85f;
            if (_stunData.Speed < 0.5) _stunData.Speed = 0.5f;

            _stunData.Timer += gameTime.DeltaTime();

            if (_stunData.Timer >= STUN_DURATION)
            {
                _stunData.IsStunned = false;
                _physic.LinearVelocity = Vector2.Zero;
            }
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is Suriken)
            {
                OnGetHit(fixtureB);
            }
        }

        private void OnGetHit(Fixture attackerFixture)
        {
            _stunData.HitPosition = attackerFixture.Body.Position;
            _stunData.IsStunned = true;
            _stunData.Timer = 0;
            _stunData.Speed = 10f;

            _health.Current -= 5;
            if(_health.Current == 0) _isUseless = true;

            _stunSoundFx.Play();
        }

        public event Action<LarvaMonster>? BecomeUseless;

        bool _isUseless = false;

        private void UpdateChasingTarget(GameTime gameTime)
        {
            Vector2 followDir = Follow != null ?
                            Follow.Position - Entity.Transform.Position :
                            Vector2.Zero;

            _facingDir =
                Vector2.Rotate(Vector2.UnitY, Entity.Transform.Rotation);

            if (followDir.Length() > 0.01) followDir.Normalize();
            _facingDir.Normalize();

            var rotationDir = MathF.Sign(FunMath.GetAngleBetween(_facingDir, followDir));

            _physic.Rotation += rotationDir * _rotationSpeed * gameTime.DeltaTime();
            _physic.Position += _facingDir * _speed * gameTime.DeltaTime();
        }

        private void UpdateAnimation()
        {
            _facingString ??= "front";
            _facingString = AnimationHelper.GetFacingDirectionString(_facingDir, _facingString);
            _animation.Play($"walk-{_facingString}");
        }

        public TransformEntity Entity { get; private set; }

        public Transform? Follow { get; set; }

        AnimationComponent _animation;
        SpriteRendererComponent _spriteRenderer;
        PhysicBodyComponent _physic;
        HealthComponent _health;

        const float STUN_DURATION = 0.6f;
        record struct StunData(bool IsStunned, float Timer, float Speed, Vector2 HitPosition);
        StunData _stunData;
        SoundEffect _stunSoundFx;

        float _speed = 1;
        float _rotationSpeed = 3.14f;

        private string? _facingString;
        private Vector2 _facingDir;

        public void Dispose()
        {
            Entity.Dispose();
            _physic.OnCollision -= Physic_OnCollision;
            BecomeUseless = default;
        }
    }
}
