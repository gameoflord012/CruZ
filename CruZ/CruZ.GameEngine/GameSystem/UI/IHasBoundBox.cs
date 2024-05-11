using System;
using System.Collections.Generic;
using System.Drawing;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.UI
{
    public struct UIRect
    {
        public UIRect() { }

        public UIRect(WorldRectangle worldRect, Vector2[] origins)
        {
            WorldOrigins.AddRange(origins);
            WorldBound = worldRect;
        }

        /// <summary>
        /// UIRect to be drawn in world coord
        /// </summary>
        public WorldRectangle? WorldBound;

        /// <summary>
        /// WorldOrigins to be drawn in world coord
        /// </summary>
        public List<Vector2> WorldOrigins = [];
    }

    /// <summary>
    /// The draw back of this is the UIRect only know when action is invoking, if not, we not know the current value
    /// </summary>
    public interface IUIRectProvider
    {
        UIRect UIRect { get; }
    }
}
