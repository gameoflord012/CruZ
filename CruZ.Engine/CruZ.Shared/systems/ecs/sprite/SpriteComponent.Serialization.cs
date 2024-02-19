using CruZ.Resource;
using CruZ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace CruZ.Components
{
    public partial class SpriteComponent
    {
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if(_spriteResInfo != null)
                LoadTexture(_spriteResInfo.ResourceName);
        }
    }
}