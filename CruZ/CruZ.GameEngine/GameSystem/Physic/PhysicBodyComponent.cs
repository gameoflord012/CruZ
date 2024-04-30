using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.Physic
{
    public class PhysicBodyComponent : Component
    {
        public PhysicBodyComponent()
        {
            _body = BodyFactory.CreateBody(PhysicManager.World);
            _body.BodyType = BodyType.Dynamic;
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

        public Body Body { get => _body; }

        public Vector2 LinearVelocity
        {
            get => Body.LinearVelocity;
            set
            {
                Body.LinearVelocity = value;
                SyncTransform();
            }
        }

        public float AngularVelocity
        {
            get => Body.AngularVelocity;
            set
            {
                Body.AngularVelocity = value;
                SyncTransform();
            }
        }

        public Vector2 Postion
        {
            get => Body.Position;
            set => Body.Position = value;
        }

        Body _body;

        Transform Transform
        {
            get => _transform ?? throw new System.NullReferenceException();
            set => _transform = value;
        }

        Transform? _transform;

        public override void Dispose()
        {
            base.Dispose();

            _body.RemoveFromWorld();
        }
    }
}
