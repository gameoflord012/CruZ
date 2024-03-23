using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.UI
{
    public struct UIBoundingBox
    {
        public UIBoundingBox()
        {
            WorldOrigins = [];
            WorldBounds = new(0, 0, 0, 0);
        }

        /// <summary>
        /// WorldBounds to be drawn in world coord
        /// </summary>
        public DRAW.RectangleF WorldBounds;
        /// <summary>
        /// WorldOrigins to be drawn in world coord
        /// </summary>
        public List<Vector2> WorldOrigins;

        public static UIBoundingBox Default => new UIBoundingBox();

        public bool IsEmpty()
        {
            return WorldBounds.IsEmpty;
        }
    }


    public interface IHasBoundBox
    {
        event Action<UIBoundingBox> BoundingBoxChanged;
    }
}
