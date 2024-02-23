using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace CruZ.Components
{
    class LightComponent : Component
    {
        public override Type ComponentType => typeof(LightComponent);

        public List<int> SortingLayers { get; } = [];

        internal void InternalDraw(GameTime gameTime)
        {

        }
    }
}