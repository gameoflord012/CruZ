using CruZ_Engine.Components;
using MonoGame.Extended.Entities;
using Newtonsoft.Json.Converters;
using System;

namespace CruZ_Engine.Serialization
{
    class TransformEntityCreationConverter : CustomCreationConverter<TransformEntity>
    {
        public override TransformEntity Create(Type objectType)
        {
            return CruZ.Instance().World.CreateTransformEntity();
        }
    }
}