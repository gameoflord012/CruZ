using CruZ.Resource;
using CruZ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace CruZ.ECS
{
    public partial class SpriteComponent
    {
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if(_spriteResInfo != null)
            {
                LoadTexture(_spriteResInfo.ResourceName);
            }
        }
    }
}