using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Genbox.VelcroPhysics.Collision.ContactSystem;
using Genbox.VelcroPhysics.Collision.Handlers;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Physic
{
    public class PhysicBodyComponent : Component
    {
        public event OnCollisionHandler? OnCollision;

        public PhysicBodyComponent()
        {
            _body = BodyFactory.CreateBody(PhysicManager.World);
            _body.BodyType = BodyType.Dynamic;
            _body.OnCollision = OnCollisionHanlder;
        }

        void OnCollisionHanlder(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            OnCollision?.Invoke(fixtureA, fixtureB, contact);
        }

        internal void Update(GameTime gameTime)
        {
            SyncTransform();
        }

        private void SyncTransform()
        {
            if(_transform == null) return;
            _transform.Position = _body.Position;
            _transform.Rotation = _body.Rotation;
        }

        protected override void OnAttached(TransformEntity entity)
        {
            base.OnAttached(entity);

            _transform ??= entity.Transform;
            SyncTransform();
        }

        public BodyType BodyType
        {
            get => _body.BodyType;
            set => _body.BodyType = value;
        }

        public bool IsSensor
        {
            set => _body.IsSensor = value;
        }

        public bool Awake
        {
            get => _body.Awake;
            set => _body.Awake = value;
        }

        public object UserData
        {
            get => _body.UserData;
            set => _body.UserData = value;
        }

        public Vector2 LinearVelocity
        {
            get => _body.LinearVelocity;
            set
            {
                _body.LinearVelocity = value;
                SyncTransform();
            }
        }

        public float AngularVelocity
        {
            get => _body.AngularVelocity;
            set
            {
                _body.AngularVelocity = value;
                SyncTransform();
            }
        }
        public float Rotation
        {
            get => _body.Rotation;
            set
            {
                _body.Rotation = value;
                SyncTransform();
            }
        }

        public Vector2 Position
        {
            get => _body.Position;
            set
            {
                _body.Position = value;
                SyncTransform();
            }
        }

        public Vector2 Postion
        {
            get => _body.Position;
            set => _body.Position = value;
        }

        Body _body;

        public Transform Transform
        {
            get => _transform ?? throw new System.NullReferenceException();
            set => _transform = value;
        }
        public Body Body { get => _body; }

        Transform? _transform;

        public override void Dispose()
        {
            base.Dispose();
            _body.RemoveFromWorld();
        }
    }
}
