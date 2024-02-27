using CruZ.Resource;
using CruZ.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace CruZ.Components
{
    public partial class SpriteComponent
    {
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if(_spriteResInfo != null)
            {
                Trace.Assert(_resource == _spriteResInfo.ResourceManager);
                LoadTexture(_spriteResInfo.ResourceName);
            }
        }
    }
}