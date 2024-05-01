﻿using CruZ.GameEngine;
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

namespace NinjaAdventure
{
    internal class LarvaMonster : IDisposable
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

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.5f, 1, _physic.Body);
                _physic.BodyType = BodyType.Kinematic;
                _physic.IsSensor = true;
                _physic.OnCollision += Physic_OnCollision;
            }
            Entity.AddComponent(_physic);
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
        }

        private void UpdateStun(GameTime gameTime)
        {
            var stunDirection = _physic.Position - _stunData.HitPosition;
            Debug.Assert(stunDirection.SqrMagnitude() > 0.01f);
            stunDirection.Normalize();

            _physic.LinearVelocity = stunDirection * _stunData.Speed;
            _stunData.Speed *= 0.85f;
            if (_stunData.Speed < 0.5) _stunData.Speed = 0.5f;

            _stunData.Timer += gameTime.GetElapsedSeconds();

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
                _stunData.HitPosition = fixtureB.Body.Position;
                _stunData.IsStunned = true;
                _stunData.Timer = 0;
                _stunData.Speed = 10f;
            }
        }

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

            _physic.Rotation += rotationDir * _rotationSpeed * gameTime.GetElapsedSeconds();
            _physic.Position += _facingDir * _speed * gameTime.GetElapsedSeconds();
        }

        private void UpdateAnimation()
        {
            _facingString ??= "front";
            _facingString = AnimationHelper.GetFacingDirectionString(_facingDir, _facingString);
            _animation.PlayAnimation($"walk-{_facingString}");
            _animation.Transform.Position = Entity.Transform.Position;
        }

        public TransformEntity Entity { get; private set; }

        public Transform? Follow { get; set; }

        AnimationComponent _animation;

        PhysicBodyComponent _physic;

        const float STUN_DURATION = 0.6f;

        record struct StunData(bool IsStunned, float Timer, float Speed, Vector2 HitPosition);
        StunData _stunData;

        float _speed = 1;
        float _rotationSpeed = 3.14f;

        private string? _facingString;
        private Vector2 _facingDir;

        public void Dispose()
        {
            Entity.Dispose();
            _physic.OnCollision -= Physic_OnCollision;
        }
    }
}
