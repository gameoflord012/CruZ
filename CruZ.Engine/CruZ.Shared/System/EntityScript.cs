using MonoGame.Extended.Entities;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using CruZ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CruZ.Components
{
    public class EntityScript : IComponent, IComponentCallback
    {
        [JsonIgnore]
        public Type ComponentType => typeof(EntityScript);

        public void OnEntityChanged(TransformEntity entity)
        {
            _e = entity;
            _e.OnDeserializationCompleted += InternalInit;
        }

        public void InternalInit()
        {
            OnInit();
        }

        public void InternalDraw(GameTime gameTime)
        {
            OnDraw(gameTime);
        }

        public void InternalUpdate(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(GameTime gameTime) { }

        protected TransformEntity AttachedEntity { get => _e; }

        TransformEntity _e;
    }
}