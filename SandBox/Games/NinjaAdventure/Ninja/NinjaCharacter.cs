﻿using System.Collections.Immutable;

using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Animation;
using CruZ.GameEngine.GameSystem.ECS;
using CruZ.GameEngine.GameSystem.Physic;
using CruZ.GameEngine.GameSystem.Scene;
using CruZ.GameEngine.GameSystem.Script;
using CruZ.GameEngine.GameSystem.StateMachine;

using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;

using NinjaAdventure.Ninja;

namespace NinjaAdventure
{
    internal class NinjaCharacter : IDisposable
    {
        public NinjaCharacter(GameScene scene)
        {
            _gameScene = scene;
            Entity = scene.CreateEntity("Ninja");

            _spriteRenderer = new SpriteRendererComponent();
            {

            }
            Entity.AddComponent(_spriteRenderer);

            _animationComponent = new AnimationComponent(_spriteRenderer);
            {
                _animationComponent.FitToWorldUnit = true;
                _animationComponent.LoadAnimationFile("anim\\Ninja\\NinjaAnim.aseprite");
            }
            Entity.AddComponent(_animationComponent);

            _scriptComponent = new ScriptComponent();
            {
                _scriptComponent.Updating += Script_Updating;
            }
            Entity.AddComponent(_scriptComponent);

            _physic = new PhysicBodyComponent();
            {
                FixtureFactory.AttachCircle(0.4f, 1, _physic.Body);
                _physic.BodyType = BodyType.Dynamic;
                _physic.IsSensor = true;
                _physic.OnCollision += Physic_OnCollision;
                _physic.OnSeperation += Physic_OnSeperation;
            }
            Entity.AddComponent(_physic);

            _health = new HealthComponent(30, _spriteRenderer);
            {

            }
            Entity.AddComponent(_health);

            _machine = new StateMachineComponent();
            {
                _stateData = new NinjaStateData();
                _stateData.Physic = _physic;
                _stateData.Animation = _animationComponent;
                _stateData.Health = _health;
                _stateData.SpriteRenderer = _spriteRenderer;
                _stateData.NinjaCharacter = this;

                _machine.InjectedStateData = _stateData;
                _machine.Add(new NinjaAttackState());
                _machine.Add(new NinjaMovingState());
                _machine.Add(new NinjaHitState());
                _machine.Add(new NinjaDieState());
            }
            Entity.AddComponent(_machine);

            _machine.SetNextState(typeof(NinjaMovingState), false);
        }

        internal void SpawnSuriken(Vector2 direction)
        {
            var suriken = new Suriken(_gameScene, _spriteRenderer, Entity.Position, direction);
            surikens.Add(suriken);
        }

        private void Physic_OnSeperation(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (IsMonster(fixtureB))
            {
                _stateData.MonsterCount--;
            }
        }

        private void Physic_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (IsMonster(fixtureB))
            {
                _stateData.MonsterCount++;
                _stateData.LastMonsterBody = fixtureB.Body;
            }
        }

        private static bool IsMonster(Fixture fixtureB)
        {
            return fixtureB.Body.UserData is LarvaMonster;
        }

        private void Script_Updating(GameTime gameTime)
        {
            ProcessUselessSuriken();
        }

        private void ProcessUselessSuriken()
        {
            foreach (var suriken in surikens.ToImmutableArray())
            {
                if (suriken.IsUseless)
                {
                    suriken.Dispose();
                    surikens.Remove(suriken);
                }
            }
        }

        PhysicBodyComponent _physic;
        ScriptComponent _scriptComponent;
        HealthComponent _health;
        StateMachineComponent _machine;
        SpriteRendererComponent _spriteRenderer;

        AnimationComponent _animationComponent;
        List<Suriken> surikens = [];

        GameScene _gameScene;

        public TransformEntity Entity;
        private NinjaStateData _stateData;

        public void Dispose()
        {
            _physic.OnCollision -= Physic_OnCollision;
            _scriptComponent.Updating -= Script_Updating;

            foreach (var uselessSuriken in surikens)
            {
                uselessSuriken.Dispose();
            }
        }
    }
}