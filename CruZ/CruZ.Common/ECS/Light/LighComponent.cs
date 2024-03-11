using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CruZ.Common.ECS
{
    class LightComponent : Component
    {
        public override Type ComponentType => typeof(LightComponent);

        public List<int> SortingLayers { get; } = [];

        internal void InternalDraw(SpriteBatch sp, Matrix view)
        {
            
        }
    }
}