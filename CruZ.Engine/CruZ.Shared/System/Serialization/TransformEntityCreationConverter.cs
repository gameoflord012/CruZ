using CruZ.Components;
using MonoGame.Extended.Entities;
using Newtonsoft.Json.Converters;
using System;

namespace CruZ.Serialization
{
    class TransformEntityCreationConverter : CustomCreationConverter<TransformEntity>
    {
        public override TransformEntity Create(Type objectType)
        {
            return MGWrapper.Instance().World.CreateTransformEntity();
        }
    }
}