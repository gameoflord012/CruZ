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
    public class EntityTemplate
    {
        public  TransformEntity Entity  { get => _entity; }
        public  string          NameId  { get => _name; }

        public EntityTemplate(string name) { _name = name; }

        public virtual void GetInstruction(IBuildInstruction buildInstruction) { }

        public virtual void Initialize(TransformEntity relativeRoot) 
        {
            _entity = relativeRoot;

            _entity.RequireComponent(typeof(EntityEventComponent));
            var evComp = _entity.GetComponent<EntityEventComponent>();

            evComp.OnDraw += Draw;
            evComp.OnUpdate += Update;

            _entity.NameId = NameId;
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        TransformEntity? _entity;
        private string _name;
    }
}