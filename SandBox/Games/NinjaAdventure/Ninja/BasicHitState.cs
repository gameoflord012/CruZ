//using System.Diagnostics;
//using System.Linq.Expressions;

//using CruZ.GameEngine.GameSystem.Physic;
//using CruZ.GameEngine.GameSystem.StateMachine;
//using CruZ.GameEngine.Utility;

//using Genbox.VelcroPhysics.Dynamics;

//using Microsoft.Xna.Framework;

//namespace NinjaAdventure.Ninja
//{
//    internal abstract class BasicHitState<T> : BasicState<T> where T : StateData
//    {
//        protected float StunTime = 0.25f;
//        protected float StunForce = 10f;
//        protected float TimeBeetweenHit = 1f;

//        protected virtual bool IsGettingHit => HitBodies != null;

//        protected abstract Body? HitBodies { get; }

//        protected override bool CanTransitionHere()
//        {
//            return IsGettingHit && _hitTimer.GetElapsed() > TimeBeetweenHit;
//        }

//        protected override void OnStateMachineAttached()
//        {
//            base.OnStateMachineAttached();

//            _hitTimer.Start();
//        }

//        protected override void OnStateEnter()
//        {
//            base.OnStateEnter();

//            if(HitBodies == null) throw new NullReferenceException();
//            _hitBody = HitBodies;

//            _stunTimer.Start();
//            _stunSpeed = StunForce;

//            HealthBar.Current -= 5;
//        }

//        protected override void OnStateUpdate(GameTime gameTime)
//        {
//            base.OnStateUpdate(gameTime);

//            if (HealthBar.Current == 0)
//            {
//                OnHitDie();
//                return;
//            }
//            else
//            if (_stunTimer.GetElapsed() > StunTime)
//            {
//                OnHitEnd();
//            }

//            var stunDirection = Physic.Position - _hitBody.Position;
//            if (stunDirection.SqrMagnitude() > 0.1) stunDirection.Normalize();

//            Physic.LinearVelocity = stunDirection * _stunSpeed;
//            _stunSpeed *= 0.85f;
//            if (_stunSpeed < 0.5) _stunSpeed = 0.5f;
//        }

//        protected override void OnStateExit()
//        {
//            base.OnStateExit();

//            _hitTimer.Restart();
//            _stunTimer.Reset();

//            Physic.LinearVelocity = Vector2.Zero;
//        }

//        Stopwatch _hitTimer = new();
//        Stopwatch _stunTimer = new();

//        Body _hitBody;

//        float _stunSpeed;
//    }
//}
