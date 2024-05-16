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
        public event OnCollisionHandler? OnSeperation;

        public PhysicBodyComponent()
        {
            _body = BodyFactory.CreateBody(PhysicManager.World);
            _body.BodyType = BodyType.Dynamic;
            _body.OnCollision = OnCollisionHanlder;
            _body.OnSeparation += OnSeperationHanlder;
            _body.SleepingAllowed = false;
        }

        private void OnSeperationHanlder(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            OnSeperation?.Invoke(fixtureA, fixtureB, contact);
        }

        private void OnCollisionHanlder(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            OnCollision?.Invoke(fixtureA, fixtureB, contact);
        }

        protected override void OnAttached(TransformEntity entity)
        {
            base.OnAttached(entity);

            _transform ??= entity.Transform;
            SyncTransform();
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

        public BodyType BodyType
        {
            get => _body.BodyType;
            set => _body.BodyType = value;
        }

        public bool IsSensor
        {
            set => _body.IsSensor = value;
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

        public Body Body { get => _body; }

        public Transform Transform
        {
            get => _transform ?? throw new System.NullReferenceException();
            set => _transform = value;
        }

        private Transform? _transform;
        private Body _body;

        public override void Dispose()
        {
            _body.RemoveFromWorld();
        }
    }
}
